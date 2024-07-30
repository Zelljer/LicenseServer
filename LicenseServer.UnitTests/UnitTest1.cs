using LicenseServer.Domain.Utils;

namespace LicenseServer.UnitTests
{
	[TestClass]
	public class UnitTest1
	{

		[TestMethod]
		public void IsValidDataTest()
		{
			var exampleInt1 = 0;
			var exampleInt2 = -1;
			var exampleInt3 = 1;

			var exampleStr1 = string.Empty;
			var exampleStr2 = "Example";


			var intTesting1 = Validator.IsValidData(exampleInt1, "Ошибка");
			var intTesting2 = Validator.IsValidData(exampleInt2, "Ошибка");
			var intTesting3 = Validator.IsValidData(exampleInt3, "Ошибка");

			var strTesting1 = Validator.IsValidData(exampleStr1, "Ошибка");
			var strTesting2 = Validator.IsValidData(exampleStr2, "Ошибка");


			CollectionAssert.AreEqual(new List<string> { "Ошибка" }, intTesting1);
			CollectionAssert.AreEqual(new List<string> { "Ошибка" }, intTesting2);
			CollectionAssert.AreEqual(new List<string> { }, intTesting3); 

			CollectionAssert.AreEqual(new List<string> { "Ошибка" }, strTesting1);
			CollectionAssert.AreEqual(new List<string> { }, strTesting2); 
		}

		[TestMethod]
		public void IsValidEmailTest()
		{

			var goodEmailExample1 = "example@mail.ru";
			var goodEmailExample2 = "nick@gmail.com";
			var goodEmailExample3 = "user@yandex.ru";

			var badEmailExample1 = "example@example";
			var badEmailExample2 = "example@example.c";
			var badEmailExample3 = "example@.com";
			var badEmailExample4 = "example@example.com.";
			var badEmailExample5 = string.Empty;


			var goodTesting1 = Validator.IsValidEmail(goodEmailExample1);
			var goodTesting2 = Validator.IsValidEmail(goodEmailExample2);
			var goodTesting3 = Validator.IsValidEmail(goodEmailExample3);

			var badTesting1 = Validator.IsValidEmail(badEmailExample1);
			var badTesting2 = Validator.IsValidEmail(badEmailExample2);
			var badTesting3 = Validator.IsValidEmail(badEmailExample3);
			var badTesting4 = Validator.IsValidEmail(badEmailExample4);
			var badTesting5 = Validator.IsValidEmail(badEmailExample5);


			CollectionAssert.AreEqual(new List<string>(), goodTesting1);
			CollectionAssert.AreEqual(new List<string>(), goodTesting2);
			CollectionAssert.AreEqual(new List<string>(), goodTesting3);

			CollectionAssert.AreEqual(new List<string> { "Введена не корректная почта" }, badTesting1);
			CollectionAssert.AreEqual(new List<string> { "Введена не корректная почта" }, badTesting2);
			CollectionAssert.AreEqual(new List<string> { "Введена не корректная почта" }, badTesting3);
			CollectionAssert.AreEqual(new List<string> { "Введена не корректная почта" }, badTesting4);
			CollectionAssert.AreEqual(new List<string> { "Укажите эл. почту" }, badTesting5);
		}
	

	[TestMethod]
		public void IsValidPhoneTest()
		{
			var goodPhoneExample1 = "89427654323";
			var goodPhoneExample2 = "79764322345";
			var goodPhoneExample3 = "+79658765443";

			var badPhoneExample1 = "54563428743";
			var badPhoneExample2 = "892365434543214";
			var badPhoneExample3 = "8462f425245";
			var badPhoneExample4 = "8-927-654-34-91";
			var badPhoneExample5 = string.Empty;


			var goodTesting1 = Validator.IsValidPhone(goodPhoneExample1);
			var goodTesting2 = Validator.IsValidPhone(goodPhoneExample2);
			var goodTesting3 = Validator.IsValidPhone(goodPhoneExample3);

			var badTesting1 = Validator.IsValidPhone(badPhoneExample1);
			var badTesting2 = Validator.IsValidPhone(badPhoneExample2);
			var badTesting3 = Validator.IsValidPhone(badPhoneExample3);
			var badTesting4 = Validator.IsValidPhone(badPhoneExample4);
			var badTesting5 = Validator.IsValidPhone(badPhoneExample5);


			CollectionAssert.AreEqual(new List<string>(), goodTesting1);
			CollectionAssert.AreEqual(new List<string>(), goodTesting2);
			CollectionAssert.AreEqual(new List<string>(), goodTesting3);

			CollectionAssert.AreEqual(new List<string> { "Номер должен начинаться на +7, 7 или 8" }, badTesting1);

			CollectionAssert.AreEqual(new List<string> { "Номер телефона должен содержать 11 цифр" }, badTesting2);

			CollectionAssert.AreEqual(new List<string> { "В номере телефона не должны присутствовать буквы и другие символы", 
														"Номер должен начинаться на +7, 7 или 8" }, badTesting3);
			
			CollectionAssert.AreEqual(new List<string> { "В номере телефона не должны присутствовать буквы и другие символы",
														"Номер должен начинаться на +7, 7 или 8",
														"Номер телефона должен содержать 11 цифр" }, badTesting4);

			CollectionAssert.AreEqual(new List<string> { "Укажите номер телефона" }, badTesting5);
		}
	}
}