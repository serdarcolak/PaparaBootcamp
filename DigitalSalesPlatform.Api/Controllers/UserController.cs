using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSalesPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<UserResponse>>> Get()
    {
        var operation = new GetAllUserQuery();
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpGet("{UserId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<UserResponse>> Get([FromRoute] int UserId)
    {
        var operation = new GetUserByIdQuery(UserId);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpGet("user/point/{UserId}")]
    [Authorize(Roles = "admin,user")]
    public async Task<ApiResponse<UserPointResponse>> GetPointUser([FromRoute] int UserId)
    {
        var operation = new GetPointQuery(UserId);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost("user/register")]
    [AllowAnonymous]
    public async Task<ApiResponse<UserResponse>> RegisterUser([FromBody] UserRequest value)
    {
        value.Role = "user";
        var operation = new CreateUserCommand(value);
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost("admin/register")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<UserResponse>> RegisterAdmin([FromBody] UserRequest value)
    {
        value.Role = "admin";
        var operation = new CreateUserCommand(value);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{UserId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Put(int UserId, [FromBody] UserRequest value)
    {
        var operation = new UpdateUserCommand(UserId,value);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{UserId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete(int UserId)
    {
        var operation = new DeleteUserCommand(UserId);
        var result = await _mediator.Send(operation);
        return result;
    }
} 