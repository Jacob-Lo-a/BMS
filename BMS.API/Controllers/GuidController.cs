using BMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuidController : ControllerBase
    {
        private readonly IGuidGenerator _g1;
        private readonly IGuidGenerator _g2;

        public GuidController(IGuidGenerator g1, IGuidGenerator g2)
        {
            _g1 = g1;
            _g2 = g2;
        }

        [HttpGet("guid")]
        public IActionResult GetValue()
        {
            return Ok($"{_g1.GetGuid()} | {_g2.GetGuid()}");
        }
    }
}
