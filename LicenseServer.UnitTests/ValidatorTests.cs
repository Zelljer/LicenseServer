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
			string errorText = "������ ��������";

			var result = Validator.DataValidation(input, errorText);

			Assert.IsTrue(result.Contains(errorText));
		}

		[TestMethod]
		public void IsValidData_String_Null_ReturnsError()
		{
			string input = null;
			string errorText = "�� ������� ������";

			var result = Validator.DataValidation(input, errorText);

			Assert.IsTrue(result.Contains(errorText));
		}

		[TestMethod]
		public void IsValidData_Int_Negative_ReturnsError()
		{
			int input = -1;
			string errorText = "������������� �����";

			var result = Validator.DataValidation(input, errorText);

			Assert.IsTrue(result.Contains(errorText));
		}

		[TestMethod]
		public void IsValidEmail_EmptyEmail_ReturnsError()
		{
			string email = "";
			var expectedErrors = "������� ��. �����";

			var result = Validator.EmailValidation(email);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidEmail_InvalidEmail_ReturnsError()
		{
			string email = "invalid-email";
			var expectedErrors = "������� �� ���������� �����";

			var result = Validator.EmailValidation(email);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_EmptyPhone_ReturnsError()
		{
			string phone = "";
			var expectedErrors = "������� ����� ��������";

			var result = Validator.PhoneValidation(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_InvalidStart_ReturnsError()
		{
			string phone = "64531232345";
			var expectedErrors = "����� ������ ���������� �� +7, 7 ��� 8";

			var result = Validator.PhoneValidation(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_InvalidFormat_ReturnsError()
		{
			string phone = "8234a235453";
			var expectedErrors = "� ������ �������� �� ������ �������������� ����� � ������ �������";

			var result = Validator.PhoneValidation(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidPhone_InvalidLength_ReturnsError()
		{
			string phone = "893465478342";
			var expectedErrors = "����� �������� ������ ��������� 11 ����";

			var result = Validator.PhoneValidation(phone);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

        [TestMethod]
        public void IsValidDate_InvalidDate_ReturnsError()
        {
            string date1 = "31.09.2024";
            string date2 = "23/09/2024";
            var expectedErrors = "������� ���������� ���� �������� �������� � ������� dd.mm.yyyy";

            var result1 = Validator.DateValidation(date1);
            var result2 = Validator.DateValidation(date2);

            Assert.IsTrue(result1.Contains(expectedErrors));
            Assert.IsTrue(result2.Contains(expectedErrors));
        }

        [TestMethod]
        public void IsValidDate_DateEarlier_ReturnsError()
        {
            string date = "01.01.2000";
            var expectedErrors = "���� �� ����� ���� ������ �������� ���";

            var result = Validator.DateValidation(date);

            Assert.IsTrue(result.Contains(expectedErrors));
        }
    }
}
	
