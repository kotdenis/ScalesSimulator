using Microsoft.EntityFrameworkCore;
using Scales.Journal.Domain.Entities;
using Scales.Journal.Domain.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Scales.Journal.Domain.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        protected readonly JournalDbContext _dbContext;
        public GenericRepository(JournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (predicate != null)
                query = query.Where(predicate).AsQueryable();
            if (orderBy != null)
                query = orderBy(query);
            await Task.CompletedTask;
            return query.ToList();
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id, ct);
            if (entity != null)
                return entity;
            else
                throw new ArgumentException(nameof(GetByIdAsync));
        }

        public Task CreateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(CreateAsync));
            _dbContext.Entry(entity).State = EntityState.Added;
            return Task.CompletedTask;
        }

        public async Task SoftDeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id, ct);
            if (entity == null)
                throw new ArgumentNullException(nameof(SoftDeleteAsync));
            if (entity is BaseEntity baseEntity)
                baseEntity.IsDeleted = true;
        }

        public Task UpdateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(UpdateAsync));
            _dbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
            return _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, ct)!;
        }

        public IQueryable<TEntity> GetAsQueryable(CancellationToken ct)
        {
            var set = _dbContext.Set<TEntity>();
            return set;
        }
    }
}
