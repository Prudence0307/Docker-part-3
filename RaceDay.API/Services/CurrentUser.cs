namespace RaceDay.API.Services
{
  public static class CurrentUser
  {
    public static string? Role(HttpContext http) =>
      http.Session.GetString("Role") ?? http.Request.Headers["X-User-Role"].FirstOrDefault();

    public static int? Id(HttpContext http)
    {
      var fromSession = http.Session.GetInt32("UserId");
      if (fromSession != null) return fromSession;
      return int.TryParse(http.Request.Headers["X-User-Id"].FirstOrDefault(), out var id) ? id : null;
    }
  }
}
