import os
from enum import IntEnum
from PIL import Image
from PIL.ExifTags import TAGS


def load_images(directory):
    print(directory)
    sub_dirs = os.scandir(directory)
    images = {}
    for sub_dir in sub_dirs:
        if (sub_dir.is_dir()):
            images[sub_dir.name] = []
            for img_file in os.scandir(sub_dir.path):
                if img_file.is_file():
                    images[sub_dir.name].append(img_file.path)
    
    return images

def extract_exif(image):
    return image._getexif()

def parse_jpeg_exif(image):
    """Parse the raw meta-data of a JPEG image"""
    raw_exif = extract_exif(image)
    exif_parsed = {}
    if raw_exif is not None:
        for key, value in raw_exif.items():
            exif_parsed[TAGS[key]] = value

    return exif_parsed

def is_gray_scaled(image):
    pixel_mode = image.mode
    if pixel_mode == '1' or pixel_mode == 'L':
        return True
    
    return False

def is_jpeg(image):
    return image.format == 'JPEG'

def handle_orientation(image, image_orientation):
    if image_orientation == Orientation.ROTATE_180:
        image = image.transpose(Image.ROTATE_180)
    if image_orientation == Orientation.ROTATE_90_CW:
        image = image.transpose(Image.ROTATE_270)
    elif image_orientation == Orientation.ROTATE_270_CW:
        image = image.transpose(Image.ROTATE_90)
    
    return image

def prepare_image(image):
    # Check if image is gray scaled
    if (not is_jpeg(image)):
        return None
    if (is_gray_scaled(image)):
        print('Image is gray scaled')
        return None
    if image.height > 720:
        # Resize huge images to something smaller so they can get processed faster
        size = 1280, 720
        image.thumbnail(size, Image.ANTIALIAS)
    
    exif_data = parse_jpeg_exif(image)
    if 'Orientation' in exif_data.keys():
        image_orientation = exif_data.get('Orientation', Orientation.HORIZONTAL)
        image = handle_orientation(image, image_orientation)
    
    return image
        


class Orientation(IntEnum):
    HORIZONTAL = 1
    MIRROR_HORIZONZAL = 2
    ROTATE_180 = 3
    MIRROR_VERTICAL = 4
    MIRROR_HORIZONZAL_ROTATE_270_CW = 5
    ROTATE_90_CW = 6
    MIRROR_HORIZONZAL_ROTATE_90_CW = 7
    ROTATE_270_CW = 8
    