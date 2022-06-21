using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rocks.Shared.Dto
{
    [ToString]
    public class DetectioinResultDto
    {
        [JsonProperty("detections")]
        public List<DetectionDto> Detections { get; set; }
    }
}
