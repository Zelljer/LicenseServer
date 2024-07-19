using LicenseServer.Models.Database;

namespace LicenseServer.Models.API
{
	public class OrganizationsLiceses
	{
		public Organization Organization { get; set; }
		public List<LicenseAPI.LicenseResponse> Licenses { get; set; } = new List<LicenseAPI.LicenseResponse>();
	}
}
