using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Helper;
using DigitalSalesPlatform.Business.Job;
using DigitalSalesPlatform.Business.Notification;
using DigitalSalesPlatform.Business.RabbitMq;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using Hangfire;
using MediatR;
using Newtonsoft.Json;

namespace DigitalSalesPlatform.Business.Command;

public class UserCommandHandler : 
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse>,
    IRequestHandler<DeleteUserCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IRabbitMQService _rabbitMQService;
    private readonly IRedisCacheService _redisCacheService;

    public UserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService, 
        IRabbitMQService rabbitMqService, IRedisCacheService redisCacheService)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
        this._notificationService = notificationService;
        this._rabbitMQService = rabbitMqService;
        this._redisCacheService = redisCacheService;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<UserRequest, User>(request.Request);
        mapped.Password = SecurityHelper.CreateMD5(mapped.Password);
        await _unitOfWork.UserRepository.Insert(mapped);
        await _unitOfWork.Complete();
        
        if (mapped.Role != "admin")
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefault(u => u.Email == request.Request.Email);

            BackgroundJob.Schedule(() => 
                    SendEmail(user.Email, $"{user.FirstName} {user.LastName}", request.Request.WalletBalance.ToString()),
                TimeSpan.FromSeconds(30));

            var message = new EmailMessage
            {
                Email = user.Email,
                Subject = "Welcome!",
                Body = "Thank you for registering."
            };
        
            await _rabbitMQService.PublishToQueue(message, "emailQueue");
        }
        
        var response = _mapper.Map<UserResponse>(mapped);
        await _redisCacheService.SetValueAsync($"user_{mapped.Id}", JsonConvert.SerializeObject(response));
        return new ApiResponse<UserResponse>(response);
    }
    
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new []{ 10, 15, 18 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public void SendEmail(string email, string name, string walletBalance)
    {
        _notificationService.SendEmail(email, "Hesap Acilisi", $"Merhaba, {name}, Adiniza {walletBalance} bakiyeli hesabiniz acilmistir.").Wait();
    }


    public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<UserRequest, User>(request.Request);
        mapped.Id = request.UserId;
        mapped.Password = SecurityHelper.CreateMD5(request.Request.Password);
        _unitOfWork.UserRepository.Update(mapped);
        await _unitOfWork.Complete();
        
        var response = _mapper.Map<UserResponse>(mapped);
        await _redisCacheService.SetValueAsync($"user_{mapped.Id}", JsonConvert.SerializeObject(response));
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.Delete(request.UserId);
        await _unitOfWork.Complete();
        await _redisCacheService.Clear($"user_{request.UserId}");
        return new ApiResponse();
    }
}