using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSalesPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IMediator _mediator;


    public AuthorizationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<ApiResponse<AuthorizationResponse>> Post([FromBody] AuthorizationRequest value)
    {
        var operation = new CreateAuthorizationTokenCommand(value);
        var result = await _mediator.Send(operation);
        return result;
        
    }
}