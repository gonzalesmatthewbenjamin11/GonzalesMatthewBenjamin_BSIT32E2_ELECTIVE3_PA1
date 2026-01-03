using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("weatherforecast")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private static List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // GET all weather forecasts
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return weatherForecasts;
        }

        // GET a specific weather forecast by date
        [HttpGet("{date}")]
        public ActionResult<WeatherForecast> GetWeatherForecast(DateOnly date)
        {
            var forecast = weatherForecasts.FirstOrDefault(f => f.Date == date);
            if (forecast == null)
            {
                return NotFound($"Weather forecast for {date} not found.");
            }
            return Ok(forecast);
        }

        // POST - Add a new weather forecast
        [HttpPost]
        public ActionResult<WeatherForecast> AddWeatherForecast([FromBody] WeatherForecast newForecast)
        {
            if (newForecast == null)
            {
                return BadRequest("Invalid weather forecast data.");
            }

            // Check if the forecast for the same date already exists
            if (weatherForecasts.Any(f => f.Date == newForecast.Date))
            {
                return Conflict($"Weather forecast for {newForecast.Date} already exists.");
            }

            weatherForecasts.Add(newForecast);
            return CreatedAtAction(nameof(GetWeatherForecast), new { date = newForecast.Date }, newForecast);
        }

        // PUT - Update an existing weather forecast
        [HttpPut("{date}")]
        public ActionResult<WeatherForecast> UpdateWeatherForecast(DateOnly date, [FromBody] WeatherForecast updatedForecast)
        {
            if (updatedForecast == null)
            {
                return BadRequest("Invalid weather forecast data.");
            }

            var existingForecast = weatherForecasts.FirstOrDefault(f => f.Date == date);
            if (existingForecast == null)
            {
                return NotFound($"Weather forecast for {date} not found.");
            }

            // Update the forecast
            existingForecast.TemperatureC = updatedForecast.TemperatureC;
            existingForecast.Summary = updatedForecast.Summary;

            return Ok(existingForecast);
        }

        // DELETE - Delete a weather forecast by date
        [HttpDelete("{date}")]
        public ActionResult DeleteWeatherForecast(DateOnly date)
        {
            var forecast = weatherForecasts.FirstOrDefault(f => f.Date == date);
            if (forecast == null)
            {
                return NotFound($"Weather forecast for {date} not found.");
            }

            weatherForecasts.Remove(forecast);
            return Ok($"Weather forecast for {date} has been deleted.");
        }

        // GET the average temperature for a range of dates
        [HttpGet("average-temperature")]
        public ActionResult<double> GetAverageTemperature([FromQuery] int startDay, [FromQuery] int endDay)
        {
            if (startDay < 1 || endDay < 1 || startDay > endDay)
            {
                return BadRequest("Invalid day range.");
            }

            var forecast = Enumerable.Range(startDay, endDay - startDay + 1)
                .Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();

            var averageTemp = forecast.Average(f => f.TemperatureC);
            return Ok(averageTemp);
        }
    }

    // WeatherForecast Model
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
