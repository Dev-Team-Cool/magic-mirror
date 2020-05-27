from facenet_pytorch import InceptionResnetV1, extract_face, fixed_image_standardization
import numpy as np
from torch import Tensor
from joblib import dump, load
from utils import load_images
import utils
from collections import Counter


class FacialRecognition:
    def __init__(self):
        self.__resnet = InceptionResnetV1('vggface2').eval()
        self.__embeddings = None

    @property
    def embeddings_loaded(self):
        return not self.__embeddings is None
    
    def predict(self, img_tensor: Tensor):
        """
        Do a prediction about who this person is.

        :param img_tensor: The tensor generated from MTCNN
        :returns: A tuple with the probability and the class
        """
        if self.__embeddings is None:
            raise TypeError('No face embeddings where loaded. Make sure one exists.')
        
        results = []
        if isinstance(img_tensor, list):
            for tensor in img_tensor: 
                embedding = FaceEmbedding(self.__resnet, '').calculate_embedding(tensor)
                results.append(self.do_prediction(embedding)[0])
            return results
        else:
            embedding = FaceEmbedding(self.__resnet, '').calculate_embedding(img_tensor)
            return self.do_prediction(embedding)
    
    def do_prediction(self, img_embedding):
        for embedding in self.__embeddings:
            embedding.calculate_distance(img_embedding)
        
        self.__embeddings.sort()
        best_scores = self.__embeddings[:3]

        if best_scores[0].score > 1.1:
            return ['Unknown', -1]
        
        count = {}
        for vector in best_scores:
            if vector.label not in count:
                count[vector.label] = 1
            else:
                count[vector.label] = count[vector.label] + 1
        
        counter = Counter(count)
        return counter.most_common(1)[0]

    def __generate_tensor(self, image):
        from facenet_pytorch import MTCNN
        from PIL import Image

        img_file = Image.open(image)
        exif_parsed = utils.parse_jpeg_exif(img_file)

        if 'Orientation' in exif_parsed.keys():
            if exif_parsed['Orientation'] == 6:
                img_file = img_file.transpose(Image.ROTATE_270)
            elif exif_parsed['Orientation'] == 8:
                img_file = img_file.transpose(Image.ROTATE_90)
        if img_file.height > 720:
            size = 480,480
            img_file.thumbnail(size, Image.ANTIALIAS)
        
        faces = self.__detector.forward(img_file)
        
        if len(faces) > 1:
            print('To many faces found.')
            return None
        face = faces[0]
        if face is None:
            print('No face found')
            return None

        return face

    def train_classifier(self, training_data_path):
        from facenet_pytorch import MTCNN
        self.__detector = MTCNN()
        
        traing_data = load_images(training_data_path)
        tmp = []
        
        for label, images in traing_data.items():
            for image in images:
                face = self.__generate_tensor(image)
                if face is not None:
                    tmp.append(FaceEmbedding(self.__resnet, label, image).calculate_embedding(face))
        
        self.__embeddings = tmp

    def save(self, filename='face_embeddings.model'):
        if self.__embeddings is not None:
            dump(self.__embeddings, filename)
    
    def load(self, filename='face_embeddings.model'):
        try:
            self.__embeddings = load(filename)
        except:
            print('Unable to load model')

        return self


class FaceEmbedding:
    def __init__(self, resnet, label, image_location = ''):
        self.__resnet = resnet
        self.embedding = None
        self.label = label
        self.score = 4
        self.file = image_location

    def calculate_embedding(self, img_tensor):
        embedding = self.__resnet.forward(img_tensor.unsqueeze(0)).detach().numpy()
        self.embedding = embedding

        return self

    def calculate_distance(self, reference_embedding):
        if (self.embedding is not None):
            self.score = np.linalg.norm(self.embedding - reference_embedding.embedding)

        return self
    
    def __lt__(self, other):
        return self.score < other.score
    
    def __str__(self):
        return f'Score: {self.score} - label: {self.label}'