using Newtonsoft.Json;

namespace HH
{
    public class Department
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }
}


