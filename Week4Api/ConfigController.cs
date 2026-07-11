using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Week4Api.Settings;

namespace Week4Api
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly EmailSettings _options;
        private readonly IOptionsMonitor<EmailSettings> _monitor;
        public ConfigController(IOptions<EmailSettings> options,
            IOptionsMonitor<EmailSettings> monitor)
        {
            _options = options.Value;
            _monitor = monitor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                FromIOptions = new
                {
                    _options.SmtpHost,
                    _options.Port,
                    _options.UseSsl,
                    _options.Username
                },
                FromIOptionsMonitor = new
                {
                    _monitor.CurrentValue.SmtpHost,
                    _monitor.CurrentValue.Port
                }
            });
        }
    }
}
