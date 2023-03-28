using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.ViewModels.User;

public class GenerateTokenViewModel
{
  [Required(ErrorMessage = "Informe o username do usuário")]
  public string Username { get; set; }

  [Required(ErrorMessage = "Informe a senha do usuário")]
  public string Password { get; set; }
}