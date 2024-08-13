using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
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
            var expectedErrors = $"������� ���������� ���� �������� �������� � ������� {Constans.TimeFormat}";

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

		[TestMethod]
		public void IsValidObject_NullObject_ReturnsFalse()
		{
			object obj = null;

			var result = Validator.isValidObject(obj);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void IsValidOrganization_ValidName_ReturnsError()
		{
			var organization = new OrganizationAPI.OrganizationRequest
			{
				Name = "",
				Inn = "616608929424",
				Email = "example@mail.ru",
				Phone = "89871234343"
			};

			var expectedErrors = "�� ���������� ��������";

			var result = Validator.OrganizationValidation(organization);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidOrganization_ValidInn_ReturnsError()
		{
			var organization = new OrganizationAPI.OrganizationRequest
			{
				Name = "Name",
				Inn = "616608924",
				Email = "example@mail.ru",
				Phone = "89871234343"
			};

			var expectedErrors = "�� ���������� ���";

			var result = Validator.OrganizationValidation(organization);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidOrganization_ValidKpp_ReturnsError()
		{
			var organization = new OrganizationAPI.OrganizationRequest
			{
				Name = "Name",
				Inn = "7707083893",
				Kpp = "123123",
				Email = "example@mail.ru",
				Phone = "89871234343"
			};

			var expectedErrors = "�� ���������� ���";

			var result = Validator.OrganizationValidation(organization);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidOrganization_ValidEmail_ReturnsError()
		{
			var organization = new OrganizationAPI.OrganizationRequest
			{
				Name = "Name",
				Inn = "7707083893",
				Kpp = "123123",
				Email = "examplemail.ru",
				Phone = "89871234343"
			};

			var expectedErrors = "������� �� ���������� �����";

			var result = Validator.OrganizationValidation(organization);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidOrganization_ValidPhone_ReturnsError()
		{
			var organization = new OrganizationAPI.OrganizationRequest
			{
				Name = "Name",
				Inn = "7707083893",
				Kpp = "123123",
				Email = "examplemail.ru",
				Phone = "57645754886"
			};

			var expectedErrors = "������� �� ���������� �����";

			var result = Validator.OrganizationValidation(organization);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidTarif_ValidName_ReturnsError()
		{
			var tarif = new TarifAPI.TarifRequest
			{
				Name = "",
				Program = ProgramType.TriarRetail,
				Price = 200000,
				DaysCount = 1,
			};

			var expectedErrors = "�� ���������� ��������";

			var result = Validator.TarifValidation(tarif);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidTarif_ValidPrice_ReturnsError()
		{
			var tarif = new TarifAPI.TarifRequest
			{
				Name = "Name",
				Program = ProgramType.TriarRetail,
				Price = -1,
				DaysCount = 1,
			};

			var expectedErrors = "�� ���������� ����";

			var result = Validator.TarifValidation(tarif);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidTarif_ValidDays_ReturnsError()
		{
			var tarif = new TarifAPI.TarifRequest
			{
				Name = "Name",
				Program = ProgramType.TriarRetail,
				Price = 200000,
				DaysCount = 0,
			};

			var expectedErrors = "������� ���������� ���� �������� ��������";

			var result = Validator.TarifValidation(tarif);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidLicense_ValidOrgId_ReturnsError()
		{
			var license = new LicenseAPI.LicenseRequest
			{
				OrganizationId = 0,
				TarifId = 1,
				DateStart = "12.12.9999"
			};

			var expectedErrors = "�� ���������� Id �����������";

			var result = Validator.LicenseValidation(license);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidLicense_ValidTarifId_ReturnsError()
		{
			var license = new LicenseAPI.LicenseRequest
			{
				OrganizationId = 1,
				TarifId = 0,
				DateStart = "12.12.9999"
			};

			var expectedErrors = "�� ���������� Id ������";

			var result = Validator.LicenseValidation(license);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidLicense_ValidDate_ReturnsError()
		{
			var license = new LicenseAPI.LicenseRequest
			{
				OrganizationId = 1,
				TarifId = 1,
				DateStart = "12.12.2004"
			};

			var expectedErrors = "���� �� ����� ���� ������ �������� ���";

			var result = Validator.LicenseValidation(license);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserRegistration_ValidName_ReturnsError()
		{
			var user = new UserAPI.UserRegistrationRequest
			{
				Name = "",
				Surname = "Surname",
				Patronymic = "Patronymic",
				Login = "Login",
				Password = "Password",
				Role = RoleType.Admin
			};

			var expectedErrors = "������� ���";

			var result = Validator.UserRegistrationValidation(user);

			Assert.IsTrue(result.Result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserRegistration_ValidSurname_ReturnsError()
		{
			var user = new UserAPI.UserRegistrationRequest
			{
				Name = "Name",
				Surname = "",
				Patronymic = "Patronymic",
				Login = "Login",
				Password = "Password",
				Role = RoleType.Admin
			};

			var expectedErrors = "������� �������";

			var result = Validator.UserRegistrationValidation(user);

			Assert.IsTrue(result.Result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserRegistration_ValidPatronymic_ReturnsError()
		{
			var user = new UserAPI.UserRegistrationRequest
			{
				Name = "Name",
				Surname = "Surname",
				Patronymic = "",
				Login = "Login",
				Password = "Password",
				Role = RoleType.Admin
			};

			var expectedErrors = "������� ��������";

			var result = Validator.UserRegistrationValidation(user);

			Assert.IsTrue(result.Result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserRegistration_ValidLogin_ReturnsError()
		{
			var user = new UserAPI.UserRegistrationRequest
			{
				Name = "Name",
				Surname = "Surname",
				Patronymic = "Patronymic",
				Login = "",
				Password = "Password",
				Role = RoleType.Admin
			};

			var expectedErrors = "������� �����";

			var result = Validator.UserRegistrationValidation(user);

			Assert.IsTrue(result.Result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserRegistration_ValidPassword_ReturnsError()
		{
			var user = new UserAPI.UserRegistrationRequest
			{
				Name = "Name",
				Surname = "Surname",
				Patronymic = "Patronymic",
				Login = "Login",
				Password = "",
				Role = RoleType.Admin
			};

			var expectedErrors = "������� ������";

			var result = Validator.UserRegistrationValidation(user);

			Assert.IsTrue(result.Result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserAuthorization_ValidLogin_ReturnsError()
		{
			var user = new UserAPI.UserAuthentificationRequest
			{

				Login = "",
				Password = "Password",
			};

			var expectedErrors = "������� �����";

			var result = Validator.UserAuthorizationValidation(user);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidUserAuthorization_ValidPassword_ReturnsError()
		{
			var user = new UserAPI.UserAuthentificationRequest
			{

				Login = "Login",
				Password = "",
			};

			var expectedErrors = "������� ������";

			var result = Validator.UserAuthorizationValidation(user);

			Assert.IsTrue(result.Contains(expectedErrors));
		}

		[TestMethod]
		public void IsValidAuthentificationValidation_ValidUser_ReturnsError()
		{
			UserEntity user = null;

			var expectedErrors = "��� ������������ � ����� �������";

			var result = Validator.UserAuthentificationValidation(user, "123");

			Assert.IsTrue(result.Contains(expectedErrors));
		}
	}
}
	
