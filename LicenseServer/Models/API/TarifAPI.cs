using LicenseServer.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.API
{
	public class TarifAPI
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public ProgramType Program { get; set; }
		[Required]
		public long Price { get; set; }
	}
}
