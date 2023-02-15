using SharedLibrary.DTO.ReferenceBook;
using SharedLibrary.Paging;

namespace Scales.ReferenceBook.Core.Services.Interfaces
{
    public interface IRefTransportService
    {
        Task<PagedList<RefTransportDto>> GetReferanceBookAsync(PageParameters parameters, CancellationToken ct);
        Task UpdateRefTransportAsync(RefTransportDto refTransportDto, CancellationToken ct);
        Task CreateRefTransportAsync(RefTransportDto refTransportDto, CancellationToken ct);
    }
}
