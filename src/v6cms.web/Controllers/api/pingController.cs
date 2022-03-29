using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace v6cms.web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class pingController : ControllerBase
    {
        // GET: api/<pingController>
        [HttpGet]
        public string Get()
        {
            return "pong";
        }
    }
}
