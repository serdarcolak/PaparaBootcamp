using DigitalSalesPlatform.Data.Context;
using DigitalSalesPlatform.Data.Repository;

namespace DigitalSalesPlatform.Data.UnitOfWork;

public class UnitOfWork:IUnitOfWork,IDisposable
{
    private readonly DigitalSalesPlatformDbContext _dbContext;
    
    public IGenericRepository<User> UserRepository { get; }
    public IGenericRepository<Category> CategoryRepository { get; }
    public IGenericRepository<Coupon> CouponRepository { get; }
    public IGenericRepository<Order> OrderRepository { get; }
    public IGenericRepository<OrderDetail> OrderDetailRepository { get; }
    public IGenericRepository<Product> ProductRepository { get; }
    public IGenericRepository<ProductCategory> ProductCategoryRepository { get; }

    public UnitOfWork(DigitalSalesPlatformDbContext dbContext)
    {
        this._dbContext = dbContext;
        
        UserRepository = new GenericRepository<User>(this._dbContext);
        CategoryRepository = new GenericRepository<Category>(this._dbContext);
        CouponRepository = new GenericRepository<Coupon>(this._dbContext);
        OrderRepository = new GenericRepository<Order>(this._dbContext);
        OrderDetailRepository = new GenericRepository<OrderDetail>(this._dbContext);
        ProductRepository = new GenericRepository<Product>(this._dbContext);
        ProductCategoryRepository = new GenericRepository<ProductCategory>(this._dbContext);

    }

    public async Task Complete()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task CompleteWithTransaction()
    {
        using (var dbTransaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                Console.WriteLine(ex);
                throw;
            }
        }
    }
    
    public void Dispose()
    {
        
    }
}