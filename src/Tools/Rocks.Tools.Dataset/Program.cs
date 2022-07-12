using System.IO;
using System.Linq;

namespace Rocks.Tools.Dataset
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"F:\Rocks\src\Detector\dataset";

            string imageRoot = Path.Combine(root, "Images");
            string[] annotationsPaths = new string[]
            {
                "annotation_0.json",
                "annotation_1.json",
                "annotation_2.json",
            };
            string output = Path.Combine(root, "annotation.json");
            annotationsPaths = annotationsPaths.Select(x => Path.Combine(root, x)).ToArray();

            string backgroundRoot = Path.Combine(root, "backgrounds");

            string trainPath = Path.Combine(root, "train");
            string trainAnnotation = Path.Combine(root, "train_annotation.json");

            string testPath = Path.Combine(root, "test");
            string testAnnotation = Path.Combine(root, "test_annotation.json");

            DatasetExtensions.CreateMasks(trainPath, trainAnnotation, @"C:\Program Files (x86)\TensorTeach\dataset", @"C:\Program Files (x86)\TensorTeach\dataset\annotation.json", 20);

            //foreach (string path in annotationsPaths)
            //{
            //    DatasetExtensions.PrepP rocess(path, path);
            //}

            //DatasetExtensions.Concat(annotationsPaths, output);
            //DatasetExtensions.AddBackground(backgroundRoot, imageRoot, output, output, 1, 0.9, 0.38);
            //DatasetExtensions.CheckImages(imageRoot, output);
            //DatasetExtensions.Split(imageRoot, output, testAnnotation, trainAnnotation, testPath, trainPath, 0.85);


            //string root = @"d:\downloads\rock_check\dmitr";

            //string imageroot = path.combine(root, "images");
            //string[] annotationspaths = new string[]
            //{
            //    "iteration_0.json",
            //    "iteration_1.json",
            //    "iteration_2.json",
            //    "iteration_3.json",
            //    "iteration_4.json"
            //};
            //string output = path.combine(root, "annotation.json");
            //annotationspaths = annotationspaths.select(x => path.combine(root, x)).toarray();

            //string backgroundroot = path.combine(root, "backgrounds");

            //string trainpath = path.combine(root, "train");
            //string trainannotation = path.combine(root, "train_annotation.json");

            //string testpath = path.combine(root, "test");
            //string testannotation = path.combine(root, "test_annotation.json");


            //foreach (string path in annotationspaths)
            //{
            //    datasetextensions.prepprocess(path, path);
            //}

            //datasetextensions.concat(annotationspaths, output);
            ////datasetextensions.addbackground(backgroundroot, imageroot, output, output, 1, 0.9, 0.38);
            //datasetextensions.checkimages(imageroot, output);
            ////datasetextensions.split(imageroot, output, testannotation, trainannotation, testpath, trainpath, 0.99);
        }
    }
}
