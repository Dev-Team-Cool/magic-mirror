---
title: Mirror of Erised MMM-Button EN
tags: Project
description: Documentation module
---

# MMM-Button
MMM-Button is a rework of the original [MMM-Button](https://github.com/ptrbld/MMM-Button) it is practicly the same with the sole difference that it supports 2 buttons instead of one
## Connecting buttons
To connect a button to the PI it is configured in a way that the button is connected to a GPIO pin and the other cable go's to the ground. The ground can be shared between the two buttons. The image below is an example, it may be possible that the pins on you're raspberry are diffrent !
![](https://i.imgur.com/3HQKF71.png)



## Installation

```console
cd ~/MagicMirror/modules
git clone #link
```
After cloning the repository you need to execute npm install to get all dependencies needed

```console
cd /MMM-Button
npm install
```

## Config
```javascript
{
    "module": "MMM-Button",
    "config": {
        "buttonPIN": 17,
        "button2PIN": 27,
        "notificationMessage": "PAGE_UP",
        "notificationMessage2": "PAGE_DOWN",

    }
},
```

The configuration for ```MMM-Button``` :

| Option                | Description                                                                                                                               |
|:--------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| ```buttonPIN```         | The GPIO pin of the first button |
| ```button2PIN```         | The GPIO pin of the second button |
| ```notificationMessage```         | The Message send when button 1 is pushed (for interacting with other modules) |
| ```notificationMessage2```         |The Message send when button 2 is pushed (for interacting with other modules) |
| ```clickDelay```         | The debounce time of the buttons (standard 500 ms) |

## Troubleshooting
It is allways possible that it look slike the button doesn't work or that the mirror gives an error. Here are some errors and how to resolve/find the source of the problem
### Error message (Not able to acces GPIO)
It is possible when downloading the buttons module you get errors that GPIO is inaccessable To fix this the user needs to be added to the GPIO group. This can be done by typing this code
```console
sudo adduser USER gpio
```
After this is done don't forget to relogin with the user so the changes are processed
### Error message (The folder you try to acces is locked)
This error may occure if another module/programm is already activly looking at the pinout of the selected pin. To fix this make sure that only MMM-Button is trying to acces the selected pins data.
### Buttons not working
If you don't get any error messages in the magic mirror and the buttons still don't work the problem may be with the buttons or cables. To test this you could open python en run the next code
```python
import RPi.GPIO as GPIO
import time

GPIO.setmode(GPIO.BCM)

GPIO.setup(18, GPIO.IN, pull_up_down=GPIO.PUD_UP)

while True:
    input_state = GPIO.input(18)
    if input_state == False:
        print('Button Pressed')
        time.sleep(0.2)
```

If there is no output with this check your wiring/cables and or buttons.
if there is output the problem lays somewhere else, probably in another module or by using two modules that acces the same