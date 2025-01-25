using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebMarket.DataAccess.Models;

namespace WebMarket.Authorization.JWT
{
    public class JWTProvider(IOptions<JWTOptions> options)
    {
        private readonly JWTOptions _options = options.Value;
        public const string UserIdJWTKey = "userId";
        public string GenerateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Actor, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
                claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
