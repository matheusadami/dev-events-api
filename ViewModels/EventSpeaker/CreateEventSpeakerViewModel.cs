using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.EventSpeaker;

public class CreateEventSpeakerViewModel
{
  [Required(ErrorMessage = "Informe o nome do palestrante")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "Informe o título / assunto da palestra")]
  public string TalkTitle { get; set; } = string.Empty;

  [Required(ErrorMessage = "Informe a descrição da palestra")]
  public string TalkDescription { get; set; } = string.Empty;

  public string? LinkedInProfile { get; set; }
}