using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FarmAdvisor.Models;
using FarmAdvisor.DataAccess.MSSQL;

namespace FarmAdvisor.Controllers
{
    [ApiController]
    [Authorize]
    [Route("farms")]
    public class FarmsController : ControllerBase
    {

        private readonly JwtAuthenticationController jwtAuthenticationController;
        private readonly FarmDataAccess farmDataAccess;
        private readonly FieldDataAccess fieldDataAccess;
        private readonly SensorDataAccess sensorDataAccess;
        public FarmsController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
            this.farmDataAccess = new FarmDataAccess();
            this.fieldDataAccess = new FieldDataAccess();
            this.sensorDataAccess = new SensorDataAccess();
        }

        [HttpPost]
        public IActionResult postFarm([FromBody] Farm farm)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                farm.UserId = userId;
                farm = farmDataAccess.add(farm);
                return Ok(farm);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult getFarms()
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                else
                {
                    Farm[] farms = farmDataAccess.getByUserId((Guid)userId);
                    return Ok(farms);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{farmId?}")]
        public IActionResult getFarm(Guid farmId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Farm? farm = farmDataAccess.getByUserAndFarmId((Guid)userId, farmId);
                if (farm == null)
                {
                    return NotFound("Farm_Not_Found");
                }
                return Ok(farm);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("{farmId?}")]
        public IActionResult patchFarm(Guid farmId, FarmUpdate farmUpdates)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Farm? farm = farmDataAccess.updateByUserAndFarmId((Guid)userId, farmId, farmUpdates);
                if (farm == null)
                {
                    return NotFound("Farm_Not_Found");
                }
                return Ok(farm);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{farmId?}")]
        public IActionResult deleteFarm(Guid farmId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Farm? farm = farmDataAccess.deleteByUserAndFarmId((Guid)userId, farmId);
                if (farm == null)
                {
                    return NotFound("Farm_Not_Found");
                }
                return Ok(farm);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{farmId?}/fields")]
        public IActionResult getFields(Guid farmId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Field[]? fields = fieldDataAccess.getByFarmId(farmId);
                return Ok(fields);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{farmId?}/notifications")]
        public IActionResult getNotifications(Guid farmId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Field[]? fields = fieldDataAccess.getByFarmId(farmId);
                List<FarmNotification> notifications = new List<FarmNotification>();
                foreach (Field field in fields)
                {
                    Sensor[]? sensors = sensorDataAccess.getByFieldId((Guid)field.FieldId!);
                    foreach (Sensor sensor in sensors)
                    {
                        if (!sensor.BatteryStatus)
                        {
                            notifications.Add(new FarmNotification()
                            {
                                FieldId = (Guid)field.FieldId,
                                FieldName = field.Name,
                                SensorId = (Guid)sensor.SensorId!,
                                SensorSerialNo = sensor.SerialNo,
                                Type = FarmNotification.NotificationType.LowBattery
                            });
                        }
                        if (sensor.GDD > sensor.DefaultGDD)
                        {
                            notifications.Add(new FarmNotification()
                            {
                                FieldId = (Guid)field.FieldId,
                                FieldName = field.Name,
                                SensorId = (Guid)sensor.SensorId!,
                                SensorSerialNo = sensor.SerialNo,
                                Type = FarmNotification.NotificationType.GddExceeded
                            });
                        }
                    }
                }
                return Ok(notifications);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
    }
}