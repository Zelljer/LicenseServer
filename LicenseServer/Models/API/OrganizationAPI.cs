using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.API
{
	public class OrganizationAPI
	{
		public class OrganizationResponse
		{
			[Required]
			public int Id { get; set; }
			[Required]
			public string Inn { get; set; }
			public string? Kpp { get; set; }
			[Required, DataType(DataType.EmailAddress)]
			public string Email { get; set; }
			[Required]
			public string Phone { get; set; }
		}
		public class OrganizationRequest
		{
			[Required]
			public required string Inn { get; set; }
			public string? Kpp { get; set; }
			[Required, DataType(DataType.EmailAddress)]
			public string Email { get; set; }
			[Required]
			public string Phone { get; set; }
		}
	}
}
