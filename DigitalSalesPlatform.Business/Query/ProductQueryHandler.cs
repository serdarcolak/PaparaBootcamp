using AutoMapper;
using DigitalSalesPlatform.Base;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;

namespace DigitalSalesPlatform.Business.Query;

public class ProductQueryHandler :
    IRequestHandler<GetAllProductsQuery, ApiResponse<List<ProductResponse>>>,
    IRequestHandler<GetProductByIdQuery, ApiResponse<ProductResponse>>,
    IRequestHandler<GetProductsByCategoryQuery, ApiResponse<List<ProductResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionContext _sessionContext;

    public ProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ISessionContext sessionContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionContext = sessionContext;
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.ProductRepository.GetAll("ProductCategories", "ProductCategories.Category");
        var mapped = _mapper.Map<List<ProductResponse>>(products);
        return new ApiResponse<List<ProductResponse>>(mapped);
    }

    public async Task<ApiResponse<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetById(request.ProductId, "ProductCategories", "ProductCategories.Category");
        var mapped = _mapper.Map<ProductResponse>(product);
        return new ApiResponse<ProductResponse>(mapped);
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.ProductRepository.Where(
            p => p.ProductCategories.Any(pc => pc.CategoryId == request.CategoryId), 
            "ProductCategories", "ProductCategories.Category"
        );
        var mapped = _mapper.Map<List<ProductResponse>>(products);
        return new ApiResponse<List<ProductResponse>>(mapped);
    }
}
