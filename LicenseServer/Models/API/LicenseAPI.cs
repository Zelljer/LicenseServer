using LicenseServer.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.API
{
	public class LicenseAPI
	{
		[Required]
		public int OrganizationId { get; set; }
		[Required]
		public int TarifId { get; set; }
		[Required]
		public DateTime DateStart { get; set; }

	}
}
