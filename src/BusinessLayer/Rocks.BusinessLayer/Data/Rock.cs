using OpenCvSharp;
using System.Collections.Generic;

namespace Rocks.BusinessLayer.Data
{
    public record Rock(int PredictionClass, double Score, double Area, Rect Rectangle, IEnumerable<IEnumerable<Point>> Polygones);
}
