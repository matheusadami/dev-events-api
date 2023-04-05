namespace DevEventsApi.Config;

public sealed class HashingSettings
{
  public const string Key = "HashingSettings";

  public int Iterations { get; set; } = 100;
}