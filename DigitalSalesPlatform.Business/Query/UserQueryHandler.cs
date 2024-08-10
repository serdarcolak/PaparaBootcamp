using AutoMapper;
using DigitalSalesPlatform.Base;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Query;

public class UserQueryHandler :
    IRequestHandler<GetAllUserQuery,ApiResponse<List<UserResponse>>>,
    IRequestHandler<GetUserByIdQuery,ApiResponse<UserResponse>>,
    IRequestHandler<GetPointQuery,ApiResponse<UserPointResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionContext _sessionContext;

    public UserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,ISessionContext sessionContext)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
        this._sessionContext = sessionContext;
    }
    
    public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        List<User> entityList = await _unitOfWork.UserRepository.GetAll("Orders");
        var mappedList = _mapper.Map<List<UserResponse>>(entityList);
        return new ApiResponse<List<UserResponse>>(mappedList);
    }

    public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.UserRepository.GetById(request.UserId,"Orders");
        var mapped = _mapper.Map<UserResponse>(entity);
        return new ApiResponse<UserResponse>(mapped);
    }

    public async Task<ApiResponse<UserPointResponse>> Handle(GetPointQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.UserRepository.GetById(request.UserId);
        var mapped = _mapper.Map<UserPointResponse>(entity);
        return new ApiResponse<UserPointResponse>(mapped);
    }
}