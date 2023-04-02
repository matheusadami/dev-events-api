using DevEventsApi.Entities.Interfaces;

namespace DevEventsApi.Entities;

public class Event : IAuditable
{
  public Guid Uid { get; set; } = Guid.NewGuid();

  public Guid UserUid { get; set; }

  public string Title { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public DateTime InitialDate { get; set; }

  public DateTime FinalDate { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime? UpdatedAt { get; set; }

  public DateTime? DeletedAt { get; set; }

  public User? User { get; set; }

  public List<EventSpeaker> Speakers { get; set; } = new();
}
