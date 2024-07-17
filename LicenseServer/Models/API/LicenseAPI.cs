using LicenseServer.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.API
{
	public class LicenseAPI
	{
		[Required]
		public virtual OrganizationAPI Organization { get; set; }
		[Required]
		public virtual TarifAPI Tarif { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
	}
}
