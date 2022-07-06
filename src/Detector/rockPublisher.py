import base64
import glob
from itertools import combinations
from detectron2.engine import DefaultPredictor
import json
import pickle
import os
import cv2
import numpy as np
import zmq
from time import sleep
import torch
from dto import *

torch.cuda.empty_cache()
cfg_save_path = "IS_rocks_cfg.pickle"
with open(cfg_save_path, 'rb') as f:
    cfg = pickle.load(f)

cfg.MODEL.WEIGHTS = os.path.join(cfg.OUTPUT_DIR, "model_final.pth")
#cfg.MODEL.WEIGHTS = 'dataset/old_weights/model_final.pth'
cfg.MODEL.ROI_HEADS.SCORE_THRESH_TEST = 0.5
#rock_scale = 0.06025
rock_scale = 0.09018
predictor = DefaultPredictor(cfg)

context = zmq.Context()
with context.socket(zmq.PUB) as socket:
    socket.bind('tcp://127.0.0.1:7777')

    path = "dataset\\train\\*"
    for file in glob.glob(path):
        mat = cv2.imread(file)
        outputs = predictor(mat)
        predictions = outputs["instances"].to("cpu")

        detectonResult = get_result(predictions, mat.shape[0], mat.shape[1])

        for detection in detectonResult.detections:
            for pol in detection.polygon:
                contour = [[[p.x, p.y] for p in pol]]
                contour = np.array(contour).reshape((-1, 1, 2))
                cv2.drawContours(mat, [contour], -1, (0, 0, 255), 6)

        polygones = [
            detection.polygon for detection in detectonResult.detections]

        sizes = []
        for polygone in polygones:
            for pol in polygone:
                contour = np.array([np.array([p.x, p.y])
                                    for p in pol], np.int32)
                epsilon = 0.001 * cv2.arcLength(contour, True)
                approx = cv2.approxPolyDP(contour, epsilon, True)

                max_value = np.max(np.array([np.linalg.norm(a-b)
                                            for a, b in combinations(approx, 2)]))
                sizes.append(rock_scale*max_value)

        sizes = [size for size in sizes if size > 5]
        res = {
            "measure": [
                {"id": 1, "meta": "0-10 мм",
                    "value": len(list(filter(lambda x:  x < 10, sizes)))},
                {"id": 2, "meta": "10-25 мм",
                    "value": len(list(filter(lambda x:  10 <= x < 25, sizes)))},
                {"id": 3, "meta": "25-40 мм",
                    "value": len(list(filter(lambda x:  25 <= x < 40, sizes)))},
                {"id": 4, "meta": "40-60 мм",
                    "value": len(list(filter(lambda x:  40 <= x < 60, sizes)))},
                {"id": 5, "meta": "60-80 мм",
                    "value": len(list(filter(lambda x:  60 <= x < 80, sizes)))},
                {"id": 6, "meta": "80+ мм",
                    "value": len(list(filter(lambda x:  80 <= x, sizes)))},
            ],
            "total_count": len(sizes),
            "image": base64.b64encode(cv2.imencode('.jpg', mat)[1].tobytes()).decode('ascii')
        }
        socket.send_json(res)
