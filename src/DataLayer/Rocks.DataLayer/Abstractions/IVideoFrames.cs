using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace Rocks.DataLayer.Abstractions
{
    public interface IVideoFrames : IDisposable
    {
        IEnumerable<Mat> GetFrames();
    }
}
