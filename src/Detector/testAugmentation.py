import cv2
import detectron2.data.transforms as T

from negative import Negative

transform_list = [
    T.RandomApply(T.RandomCrop(
        "relative_range", (0.6, 0.6)), prob=0.15),
    T.RandomApply(T.RandomRotation(
        angle=list(range(10, 180, 10)), sample_style="choice"), prob=0.15),
    T.Resize((850, 850)),
    T.RandomFlip(prob=0.25, horizontal=True, vertical=False),
    T.RandomFlip(prob=0.25, horizontal=False, vertical=True),
    T.RandomApply(T.RandomContrast(0.5, 1.5), prob=0.2),
    T.RandomApply(T.RandomBrightness(0.5, 1.5), prob=0.2),
]

image = cv2.imread(
    "dataset/test/Basler acA4096-11gm (40095137)_20220621_134410511_0018.bmp")
while True:
    augmented, transforms = T.apply_transform_gens(transform_list, image)
    cv2.imshow("augmentation", augmented)
    cv2.waitKey(50)
