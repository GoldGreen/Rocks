using Newtonsoft.Json;

namespace Rocks.Shared.Dto
{
    public class PointDto
    {
        [JsonProperty("x")]
        public double X { get; set; }
        
        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
