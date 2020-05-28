# Facial Detection / Recognition module

## Get Started

1. Train a model  
   Change the `train_data_path` in the `config.json` file with the correct path.
   Make sure the training data is split into folder with the correct label eg. a first name
   ```
    /train-data
        /Florian
            afbeelding1.jpg
            afbeelding2.jpg
            ...
        /Maurits
            ...
        /Jens
            ...
   ```
2. (Optional) validate in on a test set with `test.py`  
    The `test.py` takes one of 2 parameters. Either you pass an image with the `-f` parameter or you pass a complete directory with the `-d` parameter.  
    If an image is passed the script will try to do a prediction on the image content and return its guess.  
    If a directory is passed it will do a prediction for every image and return certain metrics such as accuracy, confussion_matrix.
    Make sure the folder structure is the same as with the training data. 
3. Run `start.py` to start the face detection and face recognition.  
    Set an environment variable `DEBUG` to get a live feed from your webcam.  
    ```
        export DEBUG=true
        python start.py
    ```

## Validate a single image

You can use the script `validate_image.py` to validate a single image.  
Pass the filename as a parameter and set the property `tmp_train_path` in the `config.json` file.
The script will automatically move a correctly processed file.

```
    python validate_image.py afbeelding.jpg
```