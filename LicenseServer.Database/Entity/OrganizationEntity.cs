namespace LicenseServer.Database.Entity
{
	public class OrganizationEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
