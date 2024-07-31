using LicenseServer.Domain.Utils;

namespace LicenseServer.UnitTests
{
	[TestClass]
	public class ValidatorTests
	{
		[TestMethod]
		public void IsValidData_String_Empty_ReturnsError()
		{
			string input = "";
			string errorText = "Пустое значение";

			var result = Validator.IsValidData(input, errorText);

			Assert.IsTrue(result.Contains(errorText));
		}

		[TestMethod]
		public void IsValidData_String_Null_ReturnsError()
		{
			string input = null;
			string errorText = "Не введены данные";

			var result = Validator.IsValidData(input, errorText);

			Assert.IsTrue(result.Contains(errorText));
		}

		[TestMethod]
		public void IsValidData_Int_Negative_ReturnsError()
		{
			int input = -1;
			string errorText = "Отрицательное число";

			var result = Validator.IsValidData(input, errorText);

			Assert.IsTrue(result.Contains(errorText));
		}

		[TestMethod]
		public void IsValidEmail_EmptyEmail_ReturnsError()
		{
			string email = "";
			var expectedErrors = "Укажите эл. почту";

			var result = Validator.IsValidEmail(email);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidEmail_InvalidEmail_ReturnsError()
		{
			string email = "invalid-email";
			var expectedErrors = "Введена не корректная почта";

			var result = Validator.IsValidEmail(email);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_EmptyPhone_ReturnsError()
		{
			string phone = "";
			var expectedErrors = "Укажите номер телефона";

			var result = Validator.IsValidPhone(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_InvalidStart_ReturnsError()
		{
			string phone = "64531232345";
			var expectedErrors = "Номер должен начинаться на +7, 7 или 8";

			var result = Validator.IsValidPhone(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_InvalidFormat_ReturnsError()
		{
			string phone = "8234a235453";
			var expectedErrors = "В номере телефона не должны присутствовать буквы и другие символы";

			var result = Validator.IsValidPhone(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_InvalidLength_ReturnsError()
		{
			string phone = "893465478342";
			var expectedErrors = "Номер телефона должен содержать 11 цифр";

			var result = Validator.IsValidPhone(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}
	}
}
	
