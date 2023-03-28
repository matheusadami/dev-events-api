namespace DevEventsApi.Entities;

public class Event
{
  public Guid Uid { get; set; } = Guid.NewGuid();
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;

  public DateTime InitialDate { get; set; }
  public DateTime FinalDate { get; set; }
  public DateTime CreateAt { get; set; } = DateTime.UtcNow;
  public DateTime? DeleteAt { get; set; }

  public Guid UserUid { get; set; }

  public User? User { get; set; }

  public List<EventSpeaker> Speakers { get; set; } = new();
}
