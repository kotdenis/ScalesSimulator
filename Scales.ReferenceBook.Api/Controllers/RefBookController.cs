using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scales.ReferenceBook.Core.Services.Interfaces;
using SharedLibrary.DTO.ReferenceBook;
using SharedLibrary.Paging;

namespace Scales.ReferenceBook.Api.Controllers
{
    [Route("api/refbook/[controller]")]
    [ApiController]
    [Authorize]
    public class RefBookController : ControllerBase
    {
        private readonly IRefTransportService _refTransportService;
        public RefBookController(IRefTransportService refTransportService)
        {
            _refTransportService = refTransportService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<RefTransportDto>>> GetRefTransport([FromQuery] PageParameters parameters, CancellationToken ct = default)
        {
            var result = await _refTransportService.GetReferanceBookAsync(parameters, ct);
            var value = System.Text.Json.JsonSerializer.Serialize(result.Metadata);
            Response.Headers.Add("X-Pagination", value);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RefTransportDto refTransportDto, CancellationToken ct = default)
        {
            await _refTransportService.CreateRefTransportAsync(refTransportDto, ct);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RefTransportDto refTransportDto, CancellationToken ct = default)
        {
            await _refTransportService.UpdateRefTransportAsync(refTransportDto, ct);
            return Ok();
        }
    }
}
