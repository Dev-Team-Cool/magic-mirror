import json
from pathlib import Path
import os

class Config:

    config = None
    
    @staticmethod
    def load_config(config_file = None):
        try:
            if not config_file:
                config_file = Config.find_config()
            with open(config_file) as config_file:
                Config.config = json.load(config_file)
        except FileNotFoundError:
            raise FileNotFoundError('File does not exists.', config_file)
        except:
            raise Exception('Could not load the config.', config_file)
    
    @staticmethod
    def get(key, fallback=None):
        if Config.config is None: return fallback
        if key not in Config.config.keys(): return fallback

        return Config.config.get(key, fallback)

    @staticmethod
    def find_config():
        home_folder = Path.home()
        return os.path.join(home_folder, 'Documents', 'config.json')