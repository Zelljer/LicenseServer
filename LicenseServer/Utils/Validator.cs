using LicenseServer.Models.Database;
using System.Text.RegularExpressions;

namespace LicenseServer.Utils
{
	public static class Validator
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

		public static List<string> IsValidInn(string inn)
		{
			List<string> errors = new List<string>();
			if (inn.Length == 0)
				errors.Add("Укажите ИНН");
			else if (!Regex.IsMatch(inn, @"^\d{10}$|\d{12}$"))
				errors.Add("Укажите корректный ИНН: для физических лиц ИНН состоит из 12 цифр, для юридических лиц ИНН состоит из 10 цифр");
			return errors;
		}

		public static List<string> IsValidKpp(string kpp)
		{
			List<string> errors = new List<string>();

			if (kpp.Length == 0)
				errors.Add("Не указанно КПП");

			else if (!Regex.IsMatch(kpp, @"^\d{4}\d{4}\d{1}$"))
				errors.Add("Укажите корректный КПП: Первые 4 цифры — код региона, cледующие 4 цифры — код налогового органа, последняя цифра — контрольная цифра");
			return errors;
		}

		public static List<string> IsValidEmail(string email)
		{
			List<string> errors = new List<string>();
			if (email.Length == 0)
			{
				errors.Add("Укажите эл. почту");
				return errors;
			}

			if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+"))
				errors.Add("У вашей почты не корректное имя почтового ящика");
			if (!email.Contains("@"))
				errors.Add("У вашей почты отсутствует символ 'собака'");
			if (!Regex.IsMatch(email, @"@[a-zA-Z0-9.-]+"))
				errors.Add("У вашей почты не корректная доменная часть");
			if (!email.Contains("."))
				errors.Add("У вашей почты нет точки после доменной части");
			if (!Regex.IsMatch(email, @"\.[a-zA-Z]{2,6}$"))
				errors.Add("У вашей почты не корректная часть домена верхнего уровня");

			return errors;
		}

		public static List<string> IsValidPhone(string phone)
		{
			List<string> errors = new List<string>();
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

		public static List<string> IsValidPrice(long price)
		{
			List<string> errors = new List<string>();
			if (price < 0)
				errors.Add("Укажите корректную цену (в копкейках)");
			return errors;
		}

		public static List<string> IsValidProgram(ProgramType program)
		{
			List<string> errors = new List<string>();
			if (program < 0)
				errors.Add("Указана не корректная программа");
			else if (!Enum.IsDefined(typeof(ProgramType), program))
				errors.Add("Указана не существующая программа");
			return errors;
		}
		/*public static List<string> IsValidTarif(Tarif tarif)
		{
			List<string> errors = new List<string>();

			if (!Enum.IsDefined(typeof(ProgramType), tarif.Program))
				errors.Add("Указана не существующая программа");
			if (tarif.Price < 0)
				errors.Add("Укажите корректную цену");
			if (tarif.DaysCount <= 0)
				errors.Add("Количество дней не может быть меньше или равняться 0");

			e
		}*/
	}
}
