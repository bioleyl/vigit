using System.Diagnostics;
using Api.Repositories.Interfaces;
using Api.Settings;
using Microsoft.Extensions.Options;

namespace Api.Repositories;

public class GitRepository : IGitRepository
{
  private readonly GitSettings _gitSettings;

  public GitRepository(IOptions<GitSettings> gitSettings)
  {
    _gitSettings = gitSettings.Value;
  }

  private string GetRepositoryPath(string repositoryName)
  {
    return Path.Combine(_gitSettings.RepositoriesRoot, $"{repositoryName}.git");
  }

  public void InitBareRepository(string repositoryName)
  {
    var repoPath = GetRepositoryPath(repositoryName);
    if (!Directory.Exists(repoPath))
    {
      Directory.CreateDirectory(repoPath);
      Run(repoPath, "init --bare");
      Process
        .Start("chown", $"-R {_gitSettings.GitUser}:{_gitSettings.GitUser} {repoPath}")
        .WaitForExit();
    }
  }

  private void Run(string repoPath, string command)
  {
    var startInfo = new ProcessStartInfo("git", command)
    {
      WorkingDirectory = repoPath,
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false,
    };

    using var process = new Process { StartInfo = startInfo };
    process.Start();
    process.WaitForExit();

    if (process.ExitCode != 0)
    {
      var error = process.StandardError.ReadToEnd();
      throw new InvalidOperationException($"Git command failed: {error}");
    }
  }
}
