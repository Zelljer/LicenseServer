using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.API
{
	public class OrganizationAPI
	{
		[Required]
		public required string Inn { get; set; }
		[Required]
		public string Kpp { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Phone { get; set; }
	}
}
