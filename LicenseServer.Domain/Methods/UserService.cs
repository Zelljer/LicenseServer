using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Domain.Methods
{
	public class UserService(CookieManager cookieManager)
	{
		public async Task<HTTPResult<string>> UserRegistration(UserAPI.UserRegistrationRequest user)
		{
			try
			{
				using var context = ApplicationContext.New;
				var errorResult = new List<string>();

				var ttt = context.Users.ToList();
                var existUser1 = context.Users.FirstOrDefault(u => u.Login == user.Login);

                var existUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
				if (existUser != null)
					errorResult.Add("Логин занят");

				errorResult
					.AddRange(Validator
					.IsValidData(user.Name, "Укажите имя"));

				errorResult
					.AddRange(Validator
					.IsValidData(user.Surname, "Укажите фамилию"));

				errorResult
					.AddRange(Validator
					.IsValidData(user.Patronymic, "Укажите отчество"));

				errorResult
					.AddRange(Validator
					.IsValidData(user.Login, "Укажите логин"));

				errorResult
					.AddRange(Validator
					.IsValidData(user.Password, "Укажите пароль"));

				if (!Enum.IsDefined(typeof(RoleType), user.Role))
					errorResult.Add("Не существующая роль");

				if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

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

                return HttpResults.StringResult.Success("Пользователь зарегистрирован успешно"); 
			}
			catch
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<string>> UserLogin(UserAPI.UserAuthentificationRequest user)
		{
			try
			{
				using var context = ApplicationContext.New;				
				var errorResult = new List<string>();

                errorResult
					.AddRange(Validator
					.IsValidData(user.Login, "Укажите логин"));

				errorResult
					.AddRange(Validator
					.IsValidData(user.Password, "Укажите пароль"));

				if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

				if (currentUser == null)
                    return HttpResults.StringResult.Fail("Нет пользователя с таким логином");

				if (!Hasher.VerifyPassword(currentUser.Password, user.Password))
                    return HttpResults.StringResult.Fail("Не правильный пароль");  

				var token = TokenManager.GenerateToken(currentUser);
				cookieManager.SetAccessTokenCookie(token);

                return HttpResults.StringResult.Success(token);
            }
			catch
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
		}
	}
}


