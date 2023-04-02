using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DevEventsApi.Entities;
using DevEventsApi.Services.Interfaces;
using DevEventsApi.Config;
using Microsoft.Extensions.Options;

namespace DevEventsApi.Services;

public class TokenService : ITokenService
{
  public AuthSettings authSettings { get; set; }

  public TokenService(IOptionsMonitor<AuthSettings> options) => this.authSettings = options.CurrentValue;

  public string GenerateToken(User user)
  {
    var key = Encoding.ASCII.GetBytes(authSettings.JWTSecretKey);

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
        {
          new Claim(ClaimTypes.Name, user.Name),
          new Claim(ClaimTypes.NameIdentifier, user.Username),
          new Claim(ClaimTypes.Role, user.Role.ToString())
        }),
      Expires = DateTime.UtcNow.AddHours(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}