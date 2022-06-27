using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rocks.Tools.Dataset
{
    public class DatasetExtensions
    {
        /// <summary>
        /// Исправление датасета (только для задачи камней)
        /// </summary>
        /// <param name="inputPath">входной путь</param>
        /// <param name="outputPath">выходной путь</param>
        public static void PrepProcess(string inputPath, string outputPath)
        {
            var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(inputPath));

            var gr = root.Images.GroupJoin(root.Annotations, x => x.Id, x => x.ImageId, (i, a) => (i, a: a.ToList()))
                                .ToList();

            foreach (var g in gr)
            {
                if (g.a.Count == 0)
                {
                    root.Images.Remove(g.i);
                }
            }

            while (root.Categories.Count > 1)
            {
                root.Categories.RemoveAt(0);
            }

            root.Categories[0].Name = "Rock";
            root.Categories[0].Id = 1;

            foreach (var img in root.Images)
            {
                img.FileName = Path.GetFileName(img.FileName);
            }

            foreach (var ann in root.Annotations)
            {
                var imgs = root.Images.Where(x => x.Id == ann.ImageId);

                if (imgs.Count() > 1)
                {
                    throw new Exception("opa");
                }

                var img = imgs.First();

                foreach (var segm in ann.Segmentation)
                {
                    for (int i = 0; i < segm.Count; i++)
                    {
                        if (segm[i] < 0)
                        {
                            segm[i] = 0;
                        }

                        if (i % 2 == 0)
                        {
                            if (segm[i] >= img.Width)
                            {
                                segm[i] = img.Width - 1;
                            }
                        }
                        else
                        {
                            if (segm[i] >= img.Height)
                            {
                                segm[i] = img.Height - 1;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < root.Annotations.Count; i++)
            {
                root.Annotations[i].Id = i + 1;
                root.Annotations[i].CategoryId = 1;

                var segmentation = root.Annotations[i].Segmentation;

                var x = segmentation.SelectMany(x => x)
                            .Where((_, i) => i % 2 == 0)
                            .ToList();

                var y = segmentation.SelectMany(x => x)
                            .Where((_, i) => i % 2 != 0)
                            .ToList();


                root.Annotations[i].Bbox = new()
                {
                    x.Min(),
                    y.Min(),
                    x.Max() - x.Min(),
                    y.Max() - y.Min()
                };
            }

            Debug.Assert(root.Images.All(x => root.Annotations.Any(a => a.ImageId == x.Id)));
            File.WriteAllText(outputPath, JsonConvert.SerializeObject(root));
        }

        /// <summary>
        /// Соединение датасетов
        /// </summary>
        /// <param name="paths">входные пути</param>
        /// <param name="outputPath">выходной путь</param>
        public static void Concat(string[] paths, string outputPath)
        {
            var first = JsonConvert.DeserializeObject<Root>(File.ReadAllText(paths[0]));
            var addable = paths.Skip(1)
                           .Select(path => JsonConvert.DeserializeObject<Root>(File.ReadAllText(path))).ToArray();

            int imageId = first.Images.Max(x => x.Id) + 1;
            int annotationId = first.Annotations.Max(x => x.Id) + 1;

            foreach (var root in addable)
            {
                Debug.Assert(root.Images.All(x => root.Annotations.Any(a => a.ImageId == x.Id)));

                var joined = root.Images.GroupJoin(root.Annotations, x => x.Id, x => x.ImageId, (image, ann) => (image, ann: ann.ToList())).ToList();

                foreach (var j in joined)
                {
                    var image = j.image;
                    int oldId = image.Id;
                    image.Id = imageId++;

                    foreach (var ann in j.ann)
                    {
                        ann.Id = annotationId++;
                        ann.ImageId = image.Id;
                    }
                }

                first.Images.AddRange(root.Images);
                first.Annotations.AddRange(root.Annotations);
            }

            var res = first.Images.Where(x => !first.Annotations.Any(a => a.ImageId == x.Id)).ToList();

            Debug.Assert(first.Images.All(x => first.Annotations.Any(a => a.ImageId == x.Id)));
            File.WriteAllText(outputPath, JsonConvert.SerializeObject(first));
        }

        private static T Copy<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Добавление заднего фона
        /// </summary>
        /// <param name="backgroundsFolder">путь до папки с изображениями задних фонов</param>
        /// <param name="imagesFodler">путь до папки с изображениями входного датасета</param>
        /// <param name="inputPath">входной путь датасета</param>
        /// <param name="outputPath">выходной путь датасета</param>
        /// <param name="applysToOneImage">количество применений для одного изображения</param>
        /// <param name="chanceTo1Obj">шанс добавления одного объекта [0,1]</param>
        /// <param name="chanceToApply">шанс применения для одной картинки</param>
        public static void AddBackground(string backgroundsFolder, string imagesFodler, string inputPath, string outputPath, int applysToOneImage = 1, double chanceTo1Obj = 1, double chanceToApply = 0.5)
        {
            var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(inputPath));
            Random rnd = new(111);

            var backgrounds = Directory.GetFiles(backgroundsFolder);

            int imgId = root.Images.Max(x => x.Id) + 1;
            int annotationId = root.Annotations.Max(x => x.Id) + 1;

            foreach (var image in root.Images.ToList())
            {
                if (rnd.NextDouble() > chanceToApply)
                {
                    continue;
                }

                using Mat img = new(Path.Combine(imagesFodler, image.FileName));

                foreach (var i in Enumerable.Range(0, applysToOneImage))
                {
                    var newImg = Copy(image);
                    var newAnnotations = Copy(root.Annotations.Where(x => x.ImageId == newImg.Id).ToList())
                                            .Where(_ => rnd.NextDouble() <= chanceTo1Obj)
                                            .ToList();

                    if (newAnnotations.Count == 0)
                    {
                        continue;
                    }

                    newImg.FileName = $"(BACKGROUND_{i})_{image.FileName}";
                    newImg.Id = imgId++;

                    foreach (var ann in newAnnotations)
                    {
                        ann.Id = annotationId++;
                        ann.ImageId = newImg.Id;
                    }

                    using var background = new Mat(backgrounds[rnd.Next(0, backgrounds.Length)])
                                              .Resize(new Size(img.Width, img.Height));

                    var points = newAnnotations.Select(x => x.Segmentation)
                                               .SelectMany(x => x.Select(z =>
                                               {
                                                   var x = z.Where((_, i) => i % 2 == 0)
                                                        .ToList();

                                                   var y = z.Where((_, i) => i % 2 != 0)
                                                        .ToList();

                                                   return x.Zip(y, (x, y) => new Point(x, y)).ToList();
                                               })).ToList();

                    using var mask = background.EmptyClone();
                    Cv2.DrawContours(mask, points, -1, Scalar.White, -1);
                    Cv2.Threshold(mask, mask, 254, 255, ThresholdTypes.Binary);

                    using Mat invMask = new();
                    Cv2.Threshold(mask, invMask, 254, 255, ThresholdTypes.BinaryInv);

                    using Mat backM = new();
                    using Mat imgM = new();

                    Cv2.CopyTo(background, backM, invMask);
                    Cv2.CopyTo(img, imgM, mask);

                    using var res = (backM + imgM).ToMat();

                    root.Annotations.AddRange(newAnnotations);
                    root.Images.Add(newImg);

                    Cv2.ImWrite(Path.Combine(imagesFodler, newImg.FileName), res);
                }
            }

            File.WriteAllText(outputPath, JsonConvert.SerializeObject(root));
        }

        /// <summary>
        /// Разделение выборки
        /// </summary>
        /// <param name="inputImagesFolder">путь до папки изображений входного датасета</param>
        /// <param name="inputPath">входной путь датасета</param>
        /// <param name="testPath">путь до тестового датасета</param>
        /// <param name="trainPath">путь до тренировочного датасета</param>
        /// <param name="testImagesFolder">путь до папки изображений тестового датасета</param>
        /// <param name="trainImagesFolder">путь до папки изображений тренировочного датасета</param>
        public static void Split(string inputImagesFolder, string inputPath, string testPath, string trainPath, string testImagesFolder, string trainImagesFolder)
        {
            var train = JsonConvert.DeserializeObject<Root>(File.ReadAllText(inputPath));
            var test = JsonConvert.DeserializeObject<Root>(File.ReadAllText(inputPath));

            test.Images = new();
            test.Annotations = new();

            Random rnd = new(111);
            var testImg = train.Images.Where(_ => rnd.NextDouble() >= 0.8).ToList();

            foreach (var img in testImg)
            {
                test.Images.Add(img);
                test.Annotations.AddRange(train.Annotations.Where(x => x.ImageId == img.Id));

                train.Images.Remove(img);
                train.Annotations.RemoveAll(x => x.ImageId == img.Id);
            }

            var res = train.Annotations.GroupBy(x => x.Id).Where(x => x.Count() > 0);

            foreach (var image in test.Images)
            {
                File.Move(Path.Combine(inputImagesFolder, image.FileName), Path.Combine(testImagesFolder, image.FileName));
            }

            foreach (var image in train.Images)
            {
                File.Move(Path.Combine(inputImagesFolder, image.FileName), Path.Combine(trainImagesFolder, image.FileName));
            }

            File.WriteAllText(testPath, JsonConvert.SerializeObject(test));
            File.WriteAllText(trainPath, JsonConvert.SerializeObject(train));
        }

        /// <summary>
        /// Проверка датасет
        /// </summary>
        /// <param name="imageFolder">путь до папки изображений входного датасета</param>
        /// <param name="input">путь до входного датасета</param>
        public static void CheckImages(string imageFolder, string input)
        {
            var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(input));

            Console.WriteLine($"Каждое изображение в папке описано в аннотации {Directory.GetFiles(imageFolder).Select(x => Path.GetFileName(x)).All(x => root.Images.Any(i => i.FileName == x))}");
            Console.WriteLine($"Каждое изображение существует {root.Images.All(x => File.Exists(Path.Combine(imageFolder, x.FileName)))}");
            Console.WriteLine($"Каждое изображение имеет аннотацию {root.Images.All(x => root.Annotations.Where(a => a.ImageId == x.Id).Any())}");
            Console.WriteLine($"Каждая аннотация имеет изображение {root.Annotations.All(x => root.Images.Where(a => a.Id == x.ImageId).Count() == 1)}");
        }
    }
}
