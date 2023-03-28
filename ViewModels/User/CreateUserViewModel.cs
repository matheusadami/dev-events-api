using System.ComponentModel.DataAnnotations;
using DevEventsApi.Enums;

namespace DevEventsApi.ViewModels.User;

public class CreateUserViewModel
{
  /// <summary>
  /// Nome do usuário
  /// </summary>
  [Required(ErrorMessage = "Informe o nome completo do usuário")]
  [StringLength(255, ErrorMessage = "O nome do usuário não pode conter mais de 255 caracteres")]
  public string Name { get; set; }

  [Required(ErrorMessage = "Informe a permissão do usuário")]
  public EUserRoles Role { get; set; }

  /// <summary>
  /// Username do usuário
  /// </summary>
  [Required(ErrorMessage = "Informe o username do usuário")]
  [StringLength(25, ErrorMessage = "O nome do usuário não pode conter mais de 25 caracteres")]
  public string Username { get; set; }

  /// <summary>
  /// Senha do usuário
  /// </summary>
  [Required(ErrorMessage = "Informe a senha do usuário")]
  [StringLength(25, MinimumLength = 3, ErrorMessage = "A senha do usuário deve conter entre 3 e 25 caracteres")]
  [DataType(DataType.Password)]
  public string Password { get; set; }
}