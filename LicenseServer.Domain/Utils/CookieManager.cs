namespace LicenseServer.Domain.Utils
{
	public class CookieManager(IHttpContextAccessor httpContextAccessor)
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public void SetAccessTokenCookie(string token)
		{
			_httpContextAccessor.HttpContext.Response.Cookies.Append(Constans.HeaderAuthorize, token, new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTimeOffset.UtcNow.AddHours(12),
				Secure = true,
				SameSite = SameSiteMode.None
			});
		}
	}
}
