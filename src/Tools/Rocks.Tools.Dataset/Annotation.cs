using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rocks.Tools.Dataset
{
    public class Annotation
    {
        [JsonProperty("segmentation")]
        public List<List<double>> Segmentation { get; set; }

        [JsonProperty("area")]
        public double Area { get; set; }

        [JsonProperty("iscrowd")]
        public int Iscrowd { get; set; }

        [JsonProperty("image_id")]
        public int ImageId { get; set; }

        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ignore")]
        public int Ignore { get; set; }
    }
}
