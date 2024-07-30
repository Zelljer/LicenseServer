using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
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
		public async Task<IHTTPResult> Register(UserAPI.UserRegistrationRequest user)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					var errorResult = new Fail();

					var existUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
					if (existUser != null)
						errorResult.Data.Add("Логин занят");

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Name, "Укажите имя"));

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Surname, "Укажите фамилию"));

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Patronymic, "Укажите отчество"));

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Login, "Укажите логин"));

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Password, "Укажите пароль"));

					if (!Enum.IsDefined(typeof(RoleType), user.Role))
						errorResult.Data.Add("Не существующая роль");

					if (errorResult.Data.Any())
						return errorResult;

					var currentUser = new UserEntity
					{
						Name = user.Name,
						Surname = user.Surname,
						Patronymic = user.Patronymic,
						Login = user.Login,
						Password = Hasher.HashPassword(user.Password),
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
					var errorResult = new Fail();

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Login, "Укажите логин"));

					errorResult.Data
						.AddRange(Validator
						.IsValidData(user.Password, "Укажите пароль"));

					if (errorResult.Data.Any())
						return errorResult;

					var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

					if (currentUser == null)
						return new Fail { Data = { "Нет пользователя с таким логином" } };

					if (!Hasher.VerifyPassword(currentUser.Password, user.Password))
						return new Fail { Data = { "Не правильный пароль" } };

					var token = GenerateToken(currentUser);
					_cookieManager.SetAccessTokenCookie(token);

					return new Success<string> { Data = token };
				}
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		private string GenerateToken(UserEntity user)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Login),
				new Claim(ClaimTypes.Role, user.Role.ToString()),
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
	}


}


