using LicenseServer.Database.Dependencies;
using Newtonsoft.Json;

namespace LicenseServer.Domain.Models
{
	public class TarifAPI
    {
        public class TarifResponse
        {
			[JsonProperty("id")]
			public int Id { get; set; }
			[JsonProperty("name")]
			public string Name { get; set; }
			[JsonProperty("program")]
			public string Program { get; set; }
			[JsonProperty("price")]
			public long Price { get; set; }
			[JsonProperty("daysCount")]
			public int DaysCount { get; set; }
		}
        public class TarifRequest
        {
			[JsonProperty("name")]
			public string Name { get; set; }
			[JsonProperty("program")]
			public ProgramType Program { get; set; }
			[JsonProperty("price")]
			public long Price { get; set; }
			[JsonProperty("daysCount")]
			public int DaysCount { get; set; }
		}
    }
}
