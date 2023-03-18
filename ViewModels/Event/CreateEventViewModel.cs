using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.Event;

public class CreateEventViewModel
{
  [Required(ErrorMessage = "Informe o título do evento")]
  [MaxLength(255, ErrorMessage = "O título do evento pode conter no máximo 255 caracteres")]
  public string Title { get; set; } = string.Empty;

  [Required(ErrorMessage = "Informe a descrição do evento")]
  [MaxLength(255, ErrorMessage = "A descrição do evento pode conter no máximo 255 caracteres")]
  public string Description { get; set; } = string.Empty;

  [Required(ErrorMessage = "Informe a data inicial do evento")]
  [DataType(DataType.Date)]
  public DateTime InitialDate { get; set; }

  [Required(ErrorMessage = "Informe a data final do evento")]
  [DataType(DataType.Date)]
  public DateTime FinalDate { get; set; }
}