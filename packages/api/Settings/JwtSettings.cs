namespace Api.Settings;

public class JwtSettings
{
  public string Audience { get; set; } = null!;
  public int ExpiresInMinutes { get; set; }
  public string Issuer { get; set; } = null!;
  public string Key { get; set; } = null!;

  public void Validate()
  {
    if (string.IsNullOrWhiteSpace(Key))
      throw new InvalidOperationException("JWT Key is missing in configuration!");
    if (string.IsNullOrWhiteSpace(Issuer))
      throw new InvalidOperationException("JWT Issuer is missing in configuration!");
    if (string.IsNullOrWhiteSpace(Audience))
      throw new InvalidOperationException("JWT Audience is missing in configuration!");
    if (ExpiresInMinutes <= 0)
      throw new InvalidOperationException("JWT ExpiresInMinutes must be greater than 0!");
  }
}
