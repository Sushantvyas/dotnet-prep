using Microsoft.AspNetCore.Mvc;

namespace Week4Api
{
    [ApiController]
    [Route("[controller]")]
    public class NotifyController : ControllerBase
    {
        [HttpPost("notify")]
        public IActionResult Notify(
            [FromBody] string message,
            [FromServices] IEnumerable<INotifier> notifiers)
        {
            var results = notifiers.Select(u => u.Send(message)).ToList();
            return Ok(results);
        }

        [HttpGet("notify-test")]          // ← moved INSIDE the class
        public IActionResult NotifyTest(
            [FromServices] IEnumerable<INotifier> notifiers)
        {
            var results = notifiers.Select(n => n.Send("Order confirmed")).ToList();
            return Ok(results);
        }

        [HttpGet("error-test")]
        public IActionResult ThrowError()
        {
            throw new InvalidOperationException("This is a test exception");
        }

    }   // ← one closing brace for the class
}       // ← one closing brace for the namespace