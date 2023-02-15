using Scales.Journal.Domain.Entities;
using Scales.Journal.Domain.Repositories.Interfaces;

namespace Scales.Journal.Domain.Repositories.Implementations
{
    public class TransportRepository : GenericRepository<Transport>, ITransportRepository
    {
        public TransportRepository(JournalDbContext dbContext) : base(dbContext)
        {

        }
    }
}
