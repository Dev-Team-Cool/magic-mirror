from facenet_pytorch import InceptionResnetV1
from torch import Tensor
from joblib import dump, load
from utils import load_images


class FacialRecognition:
    def __init__(self):
        self.__resnet = InceptionResnetV1('vggface2').eval()
        self.__model = self.load_model()
    
    
    def predict(self, img_tensor: Tensor):
        """
        Do a prediction about who this person is.

        :param img_tensor: The tensor generated from MTCNN
        :returns: A tuple with the probability and the class
        """
        if self.__model is None:
            raise TypeError('No recognition model was loaded. Make sure one exists.')

        embedding = self.__calculate_embedding(img_tensor)
        return self.__model.predict(embedding)
    
    def __calculate_embedding(self, img_tensor: Tensor):
        print(img_tensor.shape)
        embedding = self.__resnet.forward(img_tensor.unsqueeze(0)).detach()
        print(embedding.shape)
        return embedding

    def __generate_tensor(self, image):
        from facenet_pytorch import MTCNN
        return MTCNN(image)

    def train(self):
        from sklearn.neighbors import KNeighborsClassifier
        if self.__model: return
        
        X_train, y_train = load_images('data/train')
        X_train = [self.__calculate_embedding((self.__generate_tensor(image))) for image in X_train]
        knn_classifier = KNeighborsClassifier(n_neighbors=2)
        knn_classifier.fit(X_train, y_train)
        print('Model trained')
        self.__model = knn_classifier

    def load_model(self):
        try:
            return load('facerecognition.dev')
        except:
            return None
