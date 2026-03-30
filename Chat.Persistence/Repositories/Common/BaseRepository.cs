using System.Linq.Expressions;
using Chat.Domain.Entities;
using Chat.Domain.Shared.Models;
using Chat.Domain.Specification;
using Chat.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories.Common;

public class BaseRepository<TEntity> : IAsyncRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(EFContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<TEntity?> GetAsync(ISingleSpecification<TEntity> specification)
    {
        return await _dbSet.GetQueryForOneAsync(specification);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity>? specification
    )
    {
        return await _dbSet.GetQueryForManyAsync(specification ?? new DefaultSpec<TEntity>());
    }

    public virtual async Task<PaginatorResponse<TEntity>> GetPaginatedAsync(
        ISpecification<TEntity> specification,
        Pagination? pagination
    )
    {
        IQueryable<TEntity> query = await _dbSet.GetQueryForManyAsync(specification);

        int itemCount = await query.CountAsync();

        int skip = pagination?.Skip ?? 0;
        int pageSize = pagination?.PageSize ?? itemCount;
        int pageNumber = pagination?.PageNumber ?? 0;
        int pagesCount = pageSize > 0 ? (int)Math.Ceiling((float)itemCount / pageSize) : 0;

        List<TEntity> collection = pagination != null
            ? await query.Skip(pageNumber * pageSize + skip).Take(pageSize).ToListAsync()
            : await query.ToListAsync();

        var meta = new MetaResponse
        {
            ItemsCount = itemCount,
            PagesCount = pagesCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return new PaginatorResponse<TEntity>(collection, meta);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public virtual async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AllAsync(predicate);
    }
}
