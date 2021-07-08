using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MonitoringService
{
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        public HelloController(MonitoringSettings monitoringSettings)
        {
            
        }

        [HttpGet]
        public async Task<string> DoesntMatter()
        {
            return await Task.FromResult("World!");
        }
    }
}