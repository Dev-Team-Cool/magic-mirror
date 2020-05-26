import cv2
import threading
import time
from facenet_pytorch import MTCNN, extract_face, fixed_image_standardization
from facial_recognition import FacialRecognition

class FaceDetection(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.detector = MTCNN(keep_all=True) #detection classifier
        self.recognizer = FacialRecognition()
        self.recognizer.train_classifier('data/trainV2')
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
        frame_rate = 30
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
                self.detectAndConvert(frame)
                cv2.imshow('Webcam', frame)
            if cv2.waitKey(10) == 27:
                break

    def detectAndConvert(self, frame):
        """
        Function that handles the actual face detection. Detected faces are converted to tensors. 
        Amount of detected faces can be found with len(self.detected_person)
        """
        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        boxes, probas = self.detector.detect(image)
        if boxes is not None:
            for box in boxes:
                test = extract_face(frame, box)
                frame = cv2.rectangle(frame, (box[0],box[1]), (box[2], box[3]), (255,0,0))
                prediction = self.recognizer.predict(fixed_image_standardization(test))
                print(box, prediction)
                cv2.putText(frame, prediction[0], (int(box[0]), int(box[1] - 10)), cv2.FONT_HERSHEY_COMPLEX, 1, (200, 0, 0))
                # cv2.putText(frame, prediction[0], (box[0], box[1] - 10), cv2.FONT_HERSHEY_COMPLEX, 0.9, (255, 0, 0))



