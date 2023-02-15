using SharedLibrary.DTO.Journal;
using SharedLibrary.Paging;

namespace Scales.Journal.Core.Services.Interfaces
{
    public interface IJournalService
    {
        Task<PagedList<TransportDto>> GetJournalAsync(PageParameters parameters, CancellationToken ct);
        Task<PagedList<TransportDto>> GetJournalByDatesAsync(PageParameters parameters, DateTime startDate, DateTime endDate, CancellationToken ct);
        Task SaveWeighingDataAsync(TransportDto transportDto, CancellationToken ct);
    }
}
