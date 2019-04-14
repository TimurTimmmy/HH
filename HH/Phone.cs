using Newtonsoft.Json;

namespace HH
{
    public partial class Phone
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}


