using System;

namespace Rocks.PresentationLayer.Shared.Views
{
    public static class GlobalRegions
    {
        private static string Key { get; } = Guid.NewGuid().ToString();
        public static string Detection { get; } = $"{Key} Detection";
    }
}
