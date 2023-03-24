using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.Event;

public class CreateEventViewModel
{
  [Required(ErrorMessage = "Informe o título do evento")]
  [StringLength(255, MinimumLength = 3, ErrorMessage = "O título do evento deve conter entre 3 e 255 caracteres")]
  public string Title { get; set; }

  [Required(ErrorMessage = "Informe a descrição do evento")]
  [StringLength(255, MinimumLength = 3, ErrorMessage = "A descrição do evento deve conter entre 3 e 255 caracteres")]
  public string Description { get; set; }

  [Required(ErrorMessage = "Informe a data inicial do evento")]
  [DataType(DataType.DateTime)]
  public DateTime InitialDate { get; set; }

  [Required(ErrorMessage = "Informe a data final do evento")]
  [DataType(DataType.DateTime)]
  public DateTime FinalDate { get; set; }
}