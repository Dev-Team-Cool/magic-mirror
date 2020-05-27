from facenet_pytorch import InceptionResnetV1, extract_face, fixed_image_standardization
import numpy as np
from torch import Tensor
from joblib import dump, load
from utils import load_images
from collections import Counter
from PIL.ExifTags import TAGS
from PIL import ImageDraw

class FacialRecognition:
    def __init__(self):
        self.__resnet = InceptionResnetV1('vggface2').eval()
        self.vectors = None
        # self.__model = self.load_model(pretrained)
    
    
    def predict(self, img_tensor: Tensor):
        """
        Do a prediction about who this person is.

        :param img_tensor: The tensor generated from MTCNN
        :returns: A tuple with the probability and the class
        """
        if self.vectors is None:
            raise TypeError('No vectors where loaded. Make sure one exists.')
        
        results = []
        if isinstance(img_tensor, list):
            for tensor in img_tensor: 
                embedding = FaceEmbedding(self.__resnet, '').calculate_embedding(tensor)
                results.append(self.do_prediction(embedding)[0])
            return results
        else:
            embedding = FaceEmbedding(self.__resnet, '').calculate_embedding(img_tensor)
            return self.do_prediction(embedding)
        # return self.__model.predict(embedding)
    
    def do_prediction(self, img_embedding):
        for embedding in self.vectors:
            embedding.calculate_distance(img_embedding)
        
        self.vectors.sort()
        best_scores = self.vectors[:3]

        print('scores: ')
        for score in self.vectors:
            print(score)
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
        exif = img_file._getexif()
        exif_parsed = {}
        for key, value in exif.items():
            exif_parsed[TAGS[key]] = value

        if 'Orientation' in exif_parsed.keys():
            if exif_parsed['Orientation'] == 6:
                img_file = img_file.transpose(Image.ROTATE_270)
            elif exif_parsed['Orientation'] == 8:
                img_file = img_file.transpose(Image.ROTATE_90)
        if img_file.height > 720:
            size = 480,480
            img_file.thumbnail(size, Image.ANTIALIAS)
        
        box, probas = MTCNN().detect(img_file)
        
        if box is None:
            return None
        return fixed_image_standardization(extract_face(img_file, box[0]))

    def train_classifier(self, training_data_path):
        # from sklearn.neighbors import KNeighborsClassifier
        # if self.__model: return

        X_train = load_images(training_data_path)
        tmp = []
        
        for key, images in X_train.items():
            for image in images:
                print('Handling ', image)
                img_cropped = self.__generate_tensor(image)
                if img_cropped is not None:
                    tmp.append(FaceEmbedding(self.__resnet, key, image).calculate_embedding(img_cropped))
        
        self.vectors = tmp
        # X_train = np.array([self.__calculate_embedding((self.__generate_tensor(image))) for image in X_train])
        # X_train = X_train.reshape(-1, 512)
        # knn_classifier = KNeighborsClassifier(n_neighbors=2)
        # knn_classifier.fit(X_train, y_train)
        # print('Model trained')
        # self.__model = knn_classifier
    
    def save(self, filename='face_embeddings.model'):
        if self.embeddings is not None:
            dump(self.embeddings, filename)

    def load(self, filename='face_embeddings.model'):
        try:
            self.embeddings = load(filename)
        except:
            print('Unable to load model')

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

def draw_box(img, box):
    draw = ImageDraw.Draw(img)
    draw.rectangle(box)