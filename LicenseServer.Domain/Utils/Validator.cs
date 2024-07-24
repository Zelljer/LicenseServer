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

			if (!Regex.IsMatch(phone, @"^[\+\d\s\-\(\)]+$"))
				errors.Add("В номере телефона не должны присутствовать буквы и другие символы");

			if (!Regex.IsMatch(phone, @"^(\+7|7|8)"))
				errors.Add("Номер должен начинаться на +7, 7 или 8");

			if (!Regex.IsMatch(phone, @"^\+?\d{1,2}\s*\(?\d{3}\)?\s*\d{3}[-]?\d{2}[-]?\d{2}$"))
				errors.Add("В номере телефона недопустмое количество цифр");

			return errors;
		}
	}
}
