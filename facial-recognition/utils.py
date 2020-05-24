import os


def load_images(directory):
    sub_dirs = os.scandir(directory)
    images = {}
    for sub_dir in sub_dirs:
        if (sub_dir.is_dir()):
            images[sub_dir.name] = []
            print('Directory:', sub_dir.name)
            for img_file in os.scandir(sub_dir.path):
                if img_file.is_file():
                    images[sub_dir.name].append(img_file.path)
    
    return images