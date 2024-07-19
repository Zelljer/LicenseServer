using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.Database
{
    public class Organization : BaseInformation
    {
        [Required]
        public required string Inn { get; set; }
		public string Kpp { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Phone { get; set; }
    }
}
