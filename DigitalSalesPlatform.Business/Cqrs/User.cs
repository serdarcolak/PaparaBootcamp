using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Cqrs;

public record CreateUserCommand(UserRequest Request) : IRequest<ApiResponse<UserResponse>>;
public record UpdateUserCommand(int UserId,UserRequest Request) : IRequest<ApiResponse>;
public record DeleteUserCommand(int UserId) : IRequest<ApiResponse>;
public record GetAllUserQuery() : IRequest<ApiResponse<List<UserResponse>>>;
public record GetUserByIdQuery(int UserId) : IRequest<ApiResponse<UserResponse>>;
public record GetPointQuery(int UserId) : IRequest<ApiResponse<UserPointResponse>>;