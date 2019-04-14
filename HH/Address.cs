using Newtonsoft.Json;

namespace HH
{
    public partial class Address
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lng")]
        public string Lng { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("metro")]
        public Metro Metro { get; set; }

        [JsonProperty("metro_stations")]
        public Metro[] MetroStations { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}


