using LicenseServer.Database.Dependencies;
using Newtonsoft.Json;

namespace LicenseServer.Domain.Models
{
	public class UserAPI
	{
        public class UserResponse
        {
            public int Id { get; set; }
            [JsonProperty("login")]
            public string Login { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("surname")]
            public string Surname { get; set; }
            [JsonProperty("patronymic")]
            public string Patronymic { get; set; }
            [JsonProperty("role")]
            public string Role { get; set; }
        }

        public class UserRegistrationRequest
        {
            [JsonProperty("login")]
            public string Login { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("surname")]
            public string Surname { get; set; }

            [JsonProperty("patronymic")]
            public string Patronymic { get; set; }

            [JsonProperty("role")]
            public RoleType Role { get; set; }
        }
        public class UserAuthentificationRequest
        {
            [JsonProperty("login")]
            public string Login { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}
