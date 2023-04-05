using DevEventsApi.Entities;

namespace DevEventsApi.Services.Interfaces;

public interface ITokenService
{
  string GenerateToken(User user);
}