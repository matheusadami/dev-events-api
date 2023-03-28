using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.Event;

public class CreateEventViewModel
{
  /// <summary>
  /// Título do evento
  /// </summary>
  [Required(ErrorMessage = "Informe o título do evento")]
  [StringLength(255, MinimumLength = 3, ErrorMessage = "O título do evento deve conter entre 3 e 255 caracteres")]
  public string Title { get; set; }

  /// <summary>
  /// Descrição do evento
  /// </summary>
  [Required(ErrorMessage = "Informe a descrição do evento")]
  [StringLength(255, MinimumLength = 3, ErrorMessage = "A descrição do evento deve conter entre 3 e 255 caracteres")]
  public string Description { get; set; }

  /// <summary>
  /// Data incial do evento
  /// </summary>
  [Required(ErrorMessage = "Informe a data inicial do evento")]
  [DataType(DataType.DateTime)]
  public DateTime InitialDate { get; set; }

  /// <summary>
  /// Data final do evento
  /// </summary>
  [Required(ErrorMessage = "Informe a data final do evento")]
  [DataType(DataType.DateTime)]
  public DateTime FinalDate { get; set; }

  /// <summary>
  /// Código do usuário responsável pelo evento
  /// </summary>
  [Required(ErrorMessage = "Informe o código de identificação (Uid) do usuário responsável pelo evento")]
  public Guid UserId { get; set; }
}