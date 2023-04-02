using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevEventsApi.Entities;

public class Auditing
{
  public Guid Uid { get; set; } = Guid.NewGuid();

  public string EntityName { get; set; }

  public string ActionType { get; set; }

  public string Username { get; set; }

  public DateTime TimeStamp { get; set; }

  public string EntityUid { get; set; }

  public Dictionary<string, object?> Changes { get; set; }
}