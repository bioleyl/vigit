using Api.Settings;
using Microsoft.Extensions.Options;

namespace Api.Extensions;

public static class GitExtensions
{
  public static IServiceCollection AddGitConfiguration(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    // Bind and validate settings
    var gitSettings =
      configuration.GetSection("Git").Get<GitSettings>()
      ?? throw new InvalidOperationException("Git section missing");
    gitSettings.Validate();

    // Registering settings for DI
    services.Configure<GitSettings>(configuration.GetSection("Git"));

    return services;
  }

  public static void WriteGitConfigFiles(
    this IServiceProvider services,
    string templateDir = "Templates"
  )
  {
    // Get settings from DI
    var gitSettings = services.GetRequiredService<IOptions<GitSettings>>().Value;
    gitSettings.Validate();

    // Compute full template path
    var fullTemplateDir = Path.Combine(AppContext.BaseDirectory, templateDir);

    // Ensure target directory exists
    if (!Directory.Exists(gitSettings.TargetScriptDir))
      Directory.CreateDirectory(gitSettings.TargetScriptDir);

    // Define scripts to generate
    var scripts = new Dictionary<string, string>
    {
      ["authorized-keys"] = Path.Combine(fullTemplateDir, "authorized-keys.template.sh"),
      ["git-wrapper"] = Path.Combine(fullTemplateDir, "git-wrapper.template.sh"),
    };

    foreach (var scriptTemplate in scripts)
    {
      var outputPath = Path.Combine(gitSettings.TargetScriptDir, scriptTemplate.Key);

      if (!File.Exists(scriptTemplate.Value))
        throw new FileNotFoundException("Template not found", scriptTemplate.Value);

      // Read template and replace placeholders
      var content = File.ReadAllText(scriptTemplate.Value)
        .Replace("{{AppUrl}}", gitSettings.AppUrl)
        .Replace("{{GitUser}}", gitSettings.GitUser)
        .Replace("{{ScriptPath}}", gitSettings.TargetScriptDir)
        .Replace("{{RepoRoot}}", gitSettings.RepositoriesRoot);

      // Write final script
      File.WriteAllText(outputPath, content);

      // Make executable
      var chmod = System.Diagnostics.Process.Start("chmod", $"+x {outputPath}");
      chmod.WaitForExit();
    }
  }
}
