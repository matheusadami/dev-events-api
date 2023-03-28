using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DevEventsApi.Entities;
using DevEventsApi.Services.Interfaces;

namespace DevEventsApi.Services;

public class TokenService : ITokenService
{
  public IConfiguration configuration { get; set; }
  public TokenService(IConfiguration configuration) => this.configuration = configuration;

  public string GenerateToken(User user)
  {
    var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT:SecretKey"));

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
        {
          new Claim(ClaimTypes.Name, user.Username),
          new Claim(ClaimTypes.Role, user.Role.ToString())
        }),
      Expires = DateTime.UtcNow.AddHours(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}