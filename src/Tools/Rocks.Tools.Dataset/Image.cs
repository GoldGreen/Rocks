using Newtonsoft.Json;

namespace Rocks.Tools.Dataset
{
    public class Image
        {
            [JsonProperty("file_name")]
            public string FileName { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }
        }
}
