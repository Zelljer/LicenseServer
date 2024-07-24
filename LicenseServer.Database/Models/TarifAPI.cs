using System.ComponentModel.DataAnnotations;
using LicenseServer.Models;

namespace LicenseServer.Database.Models
{
    public class TarifAPI
    {
        public class TarifResponse
        {
            [Required]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Program { get; set; }
            [Required]
            public long Price { get; set; }
            [Required]
            public int DaysCount { get; set; }
        }
        public class TarifRequest
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public ProgramType Program { get; set; }
            [Required]
            public long Price { get; set; }
            [Required]
            public int DaysCount { get; set; }
        }
    }
}
