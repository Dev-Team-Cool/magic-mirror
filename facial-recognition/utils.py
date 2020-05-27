import os
from PIL.ExifTags import TAGS


def load_images(directory):
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
    for key, value in raw_exif.items():
        exif_parsed[TAGS[key]] = value

    return exif_parsed