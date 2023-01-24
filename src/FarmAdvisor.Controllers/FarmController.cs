using FarmAdvisor.Commons;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FarmAdvisor.Models;
using FarmAdvisor.DataAccess.MSSQL;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FarmAdvisor.Controllers
{
    [ApiController]
    [Authorize]
    [Route("farms")]
    public class FarmController : ControllerBase
    {

        private readonly JwtAuthenticationController jwtAuthenticationController;
        private readonly FarmDataAccess farmDataAccess;
        public FarmController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
            this.farmDataAccess = new FarmDataAccess();
        }

        [HttpPost]
        [Route("")]
        public IActionResult addFarm([FromBody] Farm farm)
        {
            try
            {
                string? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if(String.IsNullOrEmpty(userId)){
                    return NotFound("User_Not_Found");
                }
                farm.UserId = new Guid(userId);
                farm = farmDataAccess.add(farm);
                return Ok(farm);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
    }
}