using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("weathercasters")]
    public class WeatherCasterController : ControllerBase
    {
        // Simulating an in-memory list of weather casters
        private static List<WeatherCaster> casters = new List<WeatherCaster>
        {
            new WeatherCaster { Name = "Kuya Kim Atienza", Description = "A beloved weathercaster and TV personality in the Philippines known for his lighthearted and informative weather reports." },
            new WeatherCaster { Name = "Jessica Soho", Description = "A respected journalist and news anchor who occasionally covers weather updates on GMA News." },
            new WeatherCaster { Name = "Atty. Mike Puno", Description = "A seasoned weathercaster known for his precision in delivering weather forecasts with clear explanations." },
            new WeatherCaster { Name = "Dianne Castillejo", Description = "A well-known sports journalist who also provides weather reports." },
        };

        #region "GET All Casters" - Retrieve all weather casters
        // GET all casters
        [HttpGet]
        public ActionResult<IEnumerable<WeatherCaster>> GetWeatherCasters()
        {
            return Ok(casters);
        }
        #endregion

        #region "GET Specific Caster" - Retrieve a specific weather caster by name
        // GET a specific weather caster by name
        [HttpGet("{name}")]
        public ActionResult<WeatherCaster> GetWeatherCasterDetails(string name)
        {
            var caster = casters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (caster == null)
            {
                return NotFound($"Weather caster '{name}' not found.");
            }

            return Ok(caster);
        }
        #endregion

        #region "Add Caster" - Add a new weather caster
        // Add - Add a new weather caster
        [HttpPost]
        public ActionResult<WeatherCaster> AddWeatherCaster([FromBody] WeatherCaster newCaster)
        {
            if (newCaster == null)
            {
                return BadRequest("Invalid weather caster data.");
            }

            // Check if the caster already exists
            if (casters.Any(c => c.Name.Equals(newCaster.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict($"Weather caster '{newCaster.Name}' already exists.");
            }

            // Add the new caster to the list
            casters.Add(newCaster);
            return CreatedAtAction(nameof(GetWeatherCasterDetails), new { name = newCaster.Name }, newCaster);
        }
        #endregion

        #region "Update Caster" - Update an existing weather caster
        // Update - Update an existing weather caster
        [HttpPut("{name}")]
        public ActionResult<WeatherCaster> UpdateWeatherCaster(string name, [FromBody] WeatherCaster updatedCaster)
        {
            if (updatedCaster == null)
            {
                return BadRequest("Invalid weather caster data.");
            }

            var existingCaster = casters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existingCaster == null)
            {
                return NotFound($"Weather caster '{name}' not found.");
            }

            // Update the details of the existing caster
            existingCaster.Description = updatedCaster.Description;

            return Ok(existingCaster);
        }
        #endregion

        #region "Delete Caster" - Remove a weather caster by name
        // Delete - Remove a weather caster by name
        [HttpDelete("{name}")]
        public ActionResult DeleteWeatherCaster(string name)
        {
            var caster = casters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (caster == null)
            {
                return NotFound($"Weather caster '{name}' not found.");
            }

            casters.Remove(caster);
            return Ok($"Weather caster '{name}' has been deleted.");
        }
        #endregion
    }

    #region "WeatherCaster Model"
    // WeatherCaster Model
    public class WeatherCaster
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    #endregion
}
