using Newtonsoft.Json;

namespace LicenseServer.Domain.Models
{
    public class TestResult<T>
	{
        [JsonProperty("isSuccsess")]
        public bool IsSuccsess { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
	}
}
