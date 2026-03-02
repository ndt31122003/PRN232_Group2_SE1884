using Microsoft.AspNetCore.Mvc;

namespace PRN232_EbayClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRateLimitController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Request successful",
                Timestamp = DateTime.UtcNow
            });
        }

    }
}
