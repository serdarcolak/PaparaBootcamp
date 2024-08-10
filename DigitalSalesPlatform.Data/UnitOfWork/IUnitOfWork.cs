using DigitalSalesPlatform.Data.Repository;

namespace DigitalSalesPlatform.Data;

public interface IUnitOfWork
{
    Task Complete(); 
    Task CompleteWithTransaction();
    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Category> CategoryRepository { get; }
    IGenericRepository<Coupon> CouponRepository { get; }
    IGenericRepository<Order> OrderRepository { get; }
    IGenericRepository<OrderDetail> OrderDetailRepository { get; }
    IGenericRepository<Product> ProductRepository { get; }
    
    IGenericRepository<ProductCategory> ProductCategoryRepository { get; }
    
}