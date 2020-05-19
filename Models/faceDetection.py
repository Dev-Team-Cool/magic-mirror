import cv2
import sys
import threading
import time

class FaceDetection(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.path_to_haarcascade = self.get_haarcascade()
        self.face_cascade = cv2.CascadeClassifier() # Face detection classifier
        self.face_cascade.load(self.path_to_haarcascade)
        self.cap = cv2.VideoCapture(0)    
    
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

    def get_haarcascade(self):
        """
        Finding the path to haarcascades on local system
        """
        if sys.platform == 'win32':
            cv_file = cv2.__file__ # Get the path to opencv executable
            cv_file = str(cv_file)
            cv_file = cv_file.rsplit('\\', 1)[0] # Leaves us at root of opencv directory
            cv_file = cv_file + "\data\haarcascade_frontalface_alt.xml"
            return cv_file 
        else:
            cv_file = cv2.__file__ # Get the path to opencv executable
            cv_file = str(cv_file)
            cv_file = cv_file.rsplit('/', 1)[0] # Leaves us at root of opencv directory
            cv_file = cv_file + "/data/haarcascade_frontalface_alt.xml"
            return cv_file 

    def detectAndDisplay(self, frame):
        frame_gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        frame_gray = cv2.equalizeHist(frame_gray)
        #-- Detect faces
        faces = self.face_cascade.detectMultiScale(frame_gray)
        for (x,y,w,h) in faces:
            cv2.rectangle(frame, (x, y), (x + w, y + h),(0,255,0),thickness=4)
            faceROI = frame_gray[y:y+h,x:x+w]
            if sys.platform == 'win32':
                file_name = 'data\image.jpg'
            else:
                file_name = 'data/image.jpg'
                
            faceROI = cv2.resize(faceROI,(160,160))    
            cv2.imwrite(file_name, faceROI)
        cv2.imshow('Capture - Face detection', frame)

detection = FaceDetection()
detection.start()
