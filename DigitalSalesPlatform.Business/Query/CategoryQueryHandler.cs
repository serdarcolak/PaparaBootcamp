using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Query;

public class CategoryQueryHandler : IRequestHandler<GetAllCategoriesQuery, ApiResponse<List<CategoryResponse>>>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ApiResponse<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAll();
        var mapped = _mapper.Map<List<CategoryResponse>>(categories);
        return new ApiResponse<List<CategoryResponse>>(mapped);
    }
}