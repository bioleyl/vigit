namespace Api.Models.Enums;

public static class UserRepositoryRole
{
  public const string Default = Collaborator;
  public const string Owner = "Owner";
  public const string Collaborator = "Collaborator";

  public static bool IsValidRole(string role)
  {
    return role == Owner || role == Collaborator;
  }
}
