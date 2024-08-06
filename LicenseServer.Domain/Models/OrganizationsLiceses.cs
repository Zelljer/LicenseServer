using LicenseServer.Database.Entity;
using Newtonsoft.Json;

namespace LicenseServer.Domain.Models
{
    public class OrganizationsLiceses
    {
		[JsonProperty("organization")]
		public OrganizationEntity Organization { get; set; }
		[JsonProperty("licenses")]
		public List<LicenseAPI.LicenseResponse> Licenses { get; set; } = new List<LicenseAPI.LicenseResponse>();
    }
}
