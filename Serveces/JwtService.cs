using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace dndsitgen.Serveces
{
    public class JwtService
    {
        private readonly IConfiguration config;
        public JwtService(IConfiguration config)
        {
            this.config = config;
        }

        public string Generate(string username) {

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
