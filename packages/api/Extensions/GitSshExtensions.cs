using Api.Settings;
using Microsoft.Extensions.Options;

namespace Api.Extensions;

public static class GitSshExtensions
{
  public static IServiceCollection AddGitSshConfiguration(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    // Bind and validate settings
    var sshSettings =
      configuration.GetSection("GitSsh").Get<GitSshSettings>()
      ?? throw new InvalidOperationException("GitSsh section missing");
    sshSettings.Validate();

    // Registering settings for DI
    services.Configure<GitSshSettings>(configuration.GetSection("GitSsh"));

    return services;
  }

  public static void WriteGitSshConfigFiles(
    this IServiceProvider services,
    string templateDir = "Templates"
  )
  {
    // Get settings from DI
    var sshSettings = services.GetRequiredService<IOptions<GitSshSettings>>().Value;
    sshSettings.Validate();

    // Compute full template path
    var fullTemplateDir = Path.Combine(AppContext.BaseDirectory, templateDir);

    // Ensure target directory exists
    if (!Directory.Exists(sshSettings.TargetScriptDir))
      Directory.CreateDirectory(sshSettings.TargetScriptDir);

    // Define scripts to generate
    var scripts = new Dictionary<string, string>
    {
      ["authorized-keys"] = Path.Combine(fullTemplateDir, "authorized-keys.template.sh"),
      ["git-wrapper"] = Path.Combine(fullTemplateDir, "git-wrapper.template.sh"),
    };

    foreach (var scriptTemplate in scripts)
    {
      var outputPath = Path.Combine(sshSettings.TargetScriptDir, scriptTemplate.Key);

      if (!File.Exists(scriptTemplate.Value))
        throw new FileNotFoundException("Template not found", scriptTemplate.Value);

      // Read template and replace placeholders
      var content = File.ReadAllText(scriptTemplate.Value)
        .Replace("{{AppUrl}}", sshSettings.AppUrl)
        .Replace("{{GitUser}}", sshSettings.GitUser);

      // Write final script
      File.WriteAllText(outputPath, content);

      // Make executable
      var chmod = System.Diagnostics.Process.Start("chmod", $"+x {outputPath}");
      chmod.WaitForExit();
    }
  }
}
