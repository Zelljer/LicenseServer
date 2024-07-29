using LicenseServer.Database;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LicenseServer.Domain.Methods
{
	public class UserService(IConfiguration configuration, CookieManager cookieManager)
	{
		private readonly IConfiguration _configuration = configuration;
		private readonly CookieManager _cookieManager = cookieManager;

		[HttpPost("register")]
		public async Task<IHTTPResult> Register( UserAPI.UserRegistrationRequest user)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					var currentUser = new UserEntity
					{
						Name = user.Name,
						Surname = user.Surname,
						Patronymic = user.Patronymic,
						Login = user.Login,
						Password = user.Password,
						Role = user.Role,
					};

					context.Users.Add(currentUser);
					await context.SaveChangesAsync();

					return new Success<UserEntity> { Data = currentUser };
				}
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		[HttpPost("login")]
		public async Task<IHTTPResult> Login(UserAPI.UserAuthentificationRequest user)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
					if (currentUser == null)
						return new Fail { Data = { "Нет пользователя с таким логином" } };
					if (currentUser.Password != user.Password)
						return new Fail { Data = { "Не правильный пароль" } };

					var token = GenerateToken(currentUser);
					_cookieManager.SetAccessTokenCookie(token);
					return new Success<string> { Data = token };
				}
			}
			catch (Exception ex)
			{
				return new Fail { Data = { ex.Message } };
			}
		}

		private string GenerateToken(UserEntity user)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Login),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Issuer"],
				claims: claims,
				expires: DateTime.Now.AddHours(12),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		/*private string HashPassword(string password)
		{
			// Реализуйте хэширование пароля
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				return Convert.ToBase64String(hash);
			}
		}

		private bool VerifyPassword(string storedHash, string password)
        {
            var hash = HashPassword(password);
            return storedHash == hash;
        }*/
	}


}


