using System.Collections.Generic;
using Newtonsoft.Json;

namespace HH
{
    public partial class Vac 
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("found")]
        public int Found { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("clusters")]
        public string Clusters { get; set; }

        [JsonProperty("arguments")]
        public string Arguments { get; set; }

        [JsonProperty("alternate_url")]
        public string AlternateUrl { get; set; }
    }
}