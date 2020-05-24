import os


def load_images(directory):
    sub_dirs = os.scandir(directory)
    images, labels = [], []
    for sub_dir in sub_dirs:
        if (sub_dir.is_dir()):
            print('Directory:', sub_dir.name)
            for img_file in os.scandir(sub_dir.path):
                if img_file.is_file():
                    images.append(img_file.path)
                    labels.append(sub_dir.name)
    
    return images, labels