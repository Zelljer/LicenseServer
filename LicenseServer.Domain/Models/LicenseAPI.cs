namespace LicenseServer.Domain.Models
{
	public class LicenseAPI
    {
        public class LicenseResponse
        {
            public int Id { get; set; }
            public int OrganizationId { get; set; }
            public int TarifId { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class LicenseRequest
        {
            public int OrganizationId { get; set; }
            public int TarifId { get; set; }
            public string DateStart { get; set; }
        }

    }
}
