using FarmAdvisor.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FarmAdvisor.Models;
using FarmAdvisor.DataAccess.MSSQL;
using FarmAdvisor.Services.WeatherApi;

namespace FarmAdvisor.Controllers
{
    [ApiController]
    [Authorize]
    [Route("sensors")]
    public class SensorsController : ControllerBase
    {

        private readonly JwtAuthenticationController jwtAuthenticationController;
        private readonly SensorDataAccess sensorDataAccess = new SensorDataAccess();
        private readonly SensorStatisticsDataAccess sensorStatisticsDataAccess = new SensorStatisticsDataAccess();
        private readonly FieldDataAccess fieldDataAccess = new FieldDataAccess();
        private readonly WeatherForecastService weatherForecastService = new WeatherForecastService();
        public SensorsController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
        }

        [HttpPost]
        public async Task<IActionResult> postSensorAsync([FromBody] SensorInput sensorInput)
        {
            try
            {
                if (!Utils.isValidLatitude(sensorInput.Lat))
                    return Ok("Invalid_Latitude");
                if (!Utils.isValidLongitude(sensorInput.Long))
                    return Ok("Invalid_Longitude");
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Field? field = fieldDataAccess.getByUserAndFieldId((Guid)userId, sensorInput.FieldId);
                if (field == null)
                    return NotFound("Field_Not_Found");
                Sensor sensor = new Sensor()
                {
                    FieldId = sensorInput.FieldId,
                    SerialNo = sensorInput.SerialNo,
                    GDD = sensorInput.GDD,
                    DefaultGDD = sensorInput.DefaultGDD,
                    Long = sensorInput.Long,
                    Lat = sensorInput.Lat,
                    InstallationDate = sensorInput.InstallationDate,
                    LastCuttingDate = sensorInput.LastCuttingDate,
                    BaseTemperature = sensorInput.BaseTemperature,
                    UserId = userId
                };
                sensorDataAccess.add(sensor);
                DateTime date = DateTime.UtcNow.Date;
                Guid SensorId = (Guid)sensor.SensorId!;
                List<SensorStatistic> sensorStatistics = new List<SensorStatistic>();
                List<OneDayWeatherForecast> forecasts = await weatherForecastService.getForecastAsync(field.Altitude, sensor.Lat, sensor.Long, sensor.BaseTemperature, sensor.GDD);
                List<double> temps = new List<double>();
                List<double> gdds = new List<double>();
                double lastGdd = forecasts[0].GDD;
                bool cuttingDateSet = false;
                double defaultGDD = sensor.DefaultGDD;
                foreach (OneDayWeatherForecast forecast in forecasts)
                {
                    sensorStatistics.Add(new SensorStatistic()
                    {
                        Date = date,
                        SensorId = SensorId,
                        averageTemperature = forecast.averageTemperature,
                        GDD = forecast.GDD
                    });
                    if (!cuttingDateSet && forecast.GDD > defaultGDD)
                    {
                        double downDt = defaultGDD - lastGdd;
                        double upDt = forecast.GDD - defaultGDD;
                        sensor.cuttingDate = downDt > upDt ? date : date.AddDays(-1);
                        cuttingDateSet = true;
                    }
                    lastGdd = forecast.GDD;
                    temps.Add(forecast.averageTemperature);
                    gdds.Add(forecast.GDD);
                    date = date.AddDays(1);
                }
                double[] tempIncPattern = Utils.getIncPattern(temps);
                double[] gddIncPattern = Utils.getIncPattern(gdds);
                int incPatternLength = tempIncPattern.Length;
                double lastTemp = temps[^1];
                int loopCount = 100 - sensorStatistics.Count;
                for (int dateCount = 0; dateCount < loopCount; dateCount++)
                {
                    lastTemp += tempIncPattern[dateCount % incPatternLength];
                    lastGdd += gddIncPattern[dateCount % incPatternLength];
                    sensorStatistics.Add(new SensorStatistic()
                    {
                        Date = date,
                        SensorId = SensorId,
                        averageTemperature = lastTemp,
                        GDD = lastGdd
                    });

                    if (!cuttingDateSet && lastGdd > defaultGDD)
                    {
                        double downDt = defaultGDD - lastGdd;
                        double upDt = lastGdd - defaultGDD;
                        sensor.cuttingDate = downDt > upDt ? date : date.AddDays(-1);
                        cuttingDateSet = true;
                    }
                    date = date.AddDays(1);
                }
                sensorStatisticsDataAccess.addMany(sensorStatistics);
                return Ok(sensor);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{sensorId?}")]
        public IActionResult getSensor(Guid sensorId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Sensor? sensor = sensorDataAccess.getByUserAndSensorId((Guid)userId, sensorId);
                if (sensor == null)
                {
                    return NotFound("Sensor_Not_Found");
                }
                return Ok(sensor);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("{sensorId?}")]
        public IActionResult patchSensor(Guid sensorId, SensorUpdate sensorUpdates)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Sensor? sensor = sensorDataAccess.updateByUserAndSensorId((Guid)userId, sensorId, sensorUpdates);
                if (sensor == null)
                {
                    return NotFound("Sensor_Not_Found");
                }
                return Ok(sensor);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{sensorId?}")]
        public IActionResult deleteSensor(Guid sensorId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Sensor? sensor = sensorDataAccess.deleteByUserAndSensorId((Guid)userId, sensorId);
                if (sensor == null)
                {
                    return NotFound("Sensor_Not_Found");
                }
                return Ok(sensor);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{sensorId?}/statistics")]
        public IActionResult getStatistics(Guid sensorId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Sensor? sensor = sensorDataAccess.getByUserAndSensorId((Guid)userId, sensorId);
                if (sensor == null)
                {
                    return NotFound("Sensor_Not_Found");
                }
                SensorStatistic[] sensorStatistic = sensorStatisticsDataAccess.getBySensorId(sensorId)!;
                return Ok(sensorStatistic);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("{sensorId?}/trigger")]
        public IActionResult triggerSensor(Guid sensorId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Sensor? sensor = sensorDataAccess.getByUserAndSensorId((Guid)userId, sensorId);
                if (sensor == null)
                {
                    return NotFound("Sensor_Not_Found");
                }
                try
                {
                    SensorStatistic sensorStatistic = sensorStatisticsDataAccess.getBySensorIdAndDate(sensorId, DateTime.UtcNow.Date)!;
                    sensor = sensorDataAccess.updateByUserAndSensorId((Guid)userId, sensorId, new SensorUpdate() { GDD = sensorStatistic.GDD });
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
                return Ok(sensor);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
    }
}