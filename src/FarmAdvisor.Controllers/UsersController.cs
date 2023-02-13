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
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly JwtAuthenticationController jwtAuthenticationController;
        private readonly UserDataAccess userDataAccess;
        public UsersController(JwtAuthenticationController jwtAuthenticationController)
        {
            this.jwtAuthenticationController = jwtAuthenticationController;
            this.userDataAccess = new UserDataAccess();
        }
        [HttpPost]
        [Route("signup")]
        public IActionResult signup([FromBody] UserSignup userSignup)
        {
            try
            {
                if (!Utils.isValidPhone(userSignup.phone))
                {
                    return BadRequest("Invalid_Phone");
                }
                User? oldUser = userDataAccess.getByPhone(userSignup.phone);
                if (oldUser != null)
                {
                    return BadRequest("Phone_Already_Exists");
                }
                string verificationCode = "123456";
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("f864545909d0d9df7b3cd9afffb1e8bcf43af637aa46753f26b04cc2d2f44133");
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.MobilePhone, userSignup.phone),
                    new Claim(ClaimTypes.Sid, verificationCode)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var userSignUpVerificationToken = tokenHandler.WriteToken(token);
                UserSignupVerification userSignupVerification = new UserSignupVerification(userSignUpVerificationToken);
                return Ok(JsonSerializer.Serialize(userSignupVerification));
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
        [HttpPost]
        [Route("signup/verify")]
        public IActionResult signupVerify([FromBody] UserSignupVerificationInput userSignupVerificationInput)
        {
            try
            {
                JwtSecurityToken userSignVerification;
                string phone;
                string verificationCode;
                try
                {
                    userSignVerification = new JwtSecurityToken(jwtEncodedString: userSignupVerificationInput.userSignVerificationToken);
                    phone = userSignVerification.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone").Value;
                    verificationCode = userSignVerification.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid").Value;
                }
                catch (Exception)
                {
                    return BadRequest("Invalid_Token");
                }
                if (userSignupVerificationInput.userSignVerificationCode != verificationCode)
                {
                    return Unauthorized("Invalid_Verification_Code");
                }
                User? oldUser = userDataAccess.getByPhone(phone);
                if (oldUser != null)
                {
                    return BadRequest("Phone_Already_Exists");
                }
                if (!Utils.isValidEmail(userSignupVerificationInput.email))
                {
                    return BadRequest("Invalid_Email");
                }
                User user = new User()
                {
                    Name = userSignupVerificationInput.name,
                    Phone = phone,
                    Email = userSignupVerificationInput.email,
                    PasswordHash = userSignupVerificationInput.passwordHash
                };
                userDataAccess.addUser(user);
                UserLogin? userLogin = jwtAuthenticationController.Authenticate(user.Phone, user.PasswordHash);
                return Ok(userLogin);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
        [HttpPost]
        [Route("login")]
        public IActionResult login([FromBody] UserLoginInput userLoginInput)
        {
            try
            {
                if (!Utils.isValidPhone(userLoginInput.phone))
                {
                    return BadRequest("Invalid_Phone");
                }
                UserLogin? userLogin = jwtAuthenticationController.Authenticate(userLoginInput.phone, userLoginInput.passwordHash);
                if (userLogin == null)
                    return Unauthorized();
                return Ok(userLogin);
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("me")]
        public IActionResult getMe()
        {
            try
            {
                Guid? userId = jwtAuthenticationController.getCurrentUserId(HttpContext);
                if (userId != null)
                {
                    User? user = userDataAccess.getById((Guid)userId);
                    if (user != null)
                    {
                        user.PasswordHash = null;
                        return Ok(user);
                    }
                }
                return NotFound();
            }
            catch (Exception e)
            {
                Console.Write(e);
                return StatusCode(500);
            }
        }
    }
}