using Scales.ReferenceBook.Domain.Entities;
using Scales.ReferenceBook.Domain.Repositories.Interfaces;

namespace Scales.ReferenceBook.Domain.Repositories.Implementations
{
    public class ReferenceTransportRepository : GenericRepository<ReferenceTransport>, IReferenceTransportRepository
    {
        public ReferenceTransportRepository(ReferenceBookDbContext dbContext) : base(dbContext) { }
    }
}
