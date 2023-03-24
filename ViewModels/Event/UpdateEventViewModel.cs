using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.Event;

public class UpdateEventViewModel
{
  [StringLength(255, MinimumLength = 3, ErrorMessage = "O título do evento deve conter entre 3 e 255 caracteres")]
  public string? Title { get; set; }

  [StringLength(255, MinimumLength = 3, ErrorMessage = "A descrição do evento deve conter entre 3 e 255 caracteres")]
  public string? Description { get; set; }
}