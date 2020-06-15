# Magic Mirror

![WebApp Build](https://github.com/Dev-Team-Cool/magic-mirror/workflows/WebApp%20Build/badge.svg?branch=master)

# Getting started

If you want to run a development build of the API or webapp, you'll first have to open port `5432` in the docker-compose file. Add these lines to the database configuration.
```docker
	ports:
	  - "5432:5432"
```

## Install MagicMirror

Clone the [MagicMirror](https://github.com/MichMich/MagicMirror) repo.

## Link the config file

Create a symlink from our `config.js` file to the `~/MagicMirror/config/` folder.

```Shell
# unix
ln -s modules/config.js ~/MagicMirror/config/config.js

# Windows
New-Item -Path ~\MagicMirror\config\config.js -ItemType SymbolicLink -Value modules\config.js
```

You can also copy the file, but then git would not know of any changes you make to the file. So it's best to just make a symlink.

## Link the modules

There are several modules that we created or adapted ourself. You can link these with the same command as with the config file.

```Shell
# nix
ln -s modules/MMM-GoogleAssistant ~/MagicMirror/modules/MMM-GoogleAssistant

# Windows
New-Item -Path ~\MagicMirror\modules\MMM-GoogleAssistant -ItemType SymbolicLink -Value modules\MMM-GoogleAssistant
```

Do this for every module in the `modules` folder.


## Install the other modules

Install the other required modules.