using System.Text.Json.Serialization;

namespace api// <--- THIS MUST MATCH the namespace in GetVisitorCount.cs
{
    public class Counter
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "Counter";

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("partitionKey")]
        public string PartitionKey { get; set; } = "Counter";
    }
}
