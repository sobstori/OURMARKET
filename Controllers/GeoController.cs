using Microsoft.AspNetCore.Mvc;
using OurMarketBackend.Services;

namespace OurMarketBackend.Controllers
{
    [Route("api/geo")]
    [ApiController]
    public class GeoController : ControllerBase
    {
        private readonly IZipLookup _zipLookup;

        public GeoController(IZipLookup zipLookup)
        {
            _zipLookup = zipLookup;
        }

        [HttpGet("zip/{zip}")]
        public IActionResult FromZip(string zip)
        {
            if (_zipLookup.TryGet(zip, out var info))
                return Ok(new { city = info.City, state = info.State });

            return NotFound();
        }
    }
}
