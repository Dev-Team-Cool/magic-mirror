import os

import requests

import facial_recognition.utils as utils
from facial_recognition.config import Config
from facial_recognition.facial_recognition import FacialRecognition

Config.load_config()
BASE_URL = 'http://localhost:5003/api/image'

train_data_path = Config.get('train_data_path')
if train_data_path is None:
    print('No training path defined. Exiting...')
    exit(0)

train_data = utils.load_images(Config.get('train_data_path'))
save_training_images = Config.get('save_training_data', False)

if len(train_data.keys()) == 0:
    print('No new data found. Exiting...')
    exit(0)

'''
Filter images based on the IsValid state from the API
User can remove train images in the webapp which causes the IsValid state to become false
'''
for key, images in train_data.items():
    tmp = []
    for idx, image in enumerate(images):
        r = requests.get(f'{BASE_URL}/{image.split("/")[-1]}')
        if r.status_code == 200:
            if r.text == 'true':
               tmp.append(image)
    
    train_data[key] = tmp # Update list with validated images from API

facial_recognition = FacialRecognition().load()
print('Training model...')
facial_recognition.train_classifier(train_data)
facial_recognition.save()
print('Done. Model saved to:', Config.get('model_path'))

if save_training_images:
    save_path = Config.get('saved_training_data_path')
    print('Moving training files to', save_path)
    if save_path is None:
        print('Could not move files. Exiting...')
        exit(0)
    
    for key in train_data.keys():
        try:
            os.renames(f'{train_data_path}/{key}', f'{save_path}/{key}')
        except FileExistsError:
            print(f'File already exists. Ignoring')
        except:
            print('Could not move a file for some reason.')

else:
    print('Removing training files.')
    # remove dirs
    for key, images in train_data.items():
        for image in images:
            os.remove(image)
        os.removedirs(f'{train_data_path}/{key}')

print('All Done!')
