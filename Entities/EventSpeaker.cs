namespace DevEventsApi.Entities;

public class EventSpeaker
{
  public Guid Uid { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public string TalkTitle { get; set; } = string.Empty;
  public string TalkDescription { get; set; } = string.Empty;
  public string? LinkedInProfile { get; set; }
  public Guid EventUid { get; set; }
  public Event? Event { get; set; }
}
