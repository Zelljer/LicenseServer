using LicenseServer.Models.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.API
{
	public class LicenseAPI
	{
		public class LicenseResponse
		{
			[Required]
			public int Id { get; set; }
			[Required]
			public int OrganizationId { get; set; }
			[Required]
			public int TarifId { get; set; }
			[Required]
			public DateTime DateCreated { get; set; }
			[Required]
			public DateTime StartDate { get; set; }
			[Required]
			public DateTime EndDate { get; set; }
		}
		public class LicenseRequest
		{
			[Required]
			public int OrganizationId { get; set; }
			[Required]
			public int TarifId { get; set; }
			[Required]
			public string DateStart { get; set; } 
		}

	}
}
