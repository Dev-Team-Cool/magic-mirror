from setuptools import setup, find_packages

setup(
    name="facial recognition",
    version="1.0.0",
    author='Florian',
    description='Facial recognition module for magic mirror',
    packages = find_packages(),
    license='BSD',
    install_requires=['torch',
                      'torchvision',
                      'opencv_python',
                      'facenet_pytorch',
                      'Pillow',
                      'detecto']
)