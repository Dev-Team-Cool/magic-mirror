from collections import Counter

import numpy as np
from facenet_pytorch import (InceptionResnetV1, extract_face,
                             fixed_image_standardization)
from joblib import dump, load
from PIL import Image

import facial_recognition.utils as utils
from facial_recognition.config import Config


class FacialRecognition:
    def __init__(self):
        self.__resnet = InceptionResnetV1('vggface2').eval()
        self.__embeddings = None

    @property
    def embeddings_loaded(self):
        return not self.__embeddings is None
    
    def predict(self, img_tensor):
        """
        Do a prediction about who this person is.

        :param img_tensor: The tensor generated from MTCNN
        :returns: A tuple with the class and the probability
        """
        if self.__embeddings is None:
            raise TypeError('No face embeddings where loaded. Make sure one exists.')
        
        results = []
        if isinstance(img_tensor, list):
            for tensor in img_tensor: 
                embedding = FaceEmbedding(self.__resnet, '').calculate_embedding(tensor)
                results.append(self.__do_prediction(embedding))
            return results
        else:
            embedding = FaceEmbedding(self.__resnet, '').calculate_embedding(img_tensor)
            return self.__do_prediction(embedding)
    
    def __do_prediction(self, img_embedding):
        for embedding in self.__embeddings:
            # Calculate each distance from the training examples to the reference person
            embedding.calculate_distance(img_embedding)
        
        self.__embeddings.sort()
        closest_embeddings = self.__embeddings[:3] # Get the top 3 closest embeddings

        if closest_embeddings[0].score > Config.get('score_treshold', 1.1):
            # If the best score is greater then a certain treshold we classify the person as unknown
            return ['Unknown', closest_embeddings[0].probability]
        
        # Return the predicted class and average probability for the 3 closest
        return (self.__majority_voting(self.__count_occurences(closest_embeddings)), np.average([embedding.probability for embedding in closest_embeddings]))

    def __count_occurences(self, face_embeddings):
        """Count the occurences per class"""
        count = {}
        for embedding in face_embeddings:
            if embedding.label not in count.keys():
                count[embedding.label] = 1
            else:
                count[embedding.label] += 1
        
        return count

    def __majority_voting(self, embedding_count):
        counter = Counter(embedding_count)
        return counter.most_common(1)[0][0] # Only return the label of the FaceEmbedding

    def __generate_tensor(self, image_file):
        """Creates a tensor of the faces based on the input image"""
        image = Image.open(image_file)
        image = utils.prepare_image(image)

        face = self.__detector.forward(image)
        if face is None:
            print(image_file, '- NOK')
            return None
        print(image_file, '- OK')
        return face

    def train_classifier(self, training_data):
        from facenet_pytorch import MTCNN
        self.__detector = MTCNN()
        
        tmp = []
        
        for label, images in training_data.items():
            for image in images:
                face = self.__generate_tensor(image)
                if face is not None:
                    tmp.append(FaceEmbedding(self.__resnet, label, image).calculate_embedding(face))
        
        if self.__embeddings is not None:
            self.__embeddings = self.__embeddings + tmp
        else:
            self.__embeddings = tmp

    def save(self):
        if self.__embeddings is not None:
            dump(self.__embeddings, Config.get('model_path', 'facial_recognition.model'))
    
    def load(self):
        try:
            self.__embeddings = load(Config.get('model_path', 'facial_recognition.model'))
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

    @property
    def probability(self):
        return (4 - self.score) / 4 * 100
 
    def calculate_embedding(self, img_tensor):
        embedding = self.__resnet.forward(img_tensor.unsqueeze(0)).detach().numpy()
        self.embedding = embedding

        return self

    def calculate_distance(self, reference_embedding):
        """Calculate the euclidian distance between this embedding and the reference emebedding"""
        if (self.embedding is not None):
            self.score = np.linalg.norm(self.embedding - reference_embedding.embedding)

        return self
    
    def __lt__(self, other):
        return self.score < other.score
    
    def __str__(self):
        return f'Score: {self.score} - label: {self.label}'
