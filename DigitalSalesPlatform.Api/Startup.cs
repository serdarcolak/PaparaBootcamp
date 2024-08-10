using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Autofac;
using AutoMapper;
using DigitalSalesPlatform.Api.Middleware;
using DigitalSalesPlatform.Base;
using DigitalSalesPlatform.Base.Email;
using DigitalSalesPlatform.Base.Log;
using DigitalSalesPlatform.Base.Token;
using DigitalSalesPlatform.Business;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Job;
using DigitalSalesPlatform.Business.Notification;
using DigitalSalesPlatform.Business.RabbitMq;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Business.Token;
using DigitalSalesPlatform.Business.Validation;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Data.Context;
using DigitalSalesPlatform.Data.UnitOfWork;
using FluentValidation.AspNetCore;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using Module = Autofac.Module;

namespace DigitalSalesPlatform.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public static JwtConfig JwtConfig { get; private set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.AddSingleton(JwtConfig);

        var connectionStringSql = Configuration.GetConnectionString("MsSqlConnection");
        services.AddDbContext<DigitalSalesPlatformDbContext>(options => options.UseSqlServer(connectionStringSql));

        var connectionStringPostgre = Configuration.GetConnectionString("PostgresSqlConnection");
        services.AddDbContext<DigitalSalesPlatformDbContext>(options => options.UseNpgsql(connectionStringPostgre));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IOrderCalculationService, OrderCalculationService>();
        services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IRabbitMQService, RabbitMQService>();
        services.AddTransient<EmailProcessorJob>();
        
        var redisConnection = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
        services.AddSingleton<IConnectionMultiplexer>(redisConnection);
        
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
        
        services.Configure<SmtpSettings>(Configuration.GetSection("Smtp"));
        
        services.AddScoped<INotificationService, NotificationService>();
        
        services.AddControllers().AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<BaseValidator>();
        });
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(config.CreateMapper());
        
        services.AddMediatR(typeof(CreateUserCommand).GetTypeInfo().Assembly);
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = JwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.Secret)),
                ValidAudience = JwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DigitalSalesPlatform Management", Version = "v1.0" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "DigitalSalesPlatform for IT Company",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        
        services.AddHttpContextAccessor();
        
        services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();
        
        services.AddScoped<ISessionContext>(provider =>
        {
            var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            var sessionContext = new SessionContext
            {
                Session = JwtManager.GetSession(contextAccessor.HttpContext),
                HttpContext = contextAccessor.HttpContext
            };
            return sessionContext;
        });
        
        services.AddMemoryCache();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new AutofacBusinessModule());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DigitalSalesPlatform.Api v1"));
        }
        
        app.UseMiddleware<HeartbeatMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
        {
            Log.Information("-------------Request-Begin------------");
            Log.Information(requestProfilerModel.Request);
            Log.Information(Environment.NewLine);
            Log.Information(requestProfilerModel.Response);
            Log.Information("-------------Request-End------------");
        };
        app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
    }
}
