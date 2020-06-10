/* global Module */

/* Magic Mirror
 * Module: MMM-facialrec
 *
 * By mauritscottyn
 * MIT Licensed.
 */

Module.register("MMM-facialrec", {
	defaults: {
		updateInterval: 5000,
		retryDelay: 5000,
		animationSpeed: 0,
		pythonPath: '',
		scriptPath: ''
	},

	requiresVersion: "2.1.0", // Required version of MagicMirror

	start: function() {
		// Variable that keeps track of the current user.
		this.user = "No user found";
		this.sendSocketNotification('INIT', this.config);
		
		// Sends a request to start the python script every 5 seconds, node helper ignores this request if python script is already started.
		setInterval(() => {
			this.sendSocketNotification("START_RECOGNITION", true);
		}, this.config.updateInterval);
	},

	// Generates the DOM elements to be displayed, currently it only shows the name of the current user.
	getDom: function() {
		// create element wrapper that will be displayed in the module.
		var wrapper = document.createElement("div");
		wrapper.innerHTML = this.user.name;
		return wrapper;
	},

	notificationReceived: function(notification, payload, sender) {
		if (notification === 'DOM_OBJECTS_CREATED') {
			MM.getModules().exceptModule(this).enumerate(function(module) {
				module.hide(1000, function() {
					//Module hidden.
				});
			});
		}
	},

	// If additional scripts need to be loaded they go here.
	getScripts: function() {
		return [];
	},

	// Css files are loaded here.
	getStyles: function () {
		return [
			"MMM-facialrec.css",
		];
	},
	processPrediction: function(prediction) {
		if (prediction == 'no user' || prediction == 'unknown')
			this.unknownFlow();
		else if(this.user !== prediction)
			this.userFlow(prediction);
	},
	findUser: function (userIdentiefer) {
		// TODO: Fetch user data from the local API service
		return {
			name: userIdentiefer,
			tokens: {
				// tokens here
			}
		}
	},
	hideOtherModules: function () {
		MM.getModules().exceptModule(this).enumerate(function(module) {
			module.hide(1000);
		});
	},
	showOtherModules: function () {
		MM.getModules().exceptModule(this).enumerate(function(module) {
			module.show(1000);
		});
	},
	unknownFlow: function () {
		// TODO: Handle user left situation for the GoogleAssistant module
		this.user = { name: "Hello stranger!" }
		this.sendNotification('USER_LEFT')
		this.updateDom();
		this.hideOtherModules();
	},
	userFlow: function (user) {
		const currentUser = this.findUser(user);
		this.user = currentUser;
		this.sendNotification('USER_FOUND', currentUser)
		this.updateDom();
		this.showOtherModules();
	},
	// socketNotificationReceived from node helper
	socketNotificationReceived: function (notification, payload) {
		switch(notification)  {
			case 'USER_FOUND':
				this.processPrediction(payload)
				break;
			default:
				break;
		}
	}
});
