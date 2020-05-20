import cv2
import sys
import threading
import time
from facenet_pytorch import MTCNN

class FaceDetection(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.detector = MTCNN(keep_all=True) #detection classifier
        self.cap = cv2.VideoCapture(0)    # Camera object
        self._detected_person = None

    @property
    def detected_person(self):
        return self._detected_person
    @detected_person.setter
    def detected_person(self, value):
        self._detected_person = value
    
    def run(self):
        """
        Main thread: frame rate sets the amount of times the detect and convert function will be called per second.
        """
        frame_rate = 2
        prev = 0
        while True:
            time_elapsed = time.time() - prev
            ret, frame = self.cap.read()
            if frame is None:
                print('--(!) No captured frame -- Break!')
                break
            if time_elapsed > 1./frame_rate:
                prev = time.time()
                self.detectAndConvert(frame)
            if cv2.waitKey(10) == 27:
                break

    def detectAndConvert(self, frame):
        """
        Function that handles the actual face detection. Detected faces are converted to tensors. 
        Amount of detected faces can be found with len(self.detected_person)
        """
        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        tensors = self.detector.forward(image)
        self._detected_person = tensors


