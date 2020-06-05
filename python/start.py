from facial_recognition.face_detection import FaceDetection
from facial_recognition.config import Config

# Load the config
Config.load_config('/home/florian/Documents/school/magic-mirror/python/config.json')
detection = FaceDetection()
detection.start()
