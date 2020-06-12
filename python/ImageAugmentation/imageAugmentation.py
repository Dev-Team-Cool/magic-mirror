import cv2
import numpy as np
import os

# load the image
img = cv2.imread('BaseImage.png')
# img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

# draw mask
gray = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)


# define range of blue color in HSV
lower_blue = np.array([110, 100, 100])
upper_blue = np.array([170, 255, 255])

# Threshold the HSV image to get only blue colors
mask = cv2.inRange(gray, lower_blue, upper_blue)
mask = cv2.bitwise_not(mask)
# get first masked value (foreground)
fg = cv2.bitwise_or(img, img, mask=mask)
# get second masked value (background) mask must be inverted
mask = cv2.bitwise_not(mask)
#background = np.full(img.shape, 255, dtype=np.uint8)
i=0
for filename in os.listdir('textures'):
    backgroundimg = cv2.imread('textures/'+filename)
    background = np.full(img.shape, backgroundimg, dtype=np.uint8)
    bk = cv2.bitwise_or(background, background, mask=mask)

    # combine foreground+background
    final = cv2.bitwise_or(fg, bk)

    # cv2.imshow('',final)
    # cv2.waitKey(0)

    name = i
    cv2.imwrite(f'CreatedImages/image{name}.jpg', final)
    i+=1

