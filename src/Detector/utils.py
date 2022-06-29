from detectron2.data import DatasetCatalog, MetadataCatalog
from detectron2.utils.visualizer import Visualizer
from detectron2.utils.visualizer import GenericMask
from detectron2.config import get_cfg
from detectron2 import model_zoo

from detectron2.utils.visualizer import ColorMode
import random
import cv2
import json
import matplotlib.pyplot as plt
import numpy as np
from torch import Tensor


class PointDto:
    def __init__(self, x, y):
        self.x = x
        self.y = y


class DetectionDto(object):
    def __init__(self, prediction_class, score, bbox, polygon, area, mm_size, size_group):
        self.prediction_class = prediction_class
        self.score = score
        self.bbox = bbox
        self.polygon = polygon
        self.area = area
        self.mm_size = mm_size
        self.size_group = size_group


class DetectionResultDto(object):
    def __init__(self, detections):
        self.detections = detections


class DetectionResultWithMatDto(object):
    def __init__(self, detection_result, mat):
        self.detection_result = detection_result
        self.mat = mat

    @classmethod
    def deserialize(cls, j):
        cls.__dict__ = json.loads(j)


def get_result(predictions, height, width):
    bboxes = get_bboxes(predictions.pred_boxes)
    classes = get_classes(predictions.pred_classes)
    scores = get_scores(predictions.scores)
    polygons = get_polygones(predictions.pred_masks,
                             height, width)

    detections = [DetectionDto(cls, score, bbox, polygon, 0, 0, 0)
                  for (cls, score, bbox, polygon)
                  in zip(classes, scores, bboxes, polygons)]

    detectonResult = DetectionResultDto(detections)
    return detectonResult


def get_stats(polygones):
    pass


def get_bboxes(pred_bbox):
    return list(
        map(lambda x: list(map(lambda z: z.item(), x)), pred_bbox.tensor.detach().numpy()))


def cocoToPoints(coco_points):
    return [PointDto(x, y) for (x, y) in zip(
            [val for i, val in enumerate(coco_points) if i % 2 == 0],
            [val for i, val in enumerate(coco_points) if i % 2 != 0]
            )]


def get_polygones(pred_masks, height, width):
    masks = np.asarray(pred_masks)
    masks = [GenericMask(x, height,
                         width) for x in masks]

    lst = list(
        map(lambda x: list(map(lambda z: cocoToPoints(list(map(lambda p: p.item(), z))), x.polygons)), masks))

    return lst


def get_classes(pred_classes):
    return pred_classes.tolist()


def get_scores(pred_scores):
    return pred_scores.tolist()


def plot_samples(dataset_name, n=1):
    dataset = DatasetCatalog.get(dataset_name)
    dataset_metadata = MetadataCatalog.get(dataset_name)

    for s in random.sample(dataset, n):
        img = cv2.imread(s["file_name"])
        v = Visualizer(img[:, :, ::-1], metadata=dataset_metadata, scale=0.5)
        v = v.draw_dataset_dict(s)
        plt.figure(figsize=(15, 20))
        plt.imshow(v.get_image())
        plt.show()


def get_train_cfg(config_file_path, checkpoint_url, train_dataset_name, test_dataset_name, num_classes, device, output_dir):
    cfg = get_cfg()
    cfg.merge_from_file(model_zoo.get_config_file(config_file_path))

    cfg.MODEL.WEIGHTS = model_zoo.get_checkpoint_url(checkpoint_url)
    cfg.DATASETS.TRAIN = (train_dataset_name,)
    cfg.DATASETS.TEST = (test_dataset_name,)

    cfg.MODEL.ROI_HEADS.NUM_CLASSES = num_classes
    cfg.MODEL.DEVICE = device

    cfg.OUTPUT_DIR = output_dir

    cfg.DATALOADER.NUM_WORKERS = 4
    cfg.SOLVER.MAX_ITER = 60000
    cfg.SOLVER.IMS_PER_BATCH = 1
    cfg.SOLVER.BASE_LR = 0.0001

    cfg.SOLVER.CHECKPOINT_PERIOD = 5000
    cfg.SOLVER.STEPS = (30000,)

    # cfg.SOLVER.MOMENTUM = 0.9

    # cfg.SOLVER.WEIGHT_DECAY = 0.0001
    # cfg.SOLVER.WEIGHT_DECAY_NORM = 0.0

    # cfg.SOLVER.GAMMA = 0.1

    # cfg.SOLVER.WARMUP_FACTOR = 1.0 / 1000
    # cfg.SOLVER.WARMUP_ITERS = 1000
    # cfg.SOLVER.WARMUP_METHOD = "linear"

    #cfg.TEST.EVAL_PERIOD = 70

    return cfg


def on_image(image_path, predictor):
    img = cv2.imread(image_path)
    outputs = predictor(img)
    predictions = outputs["instances"].to("cpu")

    v = Visualizer(img[:, :, ::-1], {}, 0.5, ColorMode.SEGMENTATION)
    v = v.draw_instance_predictions(
        outputs["instances"].to("cpu"))
    plt.figure(figsize=(14, 10))
    plt.imshow(v.get_image())
    plt.show()
