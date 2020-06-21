---
title: Mirror of Erised MMM-faceBadgeDetec EN
tags: Project
description: Documentation module
---

# MMM-facialrec
MMM-facialrec is a facial recognition module that works closely with our [web-app / api](https://github.com/Dev-Team-Cool/magic-mirror/tree/develop/backend) where if you register a user and upload at least three photo's the recognition model can be retrained to recognize the new user.

The recognition itself is a Python script that usese a [Pytorch Facenet implementation](https://github.com/timesler/facenet-pytorch) to calculate `face embeddings` and MTCNN for face detection.


## Installation

### 1. Python installation

Clone or download our Github repo.
```shell
# Clone the complete repo
git clone https://github.com/Dev-Team-Cool/magic-mirror

# Clone just the necessary parts; using svn
svn checkout https://github.com/Dev-Team-Cool/magic-mirror/trunk/python

cd magic-mirror/python
```

Install the necessary packages
```shell
# In the python folder
pip install -e ./
```

Everything should be correctly installed now.
To use the facial recognition you first have to train a model. You can do this by running `train.py`. Before you can successfully run this script we have to setup some config files.

There is a file `config.json` in the python folder. This contains all the necessary config properties to run the script.

#### Python config

| Property | Description | default |
| -------- | -------- | -------- |
| train_data_path     | Path where the training data lives, after they get validated by the webapp    | ''     |
| tmp_train_path | Path where images get uploaded to from the webapp, before they get validated | '' |
| save_training_data | Should the training data be kept or can it be deleted? | true |
| saved_training_data_path | Path where the training data should be moved to after training | '' |
| model_path | Location to save / load the trained model | '' |
| badge_model_path | Location of the Badge detection model | '' |
| score_treshold | Hyperparameter to adjust the prediction treshold of the facial recognition | 1.1 |

The training data should be structured as this:
```shell
user1
    - image_file_user1.jpeg
    - image_file_user1.jpeg
user2
    - image_file_user2.jpeg
    - image_file_user2.jpeg
```

You should now be able to run the `train.py` script.

### 2. MagicMirror module installation

If you cloned the complete repo in step 1, you just have to change  your directory to `~/magic-mirror/modules`.

```shell
cd ~/magic-mirror/modules
```

You can either copy the complete `python` folder from step 1 into this folder. Or you can just create a symlink to the `start.py` file.

```shell
# Copy folder
cp -r ~/magic-mirror/python MMM-facialrec/python

# Symlink
ln -s ~/magic-mirror/python/start.py MMM-facialrec/start.py
```

Once this is done you can copy the `MMM-facialrec` folder to the MagicMirror modules folder. Alternativly you can again create a symlink to the folder.

```shell
# Copy the folder
cp -r MMM-facialrec ~/MagicMirror/modules

# Symlink
ln -s ~/MagicMirror/modules/MMM-facialrec MMM-facialrec
```

Go ahead and cd into the `MagicMirror/modules/MMM-facialrec` and install the necassry npm packages folder.

```shell
cd ~/MagicMirror/modules/MMM-facialrec
npm i
```

#### MagicMirror config
```json=
{
    module: "MMM-facialrec",
    position: "top_right",
    config: {
        pythonPath: 'path to where python is installed on your system',
        scriptPath: '~/MagicMirror/modules/MMM-facialrec'
    }
},

```
Configuration options for `MMM-facialrec`:

| Option                | Description                                                                                                                               |
|:--------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| ```pythonPath```         | The path to where python is installed on your system.|
| ```scriptPath```           | This should be the path to the directory where the script start.py resides, if you're using our release this should be in the directory MMM-facialrec.


## Integration with other modules
The module sends out a constant notification with the current user status:
```javascript
'USER_FOUND'
// Object
{
    username: 'renaat@vrt.be',
    firstName: 'Renaat',
    lastName: 'Schotte',
    isActive: true // Allow recognition,
    settings: {
        calendar: true,
        assistant: true,
        commute: false
    },
    commuteInfo: {
        address: {
            city: 'Anzegem'
        },
        commuteWay: 'Train'
    }
    
}
```
In the case a user has left or no user is recognized
```javascript
'USER_LEFT'
```
In the case of a badge being detected we send out the following event.
```javascript
'BADGE_DETECTED'
```


## Problems with install
- No standard ARM version pytorch [solution](https://medium.com/secure-and-private-ai-writing-challenge/a-step-by-step-guide-to-installing-pytorch-in-raspberry-pi-a1491bb80531)
    - Need to install python 3.6
    - Need to install wheel file [Link](https://forums.developer.nvidia.com/t/pytorch-for-jetson-nano-version-1-5-0-now-available/72048)
- For armV7 [Link](https://medium.com/hardware-interfacing/how-to-install-pytorch-v4-0-on-raspberry-pi-3b-odroids-and-other-arm-based-devices-91d62f2933c7) => don't use pytorch 1.5 but use 1.4 instead
- [LINK](https://github.com/maltequast/pytorch_arm_whl)
- Fix CV2 raspberry pi [Link](https://stackoverflow.com/questions/52378554/problems-importing-open-cv-in-python)
    - Run with ```LD_PRELOAD=/usr/lib/arm-linux-gnueabihf/libatomic.so.1 python magic-mirror/python/start.py ```
- Other errors
    - no module named detecto => pip3 install detecto
    - No module named 'torchvision.models.detection' => [LINK](https://github.com/sungjuGit/PyTorch-and-Vision-for-Raspberry-Pi-4B/blob/master/torchvision-0.5.0a0%2B9cdc814-cp37-cp37m-linux_armv7l.whl)
    - ModuleNotFoundError: No module named 'facenet_pytorch' => ```pip3 install facenet_pytorch```

- if unable to load image
    - change detecto.core line 565 to ```model.model.load_state_dict(torch.load(file,map_location=model._device), strict = False)```