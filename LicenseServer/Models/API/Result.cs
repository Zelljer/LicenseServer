namespace LicenseServer.Models.API
{
	public class Result()
	{
		public class Fail() 
		{
			public string Status { get; set; } = "Fail";
			public List<string> Data { get; set; } = new List<string>();
		}
		public class Success<T>() 
		{
			public string Status { get; set; } = "Success";
			public T Data { get; set; }
		}
	}
}
