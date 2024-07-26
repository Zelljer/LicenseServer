namespace LicenseServer.Domain.Models
{
    public interface IHTTPResult 
	{
		string Status { get; }
	}

	public class Fail(): IHTTPResult
	{
		public string Status => "Fail";
		public List<string> Data { get; set; } = [];
	}

	public class Success<T>() : IHTTPResult
	{
		public string Status => "Success";
		public T Data { get; set; }

	}
}
