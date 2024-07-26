using LicenseServer.Database.Entity;
using LicenseServer.Database;
using LicenseServer.Domain.Models;
using ValidationsCollection;

namespace LicenseServer.Domain.Methods
{
	public class UserService
	{
		public async Task<IHTTPResult> Login(UserAPI.UserAuthentificationRequest user)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					var currentUser = context.Users.FirstOrDefault(u=>u.Login == user.Login && u.Password == user.Password);

					if (currentUser == null)
						return new Fail { Data = { "Пользователь не найден" } };
					
					//var token = 

					return new Success<UserEntity> { Data = currentUser };
				}
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		public async Task<IHTTPResult> Register(UserAPI.UserRegistrationRequest user)
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
	}
}

