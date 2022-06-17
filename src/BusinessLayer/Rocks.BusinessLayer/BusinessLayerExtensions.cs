using Prism.Ioc;
using Rocks.DataLayer;

namespace Rocks.BusinessLayer
{
    public static class BusinessLayerExtensions
    {
        public static void AddBusinessLayer(this IContainerRegistry container)
        {
            container.AddDataLayer();


        }
    }
}
