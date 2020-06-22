'use strict';
const NodeHelper = require('node_helper');

const { PythonShell } = require('python-shell');
var pythonStarted = false

module.exports = NodeHelper.create({
  python_start: function() {
    const self = this;
    const options = {
			mode: 'text',
			pythonPath: this.config.pythonPath, // path to python executable, change this to where python is installed on your local machine
			scriptPath: this.config.scriptpath, // path to dir where the script to be started resides
      pythonOptions: ['-u'], // Unbuffered
      args: ["--ip", "0.0.0.0", "--port", "8000"]
    };
    
    self.pyshell = new PythonShell(
			// starts the face recognition script
			'webstreaming.py', options
		);

    self.pyshell.on('message', function(message) {
      try {
        const obj = JSON.parse(message);
        const keys = Object.keys(obj);

        if(keys[0] === 'photo'){
          self.sendSocketNotification('photo', obj.photo);
        }
        if(keys[0] === "result"){
          self.sendSocketNotification('user', obj.result);
        }
        if(keys[0] === "location"){
          self.sendSocketNotification('location', obj.location);
        }
        if(keys[0] === "server"){
          self.sendSocketNotification('server', obj.server)
        }
      } catch {
        console.log(message);
      }
    });

    self.pyshell.end(function(err) {
      if (err) throw err;
      console.log("[" + self.name + "] " + 'finished running...');
    });
  },

  stop: function() {
    if (!this.pyshell) return;
		this.pyshell.end(() => {
      console.log('ended');
      self.sendSocketNotification('SMILEREC_STOPPED');
		})
	},

  // Subclass socketNotificationReceived received.
  socketNotificationReceived: function(notification, payload) {
    if (notification === 'start python') {
      this.config = payload
      if (!pythonStarted) {
        console.log("python started")
        pythonStarted = true;
        this.python_start();
      };
    };
    if (notification === 'stop python'){
      this.stop();
    }
  }

});