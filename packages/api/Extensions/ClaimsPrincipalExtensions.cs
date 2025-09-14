using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipalExtensions
{
  public static int GetUserId(this ClaimsPrincipal user)
  {
    var value =
      user.FindFirstValue(ClaimTypes.NameIdentifier)
      ?? throw new InvalidOperationException("User ID claim missing");
    return int.Parse(value);
  }

  public static bool IsAdmin(this ClaimsPrincipal user)
  {
    return user.IsInRole("Admin");
  }
}
