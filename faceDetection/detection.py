import cv2

"""
Finding the path to haarcascades on local system (windows only)
"""
cv_file = cv2.__file__
cv_file = str(cv_file)
cv_file = result = cv_file.rsplit('\\', 1)[0]
cv_file_face = cv_file + "\data\haarcascade_frontalface_alt.xml"
cv_file_eyes = cv_file + "\data\haarcascade_eye_tree_eyeglasses.xml"
print(cv_file_face)


"""
Creating face detection classifiers.
"""
face_cascade = cv2.CascadeClassifier()
eyes_cascade = cv2.CascadeClassifier()
face_cascade.load(cv_file_face)
eyes_cascade.load(cv_file_eyes)


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
        frame = cv2.ellipse(frame, center, (w//2, h//2), 0, 0, 360, (255, 0, 255), 4)
        faceROI = frame_gray[y:y+h,x:x+w]
        #-- In each face, detect eyes
        eyes = eyes_cascade.detectMultiScale(faceROI)
        for (x2,y2,w2,h2) in eyes:
            eye_center = (x + x2 + w2//2, y + y2 + h2//2)
            radius = int(round((w2 + h2)*0.25))
            frame = cv2.circle(frame, eye_center, radius, (255, 0, 0 ), 4)
    cv2.imshow('Capture - Face detection', frame)


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
