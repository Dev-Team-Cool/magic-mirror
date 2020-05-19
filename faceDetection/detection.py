import cv2
import sys

windows = False

"""
Finding the path to haarcascades on local system
"""
if sys.platform == 'win32':
    print('windows')
    windows = True
    cv_file = cv2.__file__ # Get the path to opencv executable
    cv_file = str(cv_file)
    cv_file = cv_file.rsplit('\\', 1)[0] # Leaves us at root of opencv directory
    cv_file_face = cv_file + "\data\haarcascade_frontalface_alt.xml" 
else:
    cv_file = cv2.__file__ # Get the path to opencv executable
    cv_file = str(cv_file)
    cv_file = cv_file.rsplit('/', 1)[0] # Leaves us at root of opencv directory
    cv_file_face = cv_file + "/data/haarcascade_frontalface_alt.xml" 


"""
Creating face detection classifier.
"""
face_cascade = cv2.CascadeClassifier()
face_cascade.load(cv_file_face)


"""
Function that handles face detection and drawing of rectangle
"""
def detectAndDisplay(frame):
    frame_gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    frame_gray = cv2.equalizeHist(frame_gray)
    #-- Detect faces
    faces = face_cascade.detectMultiScale(frame_gray)
    for (x,y,w,h) in faces:
        center = (x + w//2, y + h//2)
        cv2.rectangle(frame, (x, y), (x + w, y + h),(0,255,0),thickness=4)
        # frame = cv2.ellipse(frame, center, (w//2, h//2), 0, 0, 360, (255, 0, 255), 4)
        faceROI = frame_gray[y:y+h,x:x+w]
        if windows:
            file_name = 'data\image.jpg'
        else:
            file_name = 'data/image.jpg'
            
        faceROI = cv2.resize(faceROI,(160,160))    
        cv2.imwrite(file_name, faceROI)
    cv2.imshow('Capture - Face detection', frame)


"""
Function that handles the saving of detected faces
"""
# def saveDetection(img)

"""
Main program, reads from webcam and calls detect and display function to find faces in the frame
"""
cap = cv2.VideoCapture(0)

while True:
    ret, frame = cap.read()
    if frame is None:
        print('--(!) No captured frame -- Break!')
        break
    detectAndDisplay(frame)
    if cv2.waitKey(10) == 27:
        break
