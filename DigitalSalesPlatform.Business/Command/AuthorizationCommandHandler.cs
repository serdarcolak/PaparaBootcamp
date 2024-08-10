using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Helper;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Business.Token;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;
using Newtonsoft.Json;

namespace DigitalSalesPlatform.Business.Command;

public class AuthorizationCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, ApiResponse<AuthorizationResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private ITokenService _tokenService;

    public AuthorizationCommandHandler(IUnitOfWork unitOfWork,ITokenService tokenService)
    {
        this._unitOfWork = unitOfWork;
        this._tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthorizationResponse>> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefault(x => x.Email == request.Request.Email);
        if (user is null)
            return new ApiResponse<AuthorizationResponse>("Invalid user informations. Check your email or password. E1");

        if (user.Password != SecurityHelper.CreateMD5(request.Request.Password))
        {
            return new ApiResponse<AuthorizationResponse>("Invalid user informations. Check your email or password. E1");
        }

        var token = await _tokenService.GetToken(user);
        AuthorizationResponse response = new AuthorizationResponse()
        {
            ExpireTime = DateTime.Now.AddMinutes(5),
            AccessToken = token,
            Email = user.Email
        };
        
        return new ApiResponse<AuthorizationResponse>(response);
    }
}