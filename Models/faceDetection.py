import cv2
import sys
import threading
import time
from mtcnn import MTCNN

class FaceDetection(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.detector = MTCNN()
        self.cap = cv2.VideoCapture(0)    # Camera object
    
    def run(self):
        """
        Main thread that does the actual face detection.
        """
        while True:
            ret, frame = self.cap.read()
            if frame is None:
                print('--(!) No captured frame -- Break!')
                break
            self.detectAndDisplay(frame)
            if cv2.waitKey(10) == 27:
                break

    def detectAndDisplay(self, frame):
        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        result = self.detector.detect_faces(image)
        if len(result) > 0:
            box = result[0]['box']
            if result[0]['confidence'] >= 0.9:
                print('face found')
                x1,y1,x2,y2 = box
                faceROI = image[y1:y1+y2, x1:x1+x2]
                cv2.imwrite('image.jpg', faceROI)
                time.sleep(10)

        cv2.imshow('Capture - Face detection', frame)

