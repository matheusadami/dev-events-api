using System.ComponentModel.DataAnnotations;

namespace DevEventsApi.Attributes;

public class BlockEmailDomainAttribute : ValidationAttribute
{
  public IEnumerable<string> Domains { get; set; }

  public BlockEmailDomainAttribute(string domain)
  {
    Domains = domain.Split(";").ToList();
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext context)
  {
    var hasBlockingDomain = Domains.Any(((string)(value ?? "")).Contains);
    return hasBlockingDomain ? new ValidationResult("Domínio do e-mail inválido") : ValidationResult.Success;
  }
}
