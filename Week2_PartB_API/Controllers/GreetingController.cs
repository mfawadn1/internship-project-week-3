using Microsoft.AspNetCore.Mvc;

namespace Week2_PartB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GreetingController : ControllerBase
    {
        // GET: api/greeting
        // Returns 200 OK
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello! This is a GET endpoint returning 200 OK." });
        }

        // GET: api/greeting/{id}
        // Returns 200 OK or 404 Not Found
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return NotFound(new { Error = "Greeting not found (404)." });
            }
            return Ok(new { Message = $"Hello user {id}!" });
        }

        // POST: api/greeting
        // Returns 201 Created or 400 Bad Request
        [HttpPost]
        public IActionResult CreateGreeting([FromBody] GreetingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Name))
            {
                return BadRequest(new { Error = "Name is required (400)." });
            }

            var resourceUrl = $"/api/greeting/1"; // Fake URL for the created resource
            return Created(resourceUrl, new { Message = $"Greeting created for {request.Name}! (201)" });
        }
    }

    public class GreetingRequest
    {
        public string Name { get; set; }
    }
}
