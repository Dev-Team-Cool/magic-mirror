# import cv2
# import urllib.request
# from PIL import Image
# import numpy as np
#
# green = np.uint8([[[182, 50, 143]]])
# hsv_green = cv2.cvtColor(green,cv2.COLOR_BGR2HSV)
# print (hsv_green)



#[[[146 122 255]]]

# from detecto.core import Model
# from detecto.visualize import detect_video
#
# model = Model()  # Initialize a pre-trained model
# detect_video(model, 'http://admin:admin@10.42.9.30/tmpfs/auto.jpg', 'output.avi')

# http://opencv-python-tutroals.readthedocs.io/en/latest/py_tutorials/py_imgproc/py_houghcircles/py_houghcircles.html
import cv2
import numpy as np

# load the image
img = cv2.imread('ImageAugmentation/BaseImage.png')
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
backgroundimg=cv2.imread('ImageAugmentation/textureTest2.png')
background = np.full(img.shape, backgroundimg, dtype=np.uint8)
bk = cv2.bitwise_or(background, background, mask=mask)

# combine foreground+background
final = cv2.bitwise_or(fg, bk)

# cv2.imshow('',final)
# cv2.waitKey(0)

name = "test"
cv2.imwrite(f'ImageAugmentation/CreatedImages/image{name}.jpg',final)



