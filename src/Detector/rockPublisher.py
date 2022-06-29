import base64
import glob
from detectron2.engine import DefaultPredictor
import json
import pickle
import os
from utils import *
import cv2
import numpy as np
import zmq
from time import sleep

cfg_save_path = "IS_rocks_cfg.pickle"
with open(cfg_save_path, 'rb') as f:
    cfg = pickle.load(f)

cfg.MODEL.WEIGHTS = os.path.join(cfg.OUTPUT_DIR, "model_0019999.pth")
#cfg.MODEL.WEIGHTS = 'dataset/old_weights/model_final.pth'
cfg.MODEL.ROI_HEADS.SCORE_THRESH_TEST = 0.5

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

        data = cv2.imencode('.jpg', mat)[1].tobytes()
        result = DetectionResultWithMatDto(detectonResult, data)

        body = pickle.dumps(result)
        socket.send(body)
