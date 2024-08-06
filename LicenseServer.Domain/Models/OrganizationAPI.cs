using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Domain.Models
{
    public class OrganizationAPI
    {
        public class OrganizationResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("inn")]
            public string Inn { get; set; }
            [JsonProperty("kpp")]
            public string? Kpp { get; set; }
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("phone")]
            public string Phone { get; set; }
        }
        public class OrganizationRequest
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("inn")]
            public string Inn { get; set; }
            [JsonProperty("kpp")]
            public string? Kpp { get; set; }
            [JsonProperty("email")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [JsonProperty("phone")]
            public string Phone { get; set; }
        }
    }
}
