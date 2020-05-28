from setuptools import setup, find_packages

setup(
    name="facial recognition",
    version="1.0.0",
    author='Florian',
    description='Facial recognition module for magic mirror',
    packages = find_packages(),
    license='BSD',
    install_required=['torch',
                      'opencv_python==4.2.0.34',
                      'facenet_pytorch',
                      'Pillow',
                      'joblib']
)