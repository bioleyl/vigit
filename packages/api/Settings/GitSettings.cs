namespace Api.Settings;

public class GitSettings
{
  public string TargetScriptDir { get; set; } = "/usr/local/bin";
  public string RepositoriesRoot { get; set; } = "/opt/vigit/repos";
  public string GitUser { get; set; } = "git";
  public string AppUrl { get; set; } = "http://localhost:5001";

  public void Validate()
  {
    if (string.IsNullOrWhiteSpace(RepositoriesRoot))
      throw new InvalidOperationException("Git RepositoriesRoot is missing in configuration!");
    if (string.IsNullOrWhiteSpace(TargetScriptDir))
      throw new InvalidOperationException("Git TargetScriptDir is missing in configuration!");
    if (string.IsNullOrWhiteSpace(GitUser))
      throw new InvalidOperationException("Git GitUser is missing in configuration!");
    if (string.IsNullOrWhiteSpace(AppUrl))
      throw new InvalidOperationException("Git AppUrl is missing in configuration!");
  }
}
