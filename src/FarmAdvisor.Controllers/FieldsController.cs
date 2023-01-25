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
        public FieldsController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
            this.fieldDataAccess = new FieldDataAccess();
            this.farmDataAccess = new FarmDataAccess();
        }

        [HttpPost]
        public IActionResult postField([FromBody] Field field)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Farm? farm = farmDataAccess.getByUserAndFarmId((Guid)userId, field.FarmId);
                if (farm == null)
                    return NotFound("Farm_Not_Found");
                field.UserId = userId;
                field = fieldDataAccess.add(field);
                return Ok(field);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{fieldId?}")]
        public IActionResult getField(Guid fieldId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Field? field = fieldDataAccess.getByUserAndFieldId((Guid)userId, fieldId);
                if (field == null)
                {
                    return NotFound("Field_Not_Found");
                }
                return Ok(field);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("{fieldId?}")]
        public IActionResult patchField(Guid fieldId, FieldUpdate fieldUpdates)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Field? field = fieldDataAccess.updateByUserAndFieldId((Guid)userId, fieldId, fieldUpdates);
                if (field == null)
                {
                    return NotFound("Field_Not_Found");
                }
                return Ok(field);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{fieldId?}")]
        public IActionResult deleteField(Guid fieldId)
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId == null)
                {
                    return NotFound("User_Not_Found");
                }
                Field? field = fieldDataAccess.deleteByUserAndFieldId((Guid)userId, fieldId);
                if (field == null)
                {
                    return NotFound("Field_Not_Found");
                }
                return Ok(field);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
    }
}