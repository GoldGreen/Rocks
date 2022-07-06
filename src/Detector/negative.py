import random
import cv2
from detectron2.data.transforms.augmentation import Augmentation
from detectron2.data.transforms.transform import Transform, NoOpTransform
import numpy as np
import glob
import pycocotools.mask as mask_util
import itertools


class Negative(Augmentation):
    def __init__(self):
        super().__init__()
        self._init(locals())

    def get_transform(self, image):
        return NegativeTransform()


class NegativeTransform(Transform):
    def __init__(self):
        super().__init__()
        self._set_attributes(locals())

    def apply_image(self, img: np.ndarray, interp: str = None) -> np.ndarray:
        return cv2.bitwise_not(img)

    def apply_coords(self, coords: np.ndarray) -> np.ndarray:
        return coords

    def apply_segmentation(self, segmentation: np.ndarray) -> np.ndarray:
        return segmentation

    def inverse(self) -> Transform:
        return NoOpTransform()
