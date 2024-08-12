using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;

namespace LicenseServer.Domain.Methods
{
    public class UserService(CookieManager cookieManager)
	{
		public async Task<HTTPResult<string>> UserRegistration(UserAPI.UserRegistrationRequest user)
		{
			try
			{
				var errorResult = await Validator.UserRegistrationValidation(user);

				if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var currentUser = DataGetter.UserAPIToUserEntity(user);

                await DataManager.AddEntityAsync(currentUser);

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
				var errorResult = Validator.UserAuthorizationValidation(user);

				if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var currentUser = await DataGetter.UserEntityByLogin(user);

				errorResult = Validator.UserAuthentificationValidation(currentUser, user.Password);

                if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var token = TokenManager.SaveTokenToCookie(currentUser, cookieManager);

                return HttpResults.StringResult.Success(token);
            }
			catch
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
		}
	}
}


