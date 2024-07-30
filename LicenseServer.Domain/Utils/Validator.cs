using System.Text.RegularExpressions;

namespace LicenseServer.Domain.Utils
{
	public class Validator
	{
		public static List<string> IsValidData<T>(T data, string errorText)
		{
			List<string> errors = [];

			if (data is string && (data.Equals("") || data == null))
				errors.Add(errorText);

			if (data is int && int.TryParse(data.ToString(), out int convertedValue))
			{
				if (convertedValue <= 0)
					errors.Add(errorText);
			}

			return errors;
		}

		public static List<string> IsValidEmail(string email)
		{
			List<string> errors = [];

			if (email.Length == 0)
			{
				errors.Add("Укажите эл. почту");
				return errors;
			}

			if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$"))
				errors.Add("Введена не корректная почта");

			return errors;
		}

		public static List<string> IsValidPhone(string phone)
		{
			List<string> errors = [];
			if (phone.Length == 0)
			{
				errors.Add("Укажите номер телефона");
				return errors;
			}

			if (!Regex.IsMatch(phone, @"^[+]?\d+$"))
				errors.Add("В номере телефона не должны присутствовать буквы и другие символы");

			if (!Regex.IsMatch(phone, @"^(\+7|7|8)\d*$"))
				errors.Add("Номер должен начинаться на +7, 7 или 8");

			// Проверка на длину номера телефона
			int phoneLength = phone.Length;
			int minLength = phone.StartsWith("+") ? 12 : 11; 
			int maxLength = phone.StartsWith("+") ? 13 : 11; 

			if (phoneLength < minLength || phoneLength > maxLength)
				errors.Add($"Номер телефона должен содержать 11 цифр");

			return errors;
		}
	}
}
