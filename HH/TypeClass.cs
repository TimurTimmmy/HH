using Newtonsoft.Json;

namespace HH
{
    public partial class TypeClass
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }    
}


