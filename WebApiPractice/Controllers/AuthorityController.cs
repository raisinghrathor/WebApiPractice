using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebApiPractice.Authority;

namespace WebApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthorityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost]
        
        public IActionResult Authenticate([FromBody] AppCredential appCredential)
        {
            if (AppRepository.Authenticate(appCredential.ClientId, appCredential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                   
                    access_token = CreateToken(appCredential.ClientId, expiresAt),
                    expires_at = expiresAt


                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
               
        }

        private string CreateToken(string clientId, DateTime expiresAt)
        {
            //Algo
            //Payload
            //Signing Key
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));
            var app = AppRepository.GetApplicationByClientId(clientId);
            var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
                new Claim("Read",(app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write",(app?.Scopes??string.Empty).Contains("write")?"true":"false"),
                new Claim("Delete",(app?.Scopes??string.Empty).Contains("delete")?"true":"false")
            };

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                     new SymmetricSecurityKey(secretKey),
                     SecurityAlgorithms.HmacSha256Signature
                    ),
                claims:claims,
                expires: expiresAt,
                notBefore:DateTime.UtcNow
                );

                    return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

   
}
