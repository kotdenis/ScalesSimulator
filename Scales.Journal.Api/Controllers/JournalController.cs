using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scales.Journal.Core.Infrastructure;
using Scales.Journal.Core.Services.Interfaces;
using SharedLibrary.DTO.Journal;
using SharedLibrary.Paging;

namespace Scales.Journal.Api.Controllers
{
    [Route("api/journal/[controller]")]
    [ApiController]
    [Authorize]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService _journalService;
        private readonly IWeighingSimulator _weighingSimulator;
        public JournalController(IJournalService journalService, IWeighingSimulator weighingSimulator)
        {
            _journalService = journalService;
            _weighingSimulator = weighingSimulator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<TransportDto>>> GetJournal([FromQuery] PageParameters parameters, CancellationToken ct = default)
        {
            var result = await _journalService.GetJournalAsync(parameters, ct);
            var value = System.Text.Json.JsonSerializer.Serialize(result.Metadata);
            Response.Headers.Add("X-Pagination", value);
            return Ok(result);
        }

        [HttpGet("bydate")]
        public async Task<ActionResult<PagedList<TransportDto>>> GetJournalByDate([FromQuery] PageParameters parameters,
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken ct = default)
        {
            var result = await _journalService.GetJournalByDatesAsync(parameters, startDate, endDate, ct);
            var value = System.Text.Json.JsonSerializer.Serialize(result.Metadata);
            Response.Headers.Add("X-Pagination", value);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWeighingData([FromBody] TransportDto transportDto, CancellationToken ct = default)
        {
            await _journalService.SaveWeighingDataAsync(transportDto, ct);
            return Ok();
        }

        [HttpGet("transport")]
        public ActionResult<TransportForWeghing> GetTransport()
        {
            var transport = _weighingSimulator.GenerateTransportDataForWeighing();
            return Ok(transport);
        }
    }
}
