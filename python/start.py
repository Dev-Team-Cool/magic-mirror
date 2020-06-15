from facial_recognition.face_detection import FaceDetection
from facial_recognition.config import Config

# Load the config
Config.load_config()
detection = FaceDetection().init()
detection.start()
