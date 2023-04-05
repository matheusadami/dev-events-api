namespace DevEventsApi.Services.Interfaces;

public interface IHasherService
{
  string Hash(string value);
  bool Verify(string hash, string value);
}