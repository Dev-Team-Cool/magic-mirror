from facial_recognition import FacialRecognition
from facenet_pytorch import MTCNN, extract_face, fixed_image_standardization
from PIL import Image, ImageDraw
from PIL.ExifTags import TAGS
from utils import load_images
import sklearn.metrics as metrics
import numpy as np

train_data = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/trainV2'

facial_recognition = FacialRecognition().load('face_embeddings.model')
if not facial_recognition.embeddings_loaded:
    # Train the classifier
    facial_recognition.train_classifier(train_data)
    facial_recognition.save()

# facial_recognition.export_classifier('facial_recognition.model')

# Read image and predict
# test_image_path = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/valV2/'
# X_test = load_images(test_image_path)

# for key, images in X_test.items():
#     for image in images:
#         test_image = Image.open(image)
#         size = 480, 480
#         test_image.thumbnail(size, Image.ANTIALIAS)
#         test_image = test_image.transpose(Image.ROTATE_270)
#         # test_image.show()
#         img_cropped = MTCNN().forward(test_image)

#         prediction = facial_recognition.predict(img_cropped)
#         print('Guess: ', prediction[0], ' - Actual: ', key)

# test_image_path = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/valV2'
# test_images = load_images(test_image_path)
# X_test = []
# X_test2 = []
# y_test = []

# detector = MTCNN()

# for label, images in test_images.items():
#     for image in images:
#         img_file = Image.open(image)
#         exif = img_file._getexif()
#         exif_parsed = {}
#         for key, value in exif.items():
#             exif_parsed[TAGS[key]] = value

#         if 'Orientation' in exif_parsed.keys() and exif_parsed['Orientation'] == 6:
#             img_file = img_file.transpose(Image.ROTATE_270)
#         if img_file.height > 720:
#             size = 480,480
#             img_file.thumbnail(size, Image.ANTIALIAS)

#         boxes, probas = detector.detect(img_file)
#         box = boxes[0]

#         X_test.append(fixed_image_standardization(extract_face(img_file, box)))
#         X_test2.append(MTCNN().forward(img_file))
#         y_test.append(label)

# print('Detect:', X_test)
# print('-----------------')
# print('Forward:', X_test2)
# print('-----------------s')
# y_pred = facial_recognition.predict(X_test)
# y_pred = np.array(y_pred)
# print('Results', y_pred)
# print('Report', metrics.classification_report(y_test, y_pred))
# print('Accuracy:', metrics.accuracy_score(y_test, y_pred))
        
test_image_path = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/mr_bean.jpg'
test_image = Image.open(test_image_path)
size=480,480
test_image.thumbnail(size, Image.ANTIALIAS)
# test_image = test_image.transpose(Image.ROTATE_270)
boxes, proba = MTCNN().detect(test_image)

print(len(boxes))
faces = []

for box in boxes:
    face_img = extract_face(test_image, box)
    faces.append(facial_recognition.predict(fixed_image_standardization(face_img)))

print(faces)