using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rocks.Shared.Dto
{
    [ToString]
    public class DetectionDto
    {
        [JsonProperty("prediction_class")]
        public int PredictionClass { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("polygon")]
        public List<List<double>> Polygon { get; set; }
    }
}
