using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.Database
{
    public class Tarif : BaseInformation
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ProgramType Program { get; set; }
        [Required]
        public long Price { get; set; }
    }
}
