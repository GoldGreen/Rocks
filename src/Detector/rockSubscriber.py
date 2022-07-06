import base64
import pickle
from charset_normalizer import detect
import cv2
import numpy as np
import zmq
import codecs
import json

context = zmq.Context()

with context.socket(zmq.SUB) as socket:
    socket.connect('tcp://127.0.0.1:7777')
    socket.setsockopt(zmq.SUBSCRIBE, b'')

    while True:
        message = socket.recv_json()

        jpg = np.frombuffer(base64.b64decode(message["image"]), dtype=np.uint8)
        mat = cv2.imdecode(jpg, cv2.IMREAD_COLOR)
        mat = cv2.resize(mat, (640, 480))
        cv2.imshow("image", mat)
        cv2.waitKey(15)
