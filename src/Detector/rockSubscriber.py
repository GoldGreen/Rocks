import pickle
from charset_normalizer import detect
import cv2
import numpy as np
import zmq
import json

context = zmq.Context()

with context.socket(zmq.SUB) as socket:
    socket.connect('tcp://127.0.0.1:7777')
    socket.setsockopt(zmq.SUBSCRIBE, b'')

    while True:
        message = socket.recv()
        res = pickle.loads(message)
        jpg = np.frombuffer(res.mat, dtype=np.uint8)
        mat = cv2.imdecode(jpg, cv2.IMREAD_COLOR)

        detections = res.detection_result.detections
        polygones = list(map(lambda detection: list(
            map(lambda pol: [[p.x, p.y] for p in pol], detection.polygon)), detections))

        for p1 in polygones:
            for p2 in p1:
                ctr = np.array(p2).reshape((-1, 1, 2))
                cv2.drawContours(mat, [ctr], -1, (0, 0, 255), 6)

        mat = cv2.resize(mat, (640, 480))
        cv2.imshow("res", mat)
        cv2.waitKey(15)
