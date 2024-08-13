using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using ValidationsCollection;

namespace LicenseServer.Domain.Utils
{
    public class Validator
	{
		public static List<string> DataValidation<T>(T data, string errorText)
		{
			List<string> errors = [];

			if (data == null)
			{
				errors.Add(errorText);
				return errors;
			}

			if (data is string && data.Equals(""))
				errors.Add(errorText);

			if (data is int && int.TryParse(data.ToString(), out int convertedValue))
			{
				if (convertedValue <= 0)
					errors.Add(errorText);
			}

			return errors;
		}

        public static bool isValidObject(object obj)
        {
            if (obj == null)
                return false;
            return true;
        }

        public static List<string> OrganizationValidation(OrganizationAPI.OrganizationRequest organization)
        {
            var errors = new List<string>();

            errors.AddRange(DataValidation(organization.Name, "Не корректное название"));

            if (!Validations.IsValidInn(organization.Inn))
                errors.Add("Не корректный ИНН");

            if (organization.Inn.Length != 12 && !Validations.IsValidKpp(organization.Kpp))
                errors.Add("Не корректный КПП");

            errors.AddRange(EmailValidation(organization.Email));

            errors.AddRange(PhoneValidation(organization.Phone));

            return errors;
        }

        public static List<string> TarifValidation(TarifAPI.TarifRequest tarif)
        {
            var errors = new List<string>();

			errors.AddRange(DataValidation(tarif.Name, "Не корректное название"));

			if (!Enum.IsDefined(typeof(ProgramType), tarif.Program))
                errors.Add("Не существующая прогрмма");

            if (tarif.Price < 0)
                errors.Add("Не корректная цена");

            errors
                .AddRange(DataValidation(tarif.DaysCount, "Укажите количество дней действия лицензии"));

            return errors;
        }

        public static List<string> LicenseValidation(LicenseAPI.LicenseRequest license)
        {
            var errors = new List<string>();

            errors
                .AddRange(DataValidation(license.OrganizationId, "Не корректный Id организации"));

            errors
                .AddRange(DataValidation(license.TarifId, "Не корректный Id тарифа"));

            var orgErrors = OrgIdValidation(license.OrganizationId);
            var tarifErrors = TarifIdValidation(license.TarifId);

            if (orgErrors.Any())
                errors.AddRange(orgErrors);

            if (tarifErrors.Any())
                errors.AddRange(tarifErrors);

            errors
                .AddRange(DateValidation(license.DateStart));

            return errors;
        }

        public static async Task<List<string>> UserRegistrationValidation(UserAPI.UserRegistrationRequest user)
        {
            using var context = ApplicationContext.New;
            var errors = new List<string>();

            var existUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

            if (existUser != null)
                errors.Add("Логин занят");

            errors
                .AddRange(DataValidation(user.Name, "Укажите имя"));

            errors
                .AddRange(DataValidation(user.Surname, "Укажите фамилию"));

            errors
                .AddRange(DataValidation(user.Patronymic, "Укажите отчество"));

            errors
                .AddRange(DataValidation(user.Login, "Укажите логин"));

            errors
                .AddRange(DataValidation(user.Password, "Укажите пароль"));

            if (!Enum.IsDefined(typeof(RoleType), user.Role))
                errors.Add("Не существующая роль");

            return errors;
        }

        public static List<string> UserAuthorizationValidation(UserAPI.UserAuthentificationRequest user)
        {
            var errors = new List<string>();

            errors
                .AddRange(DataValidation(user.Login, "Укажите логин"));

            errors
                .AddRange(DataValidation(user.Password, "Укажите пароль"));

            return errors;
        }

        public static List<string> UserAuthentificationValidation(UserEntity user, string password)
        {
            if (!isValidObject(user))
                return new() { "Нет пользователя с таким логином" };

            if (!Hasher.VerifyPassword(user.Password, password))
                return new() { "Не правильный пароль" };

            return new ();
        }

        public static List<string> OrgIdAndProgIdValidation(int organizationId, ProgramType programId)
        {
            using var context = ApplicationContext.New;
            var errors = new List<string>();

            errors.AddRange(OrgIdValidation(organizationId));

            errors.AddRange(ProgIdValidation(programId));

            return errors;
        }

        public static List<string> OrgIdValidation(int organizationId)
        {
            using var context = ApplicationContext.New;
            var errors = new List<string>();

            errors.AddRange(DataValidation(organizationId, "Не корректный Id организации"));

            if (context.Organizations.Find(organizationId) == null)
                errors.Add("Нет организации с таким Id");

            return errors;
        }

        public static List<string> TarifIdValidation(int tarifId)
        {
            using var context = ApplicationContext.New;
            var errors = new List<string>();

            errors.AddRange(DataValidation(tarifId, "Не корректный Id тарифа"));

            if (context.Tarifs.Find(tarifId) == null)
                errors.Add("Нет тарифа с таким Id");

            return errors;
        }

        public static List<string> ProgIdValidation(ProgramType programId)
        {
            using var context = ApplicationContext.New;
            var errorResult = new List<string>();

            errorResult.AddRange(DataValidation(programId, "Не корректный Id программы"));

            if (!Enum.IsDefined(typeof(ProgramType), programId))
                errorResult.Add("Указана не существующая программа");

            return errorResult;
        }


        public static List<string> PageValidation(int currentPage, int pageSize)
        {
            var errors = new List<string>();

            errors
                .AddRange(DataValidation(currentPage, "Укажите нужный номер страницы. Отсчет страниц начинается с 1")
                );

            errors
                .AddRange(DataValidation(pageSize, "Укажите, сколько элементов будет отображаться на странице (размер страницы)")
                );

            return errors;
        }

        public static List<string> EmailValidation(string email)
		{
			if (email.Length == 0)
                return new() { "Укажите эл. почту" };

            List<string> errors = [];

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$"))
				errors.Add("Введена не корректная почта");

			return errors;
		}

		public static List<string> PhoneValidation(string phone)
		{
			if (phone.Length == 0)
                return new() { "Укажите номер телефона" };

            List<string> errors = [];

            if (!Regex.IsMatch(phone, @"^[+]?\d+$"))
				errors.Add("В номере телефона не должны присутствовать буквы и другие символы");

			if (!Regex.IsMatch(phone, @"^(\+7|7|8)\d*$"))
				errors.Add("Номер должен начинаться на +7, 7 или 8");

			int phoneLength = phone.Length;
			int minLength = phone.StartsWith("+") ? 12 : 11; 
			int maxLength = phone.StartsWith("+") ? 13 : 11; 

			if (phoneLength < minLength || phoneLength > maxLength)
				errors.Add("Номер телефона должен содержать 11 цифр");

			return errors;
		}

        public static List<string> DateValidation(string date)
        {
			if (!DateTime.TryParseExact(date, Constans.TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime daeteTime))
				return new() { $"Введите корректную дату создания лицензии в формате {Constans.TimeFormat}" };

            else if (daeteTime < DateTime.Now)
                return new() { "Дата не может быть раньше текущего дня" };

            return [];
        }
    }
}
