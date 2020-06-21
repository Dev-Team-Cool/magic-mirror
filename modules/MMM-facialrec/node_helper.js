/* Magic Mirror
 * Node Helper: MMM-facialrec
 *
 * By mauritscottyn
 * MIT Licensed.
 */

var NodeHelper = require("node_helper");
const { PythonShell } = require('python-shell');

module.exports = NodeHelper.create({
	initialize: function(config) {
		this.config = config;
	},
	python_start: function() {
		const options = {
			mode: 'text',
			pythonPath: this.config.pythonPath, // path to python executable, change this to where python is installed on your local machine
			scriptPath: this.config.scriptPath, // path to dir where the script to be started resides
			pythonOptions: ['-u'], // Unbuffered
		};

		this.activeShell = new PythonShell(
			// starts the face recognition script
			'start.py', options
		);
		this.activeShell.on("message", (message) => {
			// Receives a message in the form of {'detected': 'name of user'} the message is a string and needs to be converted to json first
			try {
				message = JSON.parse(message);
				const prediction = message.detected;
				if(prediction != "Unknown" && prediction != 'no user'){
					// If a user is found, send the user name to the module and stop the python shell. Shell will be restarted in 5 seconds to see if the user is still there
					// pyshell.childProcess.kill('SIGINT');
					if (prediction == 'badge')
						this.sendSocketNotification('BADGE_FOUND')
				}

				// Notify module of current prediction
				this.sendSocketNotification('USER_FOUND', prediction);
				
			} catch(err) {
				// Unable to parse as JSON; Probably just some random info from the Python script
				return;
			}
		})

		this.activeShell.on('close', err => {
			console.log('[FACEREC]	application stopped', err);
			// TODO: Handle random crashes. Why do they happen???
			//FIXME: Badge detection causes this. Don't know why
			this.sendSocketNotification('RECOGNITION_STOPPED');
		})

	},
	recognize: function() {
		if (this.pythonAlreadyStarted) return;
		this.python_start();
		this.pythonAlreadyStarted = true;
		this.sendSocketNotification('RECOGNITION_STARTED')
	},
	stop: function() {
		this.activeShell.end(() => {
			console.log('ended');
			this.pythonAlreadyStarted = false;
		})
	},
	socketNotificationReceived: function(notification, payload) {
		// This is where the helper receives it's notifications from the module. If the payload is start and pythonAlreadyStarted is false we will execute the python script.
		switch(notification) {
			case 'INIT':
				this.initialize(payload);
				break;
			case 'START_RECOGNITION':
				this.recognize();
				break;
			case 'STOP_RECOGNITION':
				this.stop();
				break;
		}
	  }
});
