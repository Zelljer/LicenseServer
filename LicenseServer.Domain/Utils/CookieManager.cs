namespace LicenseServer.Domain.Utils
{
	public class CookieManager(IHttpContextAccessor httpContextAccessor)
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public void SetAccessTokenCookie(string token)
		{
			_httpContextAccessor.HttpContext.Response.Cookies.Append("Authorization", token, new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTimeOffset.UtcNow.AddHours(12),
				Secure = true, // Используйте true в продакшене
				SameSite = SameSiteMode.None
			});


		}
		public string GetAccessTokenCookie()
		{
			_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("access_token", out var token);
			return token;
		}
	}
}
