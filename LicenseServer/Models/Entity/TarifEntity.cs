using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Models.Database
{
    public class TarifEntity : BaseEntity
    {
        public string Name { get; set; }
        public ProgramType Program { get; set; }
        public long Price { get; set; }
		public int DaysCount { get; set; }
	}
}
