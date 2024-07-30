namespace LicenseServer.Domain.Utils
{
	public static class Hasher
	{
		public static string HashPassword(string password) =>
			BCrypt.Net.BCrypt.EnhancedHashPassword(password);

		public static bool VerifyPassword(string storedHash, string password) =>
			BCrypt.Net.BCrypt.EnhancedVerify(password, storedHash);
	}
}
