using LicenseServer.Database.Dependencies;

namespace LicenseServer.Domain.Models
{
	public class UserAPI
	{
		public class UserResponse 
		{
			public int Id { get; set; }
			public string Login { get; set; }
			public string Password { get; set; }
			public string Name { get; set; }
			public string Surname { get; set; }
			public string Patronymic { get; set; }
			public string Role { get; set; }
			public string Token { get; set; }
		}
		public class UserRegistrationRequest
		{
			public string Login { get; set; }
			public string Password { get; set; }
			public string Name { get; set; }
			public string Surname { get; set; }
			public string Patronymic { get; set; }
			public RoleType Role { get; set; }
		}
		public class UserAuthentificationRequest 
		{
			public string Login { get; set; }
			public string Password { get; set; }
		}
	}
}
