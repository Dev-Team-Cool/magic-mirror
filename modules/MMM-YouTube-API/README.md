---
title: Mirror of Erised MMM-YouTube-API
tags: Project
description: Documentation module
---

# MMM-YouTube-API
MMM-YouTube-API is a simple way for displaying a youtube video upon receiving an event. If no video is being played a placeholder image will be shown instead.
## installation

```console
cd ~/MagicMirror/modules
svn checkout https://github.com/Dev-Team-Cool/magic-mirror/trunk/modules/MMM-YouTube-API
```

There is no need for npm install.

## Config

```javascript
{
    module: 'MMM-YouTube-API',
    position: 'middle_center',
    config: {
        videoID: 'tkgjOfJaCRQ',
        width: 960,
        height: 540,
        placeholderPath : "modules/MMM-YouTube-API/pics/ML6Logo.png"
    },
},
```

Configuration options for ```MMM-YouTube-API``` :

| Option                | Description                                                                                                                               |
|:--------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| ```videoID```         | Display of a video as embeded, it is also possible to use an embed link, a regular youtube link won't work. |
| ```width```           | The width of your media player.                                                                                                                   |
| ```height```          | The height of your media player                                                                                                                    |
| ```placeholderPath``` | Path to the static image, you can also add your own images to the folder pics and change the name.                      |

## Integration with other modules
You can start the video with the following notification:
```javascript
this.sendNotification("YT_PLAY_VIDEO");
```
There are other options, here the name of the notification shows its functionality.
```javascript
this.sendNotification("YT_SHOW_IMAGE");
this.sendNotification("YT_PAUSE_VIDEO");
this.sendNotification("YT_UNPAUSE_VIDEO");
this.sendNotification("YT_NEW_IMAGE");
```