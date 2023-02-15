using Scales.Journal.Domain.Entities;
using Scales.Journal.Domain.Repositories.Interfaces;

namespace Scales.Journal.Domain.Repositories.Implementations
{
    public class AxlesRepository : GenericRepository<Axles>, IAxlesRepository
    {
        public AxlesRepository(JournalDbContext dbContext) : base(dbContext) { }

    }
}
