namespace Api.Models.Enums;

public static class UserRole
{
  public const string Admin = "Admin";
  public const string User = "User";

  public static bool IsValidRole(string role)
  {
    return role == Admin || role == User;
  }
}
