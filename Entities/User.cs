using DevEventsApi.Enums;

namespace DevEventsApi.Entities;

public class User
{
  public Guid Uid { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;

  public EUserRoles Role { get; set; }

  public string Username { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;

  public List<Event> Events { get; set; } = new();
}