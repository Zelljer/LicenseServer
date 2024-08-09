using System.Globalization;
using System.Text.RegularExpressions;

namespace LicenseServer.Domain.Utils
{
	public class Validator
	{
		public static List<string> IsValidData<T>(T data, string errorText)
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

		public static List<string> IsValidEmail(string email)
		{
			if (email.Length == 0)
                return new() { "Укажите эл. почту" };

            List<string> errors = [];

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$"))
				errors.Add("Введена не корректная почта");

			return errors;
		}

		public static List<string> IsValidPhone(string phone)
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

        public static List<string> IsValidDate(string date)
        {
			if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime daeteTime))
				return new() { "Введите корректную дату создания лицензии в формате dd.mm.yyyy" };

            else if (daeteTime < DateTime.Now)
                return new() { "Дата не может быть раньше текущего дня" };

            return [];
        }
    }
}
