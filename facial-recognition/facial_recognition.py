from facenet_pytorch import InceptionResnetV1
import numpy as np
from torch import Tensor
from joblib import dump, load
from utils import load_images


class FacialRecognition:
    def __init__(self, pretrained):
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

        embedding = FaceEmbedding(self.__resnet, '').calculate(img_tensor)
        self.do_prediction(embedding)
        # return self.__model.predict(embedding)
    
    def do_prediction(self, img_embedding):
        scores = []

        # for key, embeddings in self.vectors.items():
        #     for face_embedding in embeddings:
        #         scores.append({'users': key, 'score': np.linalg.norm(face_embedding - img_embedding)})
        #     users[key] = len(scores) - 1

        print('scores: ', scores)

    def __generate_tensor(self, image):
        from facenet_pytorch import MTCNN
        from matplotlib.image import imread

        img_file = imread(image)
        return MTCNN().forward(img_file)

    def train_classifier(self, training_data_path):
        # from sklearn.neighbors import KNeighborsClassifier
        # if self.__model: return

        X_train = load_images(training_data_path)
        tmp = []
        
        for key, images in X_train.items():
            tmp.append([FaceEmbedding(self.__resnet, key).calculate(self.__generate_tensor(image))) for image in images])
        
        self.vectors = tmp
        # X_train = np.array([self.__calculate_embedding((self.__generate_tensor(image))) for image in X_train])
        # X_train = X_train.reshape(-1, 512)
        # knn_classifier = KNeighborsClassifier(n_neighbors=2)
        # knn_classifier.fit(X_train, y_train)
        # print('Model trained')
        # self.__model = knn_classifier
    
    def export_classifier(self, path):
        dump(self.__model, path)

    def load_model(self, model_path):
        try:
            return load(model_path)
        except:
            return None

class FaceEmbedding:
    def __init__(self, resnet, label):
        self.__resnet = resnet
        self.embedding = None
        self.label = label

    def calculate(self, img_tensor):
        embedding = self.__resnet.forward(img_tensor.unsqueeze(0)).detach().numpy()
        self.embedding = embedding
        return self