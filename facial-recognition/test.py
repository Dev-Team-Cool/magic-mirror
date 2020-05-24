from face_recognition import FacialRecognition
from facenet_pytorch import MTCNN
from matplotlib.image import imread

train_data = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/train'

facial_recognition = FacialRecognition()

# Train the classifier
facial_recognition.train_classifier(train_data)
facial_recognition.export_classifier('facerecognition.dev')

# Read image and predict
test_image_path = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/val/elton_john/httpafilesbiographycomimageuploadcfillcssrgbdprgfacehqwMTEODAOTcxNjcMjczMjkzjpg.jpg'
test_image = imread(test_image_path)
prediction = facial_recognition.predict(MTCNN().forward(test_image))
print('This is: ', prediction[0])