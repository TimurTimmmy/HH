using Newtonsoft.Json;

namespace HH
{
    public partial class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("premium")]
        public bool Premium { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("department")]
        public Department Department { get; set; }

        [JsonProperty("has_test")]
        public bool HasTest { get; set; }

        [JsonProperty("response_letter_required")]
        public bool ResponseLetterRequired { get; set; }

        [JsonProperty("area")]
        public Area Area { get; set; }

        [JsonProperty("salary")]
        public Salary Salary { get; set; }

        [JsonProperty("type")]
        public TypeClass Type { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("response_url")]
        public string ResponseUrl { get; set; }

        [JsonProperty("sort_point_distance")]
        public string SortPointDistance { get; set; }

        [JsonProperty("employer")]
        public Employer Employer { get; set; }

        [JsonProperty("published_at")]
        public string PublishedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("apply_alternate_url")]
        public string ApplyAlternateUrl { get; set; }

        [JsonProperty("insider_interview")]
        public string InsiderInterview { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("alternate_url")]
        public string AlternateUrl { get; set; }

        [JsonProperty("relations")]
        public string[] Relations { get; set; }

        [JsonProperty("snippet")]
        public Snippet Snippet { get; set; }

        [JsonProperty("contacts")]
        public Contacts Contacts { get; set; }
    }
}


