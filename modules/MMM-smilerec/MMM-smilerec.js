/* global Module */

/* Magic Mirror
 * Module: MMM-Facial-Recognition
 *
 * By Paul-Vincent Roll http://paulvincentroll.com
 * MIT Licensed.
 */

Module.register('MMM-smilerec', {

  defaults: {
		updateInterval: 5000,
		retryDelay: 5000,
		animationSpeed: 0,
		pythonPath: '',
		scriptPath: ''
	},

  // Override socket notification handler.
  socketNotificationReceived: function(notification, payload) {

    if (payload === 'smile') {
      this.message = "You are smiling~"
      this.updateDom()
    } 
    if(payload === "no smile") {
      this.message = "You are not smiling~"
      this.updateDom()
    }
    if(notification === "location"){
      if(this.console.length === 10){
        this.console.pop()
      }
      var element = payload;
      var keys = Object.keys(element);
      if(keys[0] === 'face'){
        element = 'Face detected at location: ' + element["face"];
      }
      if(keys[0] === 'smile'){
        element = 'Smile detected at location: ' + element["smile"]
      }
      this.console = [element].concat(this.console);
      // this.updateDom();
    }
    if(notification === 'photo'){
      console.log(payload);
      this.url = payload;
      // this.updateDom();

    }
  },

  start: function() {
    this.message = "detecting your smile...";
    this.smiling = undefined;
    this.console = {};
    this.url = ""
    this.sendSocketNotification('STOP_RECOGNITION', 'Stop recognition so smile detection can access the camera')
    this.sendSocketNotification('start python', this.config);
    Log.info('Starting module: ' + this.name);
    this.updateDom();
  },

  getDom: function() {
    var outputArray = this.console
    var wrapper = document.createElement("div");
    var title = document.createElement("div")
    var message = document.createTextNode(this.message);

    title.className = 'thin large bright';
    title.appendChild(message);
    wrapper.appendChild(title);

    var content = document.createElement('div');
    // for (let index = 0; index < outputArray.length; index++) {
    //   var element = outputArray[index];
    //   var output = document.createTextNode(element);
    //   content.appendChild(output);
    //   var linebreak = document.createElement('br');
    //   content.appendChild(linebreak);
    // }
    var image = document.createElement('img')
    image.src = "http://127.0.0.1:8000/video_feed";
    wrapper.appendChild(image);
    wrapper.appendChild(content);

    return wrapper;
  },

  suspend: function(){
    this.sendSocketNotification('stop python', 'stop python to give access to facialrec')
    this.sendSocketNotification('START_RECOGNITION', 'python stopped @ smile detection')
  }

});