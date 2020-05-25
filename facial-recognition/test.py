from facial_recognition import FacialRecognition
from facenet_pytorch import MTCNN, extract_face
from PIL import Image, ImageDraw
from utils import load_images

train_data = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/trainV2'

facial_recognition = FacialRecognition('')

# Train the classifier
facial_recognition.train_classifier(train_data)
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

def draw_box(img, box):
    draw = ImageDraw.Draw(img)
    draw.rectangle(box)

test_image_path = '/home/florian/Documents/school/magic-mirror/facial-recognition/data/valV2/Gaelle/2020-05-24-211038.jpg'
test_image = Image.open(test_image_path)
size=480,480
test_image.thumbnail(size, Image.ANTIALIAS)
boxes, proba = MTCNN().detect(test_image)

print(len(boxes))
faces = []

# for box in boxes:
box = boxes[0]
print(box)
draw_box(test_image, box)
face_img = extract_face(test_image, box, save_path='data/extracted_test.jpg')
faces.append(facial_recognition.predict(face_img))

test_image.show()
