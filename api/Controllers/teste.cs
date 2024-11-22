using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        // GET: api/example
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello, Swagger!" });
        }
    }
}