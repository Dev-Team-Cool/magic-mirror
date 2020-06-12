import argparse

import numpy as np
import sklearn.metrics as metrics
from facenet_pytorch import MTCNN, extract_face, fixed_image_standardization
from PIL import Image, ImageDraw
from PIL.ExifTags import TAGS

import facial_recognition.utils as utils
from facial_recognition.config import Config
from facial_recognition.facial_recognition import FacialRecognition


def load_model():
    Config.load_config('config.json')
    facial_recognition = FacialRecognition().load()
    if not facial_recognition.embeddings_loaded:
        raise Exception('No model loaded.')
    
    return facial_recognition

def main(test_dir):
    # Train a model first with train.py
    facial_recognition = load_model()

    test_images = utils.load_images(test_dir)
    X_test = []
    y_test = []

    detector = MTCNN()

    for label, images in test_images.items():
        for image in images:
            img_file = Image.open(image)
            img_file = utils.prepare_image(img_file)

            face = detector.forward(img_file)
            
            X_test.append(face)
            y_test.append(label)

    y_pred = facial_recognition.predict(X_test)
    y_pred = np.array(y_pred)
    print('Report', metrics.classification_report(y_test, y_pred))
    print('Accuracy:', metrics.accuracy_score(y_test, y_pred))

def predict_one(image_file):
    # Train a model first with train.py
    facial_recognition = load_model()
    detector = MTCNN()

    image = Image.open(image_file)
    image = utils.prepare_image(image)
    face = detector.forward(image)
    prediction = facial_recognition.predict(face)

    print('Prediction:', prediction)

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('-f', '--file', help="A file to predict on")
    parser.add_argument('-d', '--dir', help='A directory with the same folder structure as train data, to validate on.')

    args = parser.parse_args()
    if args.file:
        predict_one(args.file)
    elif args.dir:
        main(args.dir)
