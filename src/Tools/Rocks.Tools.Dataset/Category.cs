using Newtonsoft.Json;

namespace Rocks.Tools.Dataset
{
    public class Category
    {
        [JsonProperty("supercategory")]
        public string Supercategory { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
