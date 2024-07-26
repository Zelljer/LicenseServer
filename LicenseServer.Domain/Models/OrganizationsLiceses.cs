using LicenseServer.Database.Entity;

namespace LicenseServer.Domain.Models
{
    public class OrganizationsLiceses
    {
        public OrganizationEntity Organization { get; set; }
        public List<LicenseAPI.LicenseResponse> Licenses { get; set; } = new List<LicenseAPI.LicenseResponse>();
    }
}
