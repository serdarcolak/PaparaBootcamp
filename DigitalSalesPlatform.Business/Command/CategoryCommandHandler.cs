using AutoMapper;
using DigitalSalesPlatform.Base.Response;
using DigitalSalesPlatform.Business.Cqrs;
using DigitalSalesPlatform.Business.Service;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;
using MediatR;
using Newtonsoft.Json;

namespace DigitalSalesPlatform.Business.Command;

public class CategoryCommandHandler : 
    IRequestHandler<CreateCategoryCommand, ApiResponse<CategoryResponse>>,
    IRequestHandler<UpdateCategoryCommand, ApiResponse>,
    IRequestHandler<DeleteCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisCacheService _redisCacheService;

    public CategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService redisCacheService)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
        this._redisCacheService = redisCacheService;
    }

    public async Task<ApiResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request.Request);
        await _unitOfWork.CategoryRepository.Insert(category);
        await _unitOfWork.Complete();
        
        var categoryResponse = _mapper.Map<CategoryResponse>(category);
        await _redisCacheService.SetValueAsync($"category_{category.Id}", JsonConvert.SerializeObject(categoryResponse));
        return new ApiResponse<CategoryResponse>(categoryResponse);
    }

    public async Task<ApiResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetById(request.CategoryId);
        if (category == null)
        {
            return new ApiResponse();
        }
        _mapper.Map(request.Request, category);
        _unitOfWork.CategoryRepository.Update(category);
        await _unitOfWork.Complete();
        
        var updatedCategoryResponse = _mapper.Map<CategoryResponse>(category);
        await _redisCacheService.SetValueAsync($"category_{category.Id}", JsonConvert.SerializeObject(updatedCategoryResponse));
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.CategoryRepository.Delete(request.CategoryId);
        await _unitOfWork.Complete();
        await _redisCacheService.Clear($"category_{request.CategoryId}");
        return new ApiResponse();
    }
}
