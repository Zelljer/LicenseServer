using LicenseServer.Database.Dependencies;

namespace LicenseServer.Domain.Models
{
	public class TarifAPI
    {
        public class TarifResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Program { get; set; }
            public long Price { get; set; }
            public int DaysCount { get; set; }
        }
        public class TarifRequest
        {
            public string Name { get; set; }
            public ProgramType Program { get; set; }
            public long Price { get; set; }
            public int DaysCount { get; set; }
        }
    }
}
