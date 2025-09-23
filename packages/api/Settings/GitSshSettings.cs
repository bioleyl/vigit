namespace Api.Settings;

public class GitSshSettings
{
  public string TargetScriptDir { get; set; } = "/usr/local/bin";
  public string GitUser { get; set; } = "git";
  public string AppUrl { get; set; } = "http://localhost:5001";

  public void Validate()
  {
    if (string.IsNullOrWhiteSpace(TargetScriptDir))
      throw new InvalidOperationException("GitSsh TargetScriptDir is missing in configuration!");
    if (string.IsNullOrWhiteSpace(GitUser))
      throw new InvalidOperationException("GitSsh GitUser is missing in configuration!");
    if (string.IsNullOrWhiteSpace(AppUrl))
      throw new InvalidOperationException("GitSsh AppUrl is missing in configuration!");
  }
}
