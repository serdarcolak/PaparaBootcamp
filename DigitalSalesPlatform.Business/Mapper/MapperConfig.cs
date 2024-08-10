using AutoMapper;
using DigitalSalesPlatform.Data;
using DigitalSalesPlatform.Schema;

namespace DigitalSalesPlatform.Business;

public class MapperConfig : Profile
{

    public MapperConfig()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
        
        CreateMap<User, UserPointResponse>();
        
        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryRequest, Category>();
        
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)));
        CreateMap<ProductRequest, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

        CreateMap<Coupon, CouponResponse>();
        CreateMap<CouponRequest, Coupon>();

        CreateMap<Order, OrderResponse>();
        CreateMap<OrderRequest, Order>();

        CreateMap<OrderDetail, OrderDetailResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<OrderDetailRequest, OrderDetail>();
        
        CreateMap<Order, OrderReportResponse>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
        CreateMap<OrderDetail, OrderDetailReportResponse>();
        
        CreateMap<OrderDigitalWalletRequest, Order>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CouponAmount, opt => opt.Ignore())
            .ForMember(dest => dest.PointsUsed, opt => opt.Ignore())
            .ForMember(dest => dest.PointsEarned, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());
        
        CreateMap<OrderCreditCardRequest, Order>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CouponAmount, opt => opt.Ignore())
            .ForMember(dest => dest.PointsUsed, opt => opt.Ignore())
            .ForMember(dest => dest.PointsEarned, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());
        
    }
}