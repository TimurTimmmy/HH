using Newtonsoft.Json;

namespace HH
{
    public partial class Salary
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("gross")]
        public bool Gross { get; set; }
    }
}


