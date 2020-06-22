import argparse
import shutil
import os

from PIL import Image
from facenet_pytorch import MTCNN
from facial_recognition.config import Config
import facial_recognition.utils as utils

def main(img_file: str):
    tmp_path = Config.get('tmp_train_path')
    train_data_path = Config.get('train_data_path')

    if tmp_path is None:
        raise('No temp path assigned')

    full_image_path = f'{tmp_path}/{img_file}'
    image = Image.open(full_image_path)
    image = utils.prepare_image(image) # Check if gray scaled and resize, rotate, flip if needed
    if image is None:
        print('NOK')
        exit(0)
    
    detector = MTCNN()
    face = detector.forward(image) # Check for faces
    if face is None:
        print('NOK')
        exit(0)

    try:
        destination_dir = f'{train_data_path}/{img_file.split("_")[1]}'
        if not os.path.exists(destination_dir):
            os.makedirs(destination_dir)
        shutil.move(full_image_path, f'{destination_dir}/{img_file}')
    except:
        print('Failed to move file')
    finally:
        print('OK')

if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('file', help='filename')

    args = parser.parse_args()
    Config.load_config()
    main(args.file)
