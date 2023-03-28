using DevEventsApi.Entities;

namespace DevEventsApi.Services.Interfaces;

public interface ITokenService
{
  public abstract string GenerateToken(User user);
}