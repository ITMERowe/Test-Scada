using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InfluxDBWebApp.Services;

namespace InfluxDBWebApp.Controllers
{
    [Route("api/influx")]
    [ApiController]
    public class InfluxController : ControllerBase
    {
        private readonly InfluxDBService _influxDBService;

        public InfluxController(InfluxDBService influxDBService)
        {
            _influxDBService = influxDBService;
        }

        [HttpGet("cpu-usage")]
        public async Task<IActionResult> GetCpuUsage()
        {
            var data = await _influxDBService.GetCpuUsageAsync();
            return Ok(data);
        }
    }
}
