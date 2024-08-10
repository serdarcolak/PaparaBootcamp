using System.Linq.Expressions;
using DigitalSalesPlatform.Base.Entity;
using DigitalSalesPlatform.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalSalesPlatform.Data.Repository;

internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DigitalSalesPlatformDbContext _dbContext;

    public GenericRepository(DigitalSalesPlatformDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> GetById(long Id, params string[] includes)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, x => x.Id == Id);
    }

    public async Task<TEntity> Insert(TEntity entity)
    {
        entity.IsActive = true;
        entity.InsertDate = DateTime.UtcNow;
        entity.InsertUser = "System";
        await _dbContext.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public void Update(TEntity entity)
    {
        entity.IsActive = true;
        entity.InsertDate = DateTime.UtcNow;
        entity.InsertUser = "System";
        _dbContext.Set<TEntity>().Update(entity);
    }
    
    public async Task Delete(long Id)
    {
        var entity = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_dbContext.Set<TEntity>(), x => x.Id == Id);
        if (entity is not null)
            _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression,params string[] includes)
    {
        var query = _dbContext.Set<TEntity>().Where(expression).AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.ToListAsync(query);
    }
    
    public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression,params string[] includes)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return query.Where(expression).FirstOrDefault();
    }

    public async Task<List<TEntity>> GetAll(params string[] includes)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await EntityFrameworkQueryableExtensions.ToListAsync(query);
    }
}