using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.Event;

public class UpdateEventViewModel
{
  [MaxLength(255, ErrorMessage = "O título do evento pode conter no máximo 255 caracteres")]
  public string? Title { get; set; }

  [MaxLength(255, ErrorMessage = "A descrição do evento pode conter no máximo 255 caracteres")]
  public string? Description { get; set; }
}