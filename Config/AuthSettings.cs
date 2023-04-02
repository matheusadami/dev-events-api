namespace DevEventsApi.Config;

public class AuthSettings
{
  public const string Key = "AuthSettings";
  public string JWTSecretKey { get; set; } = string.Empty;
}
