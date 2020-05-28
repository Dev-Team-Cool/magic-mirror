import json

class Config:

    config = None
    
    @staticmethod
    def load_config(config_file):
        try:
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