---
title: Mirror of Erised MMM-GoogleCalendar EN
tags: Project
description: Documentation module
---

# MMM-GoogleCalendar

MMM-GoogleCalendar is a way to display someone's google calendar without needing an ICal address. This module is made specifically to work with our [WebApp/api](https://github.com/Dev-Team-Cool/magic-mirror/tree/develop/backend) and [MMM-facialrec](https://github.com/Dev-Team-Cool/magic-mirror/tree/develop/modules/MMM-facialrec)
## Installatie

```
sudo apt install subversion
cd ~/MagicMirror/modules
svn checkout https://github.com/Dev-Team-Cool/magic-mirror/trunk/modules/MMM-googleCalendar
```

npm install is not required.
## Config
```json=
{
    module: "MMM-googleCalendar",
    position: "top_left"
},
```
This module requires no configuration when running together with the WebApp/api and MMM-facialrec. If you wish to run this module independantly I would advise to use default calendar module.

## Integration with other modules
voor het ontvangen en verander van de user kunnen er bepaalde notifications verzonden worden:
To change the current user you can use the following notification.
```javascript=
this.sendNotification("USER_FOUND", user object);
```
To stop displaying the user you can use:
```javascript=
this.sendNotification("USER_LEFT")
```