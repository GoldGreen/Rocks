using System.IO;
using System.Linq;

namespace Rocks.Tools.Dataset
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"C:\Users\Admin\Desktop\Rocks\dataset";

            string imageRoot = Path.Combine(root, "Images");
            string[] annotationsPaths = new string[]
            {
                //"9_coco_imglab.json",
                //"16_coco_imglab.json",
                "21_coco_imglab.json",
                "annotations.json",
                "cam37.json"
            };
            string output = Path.Combine(root, "annotation.json");
            annotationsPaths = annotationsPaths.Select(x => Path.Combine(root, x)).ToArray();

            string backgroundRoot = Path.Combine(root, "backgrounds");

            string trainPath = Path.Combine(root, "train");
            string trainAnnotation = Path.Combine(root, "train_annotation.json");

            string testPath = Path.Combine(root, "test");
            string testAnnotation = Path.Combine(root, "test_annotation.json");


            foreach (string path in annotationsPaths)
            {
                DatasetExtensions.PrepProcess(path, path);
            }

            DatasetExtensions.Concat(annotationsPaths, output);
            DatasetExtensions.AddBackground(backgroundRoot, imageRoot, output, output, 1, 0.9, 0.38);
            DatasetExtensions.CheckImages(imageRoot, output);
            DatasetExtensions.Split(imageRoot, output, testAnnotation, trainAnnotation, testPath, trainPath, 0.99);
        }
    }
}
