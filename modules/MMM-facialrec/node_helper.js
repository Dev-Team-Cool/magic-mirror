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
		// const self = this;
		const options = {
			mode: 'text',
			pythonPath: this.config.pythonPath, // path to python executable, change this to where python is installed on your local machine
			scriptPath: this.config.scriptPath, // path to dir where the script to be started resides
			pythonOptions: ['-u'], // Unbuffered
		};

		pyshell = new PythonShell(
			// starts the face recognition script
			'start.py', options
		);
		pyshell.on("message", (message) => {
			// Receives a message in the form of {'detected': 'name of user'} the message is a string and needs to be converted to json first
			try {
				message = JSON.parse(message);
				const prediction = message.detected;
				console.log('Prediction: ', prediction)
				if(prediction != "Unknown" && prediction != 'no user'){
					// If a user is found, send the user name to the module and stop the python shell. Shell will be restarted in 5 seconds to see if the user is still there
					// pyshell.childProcess.kill('SIGINT');
					if (prediction == 'badge')
						this.sendSocketNotification('BADGE_FOUND')
					this.pythonAlreadyStarted = false;
					console.info('Ending...')
				}

				// Notify module of current prediction
				this.sendSocketNotification('USER_FOUND', prediction);
				
			} catch(err) {
				// Unable to parse as JSON; Probably just some random info from the Python script
				return;
			}
		})

	},
	recognize: function() {
		if (this.pythonAlreadyStarted) return;
		console.info('Starting...')
		this.python_start();
		this.pythonAlreadyStarted = true;
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
		}
	  }
});
