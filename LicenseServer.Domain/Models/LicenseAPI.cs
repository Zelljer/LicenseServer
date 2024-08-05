using Newtonsoft.Json;

namespace LicenseServer.Domain.Models
{
	public class LicenseAPI
    {
        public class LicenseResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("organizationId")]
            public int OrganizationId { get; set; }
            [JsonProperty("tarifId")]
            public int TarifId { get; set; }
            [JsonProperty("dateCreated")]
            public DateTime DateCreated { get; set; }
            [JsonProperty("startDate")]
            public DateTime StartDate { get; set; }
            [JsonProperty("endDate")]
            public DateTime EndDate { get; set; }
        }

        public class LicenseRequest
        {
            [JsonProperty("organizationId")]
            public int OrganizationId { get; set; }
            [JsonProperty("tarifId")]
            public int TarifId { get; set; }
            [JsonProperty("dateStart")]
            public string DateStart { get; set; }
        }

    }
}
