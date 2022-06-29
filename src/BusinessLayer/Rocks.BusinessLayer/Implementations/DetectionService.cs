using OpenCvSharp;
using Rabbit.Rpc.Abstractions;
using Rocks.BusinessLayer.Abstractions;
using Rocks.BusinessLayer.Data;
using Rocks.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Implementations
{
    internal class DetectionService : IDetectionService
    {
        public IRpcClient<Mat, DetectioinResultDto> RpcClient { get; }

        public DetectionService(IRpcClient<Mat, DetectioinResultDto> rpcClient)
        {
            RpcClient = rpcClient;
        }

        public async Task<IEnumerable<Rock>> Detect(Mat mat)
        {
            var detectionResult = await RpcClient.SendAsync(mat);

            return detectionResult.Detections
                                  .Select(Convert)
                                  .ToList();
        }

        private Rock Convert(DetectionDto detection)
        {
            var rect = Convert(detection.Bbox);
            var polygones = Convert(detection.Polygon);
            double area = CalculateArea(polygones);

            return new(detection.PredictionClass, detection.Score, area, rect, polygones);
        }

        private static Rect Convert(List<double> bbox)
        {
            int[] intBbox = bbox.Select(x => (int)x).ToArray();
            return new Rect(intBbox[0], intBbox[1], intBbox[2] - intBbox[0], intBbox[3] - intBbox[1]);
        }

        private static List<List<Point>> Convert(List<List<PointDto>> polygones)
        {
            return polygones.Select(p => p.Select(x => new Point(x.X, x.Y))
                                          .ToList())
                            .ToList();
        }

        private static double CalculateArea(List<List<Point>> points)
        {
            return points.Select(CalculateArea)
                         .Sum();
        }

        private static double CalculateArea(List<Point> points)
        {
            return Cv2.ContourArea(points);
        }
    }
}
