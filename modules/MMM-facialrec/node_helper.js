/* Magic Mirror
 * Node Helper: MMM-facialrec
 *
 * By mauritscottyn
 * MIT Licensed.
 */

var NodeHelper = require("node_helper");
const { PythonShell } = require('python-shell');
var pythonStarted = false;

module.exports = NodeHelper.create({
	pyshell: null,
	python_start: function(){
		const self = this;
		var options = {
			mode: 'text',
			pythonPath: 'C:\\Users\\cotty\\AppData\\Local\\Programs\\Python\\Python38\\python.exe', // path to python executable, change this to where python is installed on your local machine
			scriptPath: 'C:\\src\\test_mm\\MagicMirror\\modules\\MMM-facialrec\\python\\', // path to dir where the script to be started resides
			pythonOptions: ['-u'], // Unbuffered
		};

		self.pyshell = new PythonShell(
			// starts the face recognition script
			'start.py', options
		);

		self.pyshell.on("message", function(message){
			// Receives a message in the form of {'detected': 'name of user'} the message is a string and needs to be converted to json first
			var obj = JSON.parse(message)
			if(obj.detected != "Unknown" && obj.detected != 'no user'){
				// If a user is found, send the user name to the module and stop the python shell. Shell will be restarted in 5 seconds to see if the user is still there
				self.sendSocketNotification('user', obj.detected);
				self.pyshell.childProcess.kill('SIGINT');
				pythonStarted = false;
			}
			else{
				// Notify module that no user has been found yet. Can either be an unknown face or no user
				self.sendSocketNotification('user', obj.detected);
			};
		})

	},
	socketNotificationReceived: function(notification, payload) {
		// This is where the helper receives it's notifications from the module. If the payload is start and pythonStarted is false we will execute the python script.
		if(payload === "start" && !pythonStarted){
			this.python_start();
			pythonStarted = true;
		};
	  }
});
