namespace DevEventsApi.Config;

public sealed class AuthSettings
{

  public const string Key = "AuthSettings";

  public const string UserIdentifierClaimTypeName = "UserIdentifier";


  public string JWTSecretKey { get; set; } = string.Empty;
}
