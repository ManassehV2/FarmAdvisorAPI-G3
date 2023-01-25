using FarmAdvisor.Commons;
using Microsoft.AspNetCore.Mvc;
using FarmAdvisor.Services.WeatherApi;
using FarmAdvisor.Models;

namespace FarmAdvisor.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("weather-api")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastService weatherForecastService = new WeatherForecastService();

        [HttpGet]
        [Route("forecast")]
        public async Task<IActionResult> getForecastAsync([FromQuery] int altitude, double latitude, double longitude, double baseTemperature)
        {
            try
            {
                if (!Utils.isValidLatitude(latitude))
                    return Ok("Invalid_Latitude");
                if (!Utils.isValidLongitude(longitude))
                    return Ok("Invalid_Longitude");
                if (altitude < -500)
                    altitude = -500;
                if (altitude > 9000)
                    altitude = 9000;
                List<OneDayWeatherForecast> forecasts = await weatherForecastService.getForecastAsync(altitude, latitude, longitude, baseTemperature);
                return Ok(forecasts);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

    }
}