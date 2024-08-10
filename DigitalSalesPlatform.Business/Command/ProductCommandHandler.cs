using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;
using Newtonsoft.Json;

namespace DigitalSalesPlatform.Business.Command;

public class ProductCommandHandler : 
    IRequestHandler<CreateProductCommand, ApiResponse<ProductResponse>>,
    IRequestHandler<UpdateProductCommand, ApiResponse>,
    IRequestHandler<DeleteProductCommand, ApiResponse>,
    IRequestHandler<UpdateStockCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisCacheService _redisCacheService;

    public ProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService redisCacheService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.Request);
        await _unitOfWork.ProductRepository.Insert(product);
        await _unitOfWork.Complete();

        if (request.Request.CategoryIds != null && request.Request.CategoryIds.Any())
        {
            foreach (var categoryId in request.Request.CategoryIds)
            {
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId,
                    InsertUser = "System"
                };
                await _unitOfWork.ProductCategoryRepository.Insert(productCategory);
            }
            await _unitOfWork.Complete();
        }
        
        var productResponse = _mapper.Map<ProductResponse>(product);
        await _redisCacheService.SetValueAsync($"product_{product.Id}", JsonConvert.SerializeObject(productResponse));
        return new ApiResponse<ProductResponse>(productResponse);
    }

    public async Task<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetById(request.ProductId);
        if (product == null)
        {
            return new ApiResponse();
        }
        _mapper.Map(request.Request, product);
        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.Complete();
        var productResponse = _mapper.Map<ProductResponse>(product);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProductRepository.Delete(request.ProductId);
        await _unitOfWork.Complete();
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetById(request.ProductId);
        if (product == null)
        {
            return new ApiResponse();
        }
        product.Stock = request.Stock;
        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.Complete();
        return new ApiResponse();
    }
}
