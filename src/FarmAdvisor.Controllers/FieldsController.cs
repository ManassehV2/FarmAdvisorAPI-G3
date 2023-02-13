using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FarmAdvisor.Models;
using FarmAdvisor.DataAccess.MSSQL;
namespace FarmAdvisor.Controllers
{
    [ApiController]
    [Authorize]
    [Route("fields")]
    public class FieldsController : ControllerBase
    {
        private readonly JwtAuthenticationController jwtAuthenticationController;
        private readonly FieldDataAccess fieldDataAccess;
        private readonly FarmDataAccess farmDataAccess;
        private readonly SensorDataAccess sensorDataAccess;
        public FieldsController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
            this.fieldDataAccess = new FieldDataAccess();
            this.farmDataAccess = new FarmDataAccess();
            this.sensorDataAccess = new SensorDataAccess();
        }
        [HttpPost]
        public IActionResult postField([FromBody] Field field)
        {
            Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
            Farm? farm = farmDataAccess.getByUserAndFarmId((Guid)userId!, field.FarmId);
            if (farm == null)
                return NotFound("Farm_Not_Found");
            field.UserId = userId;
            field = fieldDataAccess.add(field);
            return Ok(field);
        }
        [HttpGet]
        [Route("{fieldId?}")]
        public IActionResult getField(Guid fieldId)
        {
            Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
            Field? field = fieldDataAccess.getByUserAndFieldId((Guid)userId!, fieldId);
            if (field == null)
            {
                return NotFound("Field_Not_Found");
            }
            return Ok(field);

        }
        [HttpPatch]
        [Route("{fieldId?}")]
        public IActionResult patchField(Guid fieldId, FieldUpdate fieldUpdates)
        {
            Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
            Field? field = fieldDataAccess.updateByUserAndFieldId((Guid)userId!, fieldId, fieldUpdates);
            if (field == null)
            {
                return NotFound("Field_Not_Found");
            }
            return Ok(field);
        }
        [HttpDelete]
        [Route("{fieldId?}")]
        public IActionResult deleteField(Guid fieldId)
        {
            Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
            Field? field = fieldDataAccess.deleteByUserAndFieldId((Guid)userId!, fieldId);
            if (field == null)
            {
                return NotFound("Field_Not_Found");
            }
            return Ok(field);
        }
        [HttpGet]
        [Route("{fieldId?}/sensors")]
        public IActionResult getSensors(Guid fieldId)
        {
            Sensor[]? sensors = sensorDataAccess.getByFieldId(fieldId);
            return Ok(sensors);
        }
    }
}