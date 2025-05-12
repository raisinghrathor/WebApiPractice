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
   
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthorityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost("auth")]
        
        public IActionResult Authenticate([FromBody] AppCredential appCredential)
        {
            if (Authenticator.Authenticate(appCredential.ClientId, appCredential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new 
                {
                   
                    access_token = Authenticator.CreateToken(appCredential.ClientId, expiresAt,configuration.GetValue<string>("SecretKey")),
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

       
    }

   
}
