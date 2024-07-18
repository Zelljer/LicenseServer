namespace LicenseServer.Models.API
{
	public class Result()
	{
		public class Fail() 
		{
			public string Status { get; set; } = "Fail";
			public string Data { get; set; } = "Ошибка";
		}
		public class Success<T>() 
		{
			public string Status { get; set; } = "Success";
			public T Data { get; set; }
		}
	}
}
