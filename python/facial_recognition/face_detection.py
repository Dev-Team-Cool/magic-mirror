import os
import threading
import time

import cv2
from detecto.core import DataLoader, Model
from facenet_pytorch import MTCNN, extract_face, fixed_image_standardization

from facial_recognition.facial_recognition import FacialRecognition
from facial_recognition.config import Config

class FaceDetection(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.cap = cv2.VideoCapture(0)    # Camera object
        self.__allow_new_thread = True
        self.__debug = True if os.getenv('DEBUG', 'false') == 'true' else False

    def init(self):
        self.detector = MTCNN(keep_all=False) #detection classifier
        self.recognizer = FacialRecognition().load()
        self.badgeDetector = Model.load(Config.get('badge_model_path'), ['Badge', 'ML6 logo'])

        return self

    def run(self):
        """
        Main thread: frame rate sets the amount of times the detect and convert function will be called per second.
        """
        frame_rate = 30 if self.__debug else 2
        prev = 0
        print('Started looking...')
        while True:
            time_elapsed = time.time() - prev
            ret, frame = self.cap.read()
            if frame is None:
                print('--(!) No captured frame -- Break!')
                break
            if time_elapsed > 1./frame_rate:
                prev = time.time()
                # Detect faces and badges
                self.detectAndConvert(frame)
                if self.__debug:
                    # Show a live feed
                    cv2.imshow('Webcam', frame)
            if cv2.waitKey(10) == 27:
                break

    def detectAndConvert(self, frame):
        """
        Function that handles the actual face detection. Detected faces are converted to tensors. 
        Amount of detected faces can be found with len(self.detected_person)
        """
        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        boxes, probas = self.detector.detect(image) # Get detected faces

        if boxes is not None:
            for box in boxes:
                face = extract_face(frame, box)
                prediction = self.recognizer.predict(fixed_image_standardization(face)) # Do a prediction on a standardized image tensor
                
                # Print prediction to console, we read this in the MagicMirror module
                return_prediction(prediction)

                if prediction[0] != "Unknown" and self.__allow_new_thread:
                    self.__allow_new_thread = False
                    # Start a seperate thread for the badge detection
                    threading.Thread(target=self.detectAndConvertBadge, args=[frame]).start()

                if self.__debug:
                    frame = cv2.rectangle(frame, (box[0],box[1]), (box[2], box[3]), (255,0,0)) # Draw a rectangle arround the face
                    cv2.putText(frame, f'{prediction[0]}', (int(box[0]), int(box[1] - 10)), cv2.FONT_HERSHEY_COMPLEX, 1, (200, 0, 0))
        else:
            return_prediction(('no user', 100))

    def detectAndConvertBadge(self, frame):
        try:
            predictions = self.badgeDetector.predict(frame)
        except:
            pass

        if predictions != "":
            for label, box, score in zip(*predictions):
                if score > 0.6:
                    if label == "Badge":
                        return_prediction(("badge", score))
        
        self.__allow_new_thread = True
    
def return_prediction(prediction):
    import json
    print(json.dumps({
        "detected": prediction[0],
        "probability": prediction[1]
    }))
