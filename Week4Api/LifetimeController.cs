using Microsoft.AspNetCore.Mvc;

namespace Week4Api
{
    [ApiController]
    [Route("[controller]")]
    public class LifetimeController : ControllerBase
    {
        private readonly ITransientCounter _transient1;
        private readonly ITransientCounter _transient2;
        private readonly IScopedCounter _scoped1;
        private readonly IScopedCounter _scoped2;
        private readonly ISingletonCounter _singleton;

        public LifetimeController(
            ITransientCounter transient1,
            ITransientCounter transient2,
            IScopedCounter scoped1,
            IScopedCounter scoped2,
            ISingletonCounter singleton
            )
        {
            _transient1 = transient1;
            _transient2 = transient2;
            _scoped1 = scoped1;
            _scoped2 = scoped2;
            _singleton = singleton;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Transient1Id = _transient1.Id,
                Transient2Id = _transient2.Id,  // different from Transient1
                Scoped1Id = _scoped1.Id,
                Scoped2Id = _scoped2.Id,     // same as Scoped1 within request
                SingletonId = _singleton.Id    // same across all requests

            });
        }
    }
}




