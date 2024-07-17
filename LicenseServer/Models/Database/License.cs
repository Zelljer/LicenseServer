using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.Database
{
    public class License : BaseInformation
    {
		[Required]
		public virtual Organization Organization { get; set; }
		[Required]
		public virtual Tarif Tarif { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
    }
}
