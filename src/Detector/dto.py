
import numpy as np
from detectron2.utils.visualizer import GenericMask


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