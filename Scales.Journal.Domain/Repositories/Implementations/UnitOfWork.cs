using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Scales.Journal.Domain.Repositories.Implementations
{
    public class UnitOfWork : IDisposable
    {
        private readonly JournalDbContext _dbContext;
        public UnitOfWork(JournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SaveAsync(CancellationToken ct)
        {
            return await _dbContext.SaveChangesAsync(ct) >= 0;
        }

        public Task<IDbContextTransaction> EnsureTransactionAsync(IsolationLevel isolationLevel, CancellationToken ct = default)
        {
            IDbContextTransaction transaction = _dbContext.Database.CurrentTransaction!;
            if (transaction == null)
                return _dbContext.Database.BeginTransactionAsync(isolationLevel, ct);
            return Task.FromResult(transaction);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
