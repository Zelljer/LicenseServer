using LicenseServer.Database.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LicenseServer.Domain.Utils
{
	public static class TokenManager
	{
		public static string Key => "5674585686575768456769735897364579869805675896705986725-467859206798576";
		public static string Issuer => "issuer";

		public static string GenerateToken(UserEntity user)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Login),
				new Claim(ClaimTypes.Role, user.Role.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: Issuer,
				audience: Issuer,
				claims: claims,
				expires: DateTime.Now.AddHours(12),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

}
