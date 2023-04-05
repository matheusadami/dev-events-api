using System.Security.Cryptography;
using DevEventsApi.Config;
using DevEventsApi.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DevEventsApi.Services;

public class HasherService : IHasherService
{
  private const int SaltSize = 16; // 128 bits
  private const int KeySize = 32; // 256 bits

  private HashingSettings hashingSettings { get; set; }

  public HasherService(IOptionsMonitor<HashingSettings> options) => this.hashingSettings = options.CurrentValue;

  public string Hash(string value)
  {
    using (var algorithm = new Rfc2898DeriveBytes(value, SaltSize, hashingSettings.Iterations, HashAlgorithmName.SHA256))
    {
      var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
      var salt = Convert.ToBase64String(algorithm.Salt);
      return $"{key}.{salt}";
    }
  }

  public bool Verify(string hash, string value)
  {
    var hashParts = hash.Split('.', 2);
    if (hashParts.Length < 2)
      throw new FormatException($"Formato de hash inválido ({hash}).");

    var key = Convert.FromBase64String(hashParts.ElementAt(0));
    var salt = Convert.FromBase64String(hashParts.ElementAt(1));

    using (var algorithm = new Rfc2898DeriveBytes(value, salt, hashingSettings.Iterations, HashAlgorithmName.SHA256))
    {
      var checkingKey = algorithm.GetBytes(KeySize);
      return checkingKey.SequenceEqual(key);
    }
  }
}