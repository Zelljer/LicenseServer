using LicenseServer.Database.Dependencies;

namespace LicenseServer.Database.Entity
{
	public class UserEntity : BaseEntity
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Patronymic { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public virtual RoleType Role { get; set; }
	}
}
