using FarmAdvisor.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FarmAdvisor.Models;
using FarmAdvisor.DataAccess.MSSQL;

namespace FarmAdvisor.Controllers
{
    [ApiController]
    [Authorize]
    [Route("sensors")]
    public class SensorsController : ControllerBase
    {

        private readonly JwtAuthenticationController jwtAuthenticationController;
        private readonly SensorDataAccess sensorDataAccess;
        private readonly FieldDataAccess fieldDataAccess;
        public SensorsController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
            this.sensorDataAccess = new SensorDataAccess();
            this.fieldDataAccess = new FieldDataAccess();
        }

        [HttpPost]
        public IActionResult postSensor([FromBody] SensorInput sensorInput)
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
    }
}