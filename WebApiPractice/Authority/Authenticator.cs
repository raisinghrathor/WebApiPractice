using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace WebApiPractice.Authority
{
    public class Authenticator
    {

        public static bool Authenticate(string? clientid, string? secret)
        {
            var app = AppRepository.GetApplicationByClientId(clientid);
            if (app == null)
            {
                return false;
            }
            return (app.ClientId == clientid && app.Secret == secret);
        }
        public static string    CreateToken(string clientId, DateTime expiresAt, string? strSecretKey)
        {
            //Algo
            //Payload
            //Signing Key
            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);
            var app = AppRepository.GetApplicationByClientId(clientId);
            var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
               
            };
            var scope = app?.Scopes?.Split(',');
            if(scope != null && scope.Length > 0)
            {
                foreach (var item in scope)
                {
                    claims.Add(new Claim(item.ToLower(),"true"));
                }
            }
           

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                     new SymmetricSecurityKey(secretKey),
                     SecurityAlgorithms.HmacSha256Signature
                    ),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow
                );
                
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static IEnumerable<Claim>? VerifyToken(string token, string? strSecretKey)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring(6).Trim();
            }

            SecurityToken validatedToken;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(strSecretKey)),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,                   
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);
                if (validatedToken != null) 
                {
                    var tokenOject = tokenHandler.ReadJwtToken(token);//.ReadToken(tokenString) as JwtSecurityToken;
                    return tokenOject.Claims ?? (new List<Claim>());
                }
                else
                {
                    return null;
                }
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch
            {
                throw;
            }
          //  return validatedToken != null;
        }
    }
}
