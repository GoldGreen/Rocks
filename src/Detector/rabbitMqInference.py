from time import time
from detectron2.engine import DefaultPredictor

import os
import json
import pickle

from utils import *

import pika
import cv2
import numpy as np
from utils import *

cfg_save_path = "IS_rocks_cfg.pickle"
with open(cfg_save_path, 'rb') as f:
    cfg = pickle.load(f)

cfg.MODEL.WEIGHTS = os.path.join(cfg.OUTPUT_DIR, "model_0019999.pth")
#cfg.MODEL.WEIGHTS = 'dataset/old_weights/model_final.pth'
cfg.MODEL.ROI_HEADS.SCORE_THRESH_TEST = 0.5

predictor = DefaultPredictor(cfg)

queueName = 'rocks'

connection = pika.BlockingConnection(
    pika.ConnectionParameters(host='localhost'))

channel = connection.channel()
channel.queue_declare(queue=queueName)


def on_request(ch, method, props, body):
    jpg = np.frombuffer(body, dtype=np.uint8)
    mat = cv2.imdecode(jpg, cv2.IMREAD_COLOR)

    outputs = predictor(mat)
    predictions = outputs["instances"].to("cpu")
    detectonResult = get_result(predictions,mat.shape[0], mat.shape[1])

    ch.basic_publish(exchange='',
                     routing_key=props.reply_to,
                     properties=pika.BasicProperties(
                         correlation_id=props.correlation_id),
                     body=json.dumps(detectonResult, default=lambda o: o.__dict__))

    ch.basic_ack(delivery_tag=method.delivery_tag)


channel.basic_qos(prefetch_count=1)
channel.basic_consume(queue=queueName, on_message_callback=on_request)

print(" [x] Awaiting RPC requests")
channel.start_consuming()
