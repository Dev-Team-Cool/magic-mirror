import cv2
import urllib.request
from PIL import Image
from detecto.core import DataLoader, Model
import numpy as np


while True:
    cap = cv2.VideoCapture('http://admin:admin@10.42.9.30/tmpfs/auto.jpg')

    ret, frame = cap.read()

    if ret == True:
        labels = ['Badge', 'ML6 logo']
        model = Model.load('badgeDetectionV2.1.pth', labels)
        predictions = model.predict(frame)
        if predictions != "":
            for label, box, score in zip(*predictions):
                if score > 0.6:
                    if label == "Badge":
                        print(label + "" +str(round(score.item(), 2)))


cap.release()
cv2.destroyAllWindows()