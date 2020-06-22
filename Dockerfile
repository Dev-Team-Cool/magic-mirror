FROM alpine

WORKDIR /python
COPY ./python/facial_recognition facial_recognition
COPY ./python/setup.py .
COPY ./python/train.py .
COPY ./python/validate_image.py .