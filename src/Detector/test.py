from time import time
from detectron2.engine import DefaultPredictor

import os
import pickle
from utils import *


cfg_save_path = "IS_rocks_cfg.pickle"
with open(cfg_save_path, 'rb') as f:
    cfg = pickle.load(f)

cfg.MODEL.WEIGHTS = os.path.join(cfg.OUTPUT_DIR, "model_0019999.pth")
#cfg.MODEL.WEIGHTS = 'dataset/old_weights/model_final.pth'
cfg.MODEL.ROI_HEADS.SCORE_THRESH_TEST = 0.5
#cfg.TEST.DETECTIONS_PER_IMAGE = 400

predictor = DefaultPredictor(cfg)

folder = "D:/downloads/Sum/Sum/9_coco_imglab/"
images = ["Image__2022-04-25__08-18-00.jpg", "Image__2022-04-25__08-18-21.jpg"]

for image in images:
    path = folder+image
    on_image(path, predictor)
