using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rocks.Tools.Dataset
{
    public class Root
        {
            [JsonProperty("images")]
            public List<Image> Images { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("annotations")]
            public List<Annotation> Annotations { get; set; }

            [JsonProperty("categories")]
            public List<Category> Categories { get; set; }
        }
}
