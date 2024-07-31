using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Domain.Models
{
    public class OrganizationAPI
    {
        public class OrganizationResponse
        {
            public int Id { get; set; }
            public string Inn { get; set; }
            public string? Kpp { get; set; } 
			[DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            public string Phone { get; set; }
        }
        public class OrganizationRequest
        {
            public required string Inn { get; set; }
            public string? Kpp { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}
