using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using FarmAdvisor.DataAccess.MSSQL;
using FarmAdvisor.Models;

namespace FarmAdvisor.Controllers
{
    public class JwtAuthenticationController
    {
        private readonly IConfiguration _configuration;
        private readonly UserDataAccess userDataAccess;

        public JwtAuthenticationController(IConfiguration configuration)
        {
            this.userDataAccess = new UserDataAccess();
            _configuration = configuration;
        }

        public UserLogin? Authenticate(string phone, string passwordHash)
        {
            User? user = userDataAccess.getByCredentials(phone, passwordHash);
            if(user == null){
                return null;
            }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Token"]!);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Actor, user.UserId.ToString()!),
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserLogin userLogin = new UserLogin(){
                userId = user.UserId,
                name = user.Name,
                phone = user.Phone,
                email = user.Email,
                accessToken = tokenHandler.WriteToken(token)
            };
            return userLogin;
        }
        
        public Guid? getCurrentUserId(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new Guid(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value!);
            }
            return null;
        }
    }
}