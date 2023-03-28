using System.ComponentModel.DataAnnotations;
using DevEventsApi.Attributes;

namespace DevEventsApi.ViewModels.EventSpeaker;

public class CreateEventSpeakerViewModel
{
  /// <summary>
  /// Nome do palestrante
  /// </summary>
  [Required(ErrorMessage = "Informe o nome do palestrante")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// E-mail do palestrante
  /// </summary>
  [Required(ErrorMessage = "Informe o e-mail do palestrante")]
  [BlockEmailDomainAttribute("@myyahoo.com;@yahoo.com;@google.com")]
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Título / assunto da palestra
  /// </summary>
  [Required(ErrorMessage = "Informe o título / assunto da palestra")]
  public string TalkTitle { get; set; } = string.Empty;

  /// <summary>
  /// Descrição da palestra
  /// </summary>
  [Required(ErrorMessage = "Informe a descrição da palestra")]
  public string TalkDescription { get; set; } = string.Empty;

  /// <summary>
  /// LinkedIn do palestrante
  /// </summary>
  public string? LinkedInProfile { get; set; }
}