using DevEventsApi.Entities.Interfaces;

namespace DevEventsApi.Entities;

public class EventSpeaker : IAuditable
{
  public Guid Uid { get; set; } = Guid.NewGuid();

  public Guid EventUid { get; set; }

  public string Name { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public string TalkTitle { get; set; } = string.Empty;

  public string TalkDescription { get; set; } = string.Empty;

  public string? LinkedInProfile { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime? UpdatedAt { get; set; }

  public Event? Event { get; set; }
}
