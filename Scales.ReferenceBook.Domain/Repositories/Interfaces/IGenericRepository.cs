using System.Linq.Expressions;

namespace Scales.ReferenceBook.Domain.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, CancellationToken ct = default);
        Task<TEntity> GetByIdAsync(int id, CancellationToken ct);
        Task CreateAsync(TEntity entity, CancellationToken ct);
        Task SoftDeleteAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(TEntity entity, CancellationToken ct);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
        IQueryable<TEntity> GetAsQueryable(CancellationToken ct);
    }
}
