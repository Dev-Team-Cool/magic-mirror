/* global Module */

/* Magic Mirror
 * Module: MMM-faceBadgeRec
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
		this.user = {
			name: 'No user found',
			validUser: false,
			hasBadge: false
		};
		this.FaceRecognitionTimeOutPassed = true;
		this.__previousPrediciton = 'no user';
		
		// Init node_helper with the config
		this.sendSocketNotification('INIT', this.config);
		
		// Sends a request to start the python script every 5 seconds, node helper ignores this request if python script is already started.
		this.sendSocketNotification("START_RECOGNITION", true);
		
		// setInterval(() => {
		// }, this.config.updateInterval);
	},

	// Generates the DOM elements to be displayed, currently it only shows the name of the current user.
	getDom: function() {
		// create element wrapper that will be displayed in the module.
		const wrapper = document.createElement("div");
		const { name, hasBadge, validUser } = this.user;
		if (validUser) {
			wrapper.innerHTML = `Hello ${name}!</br>`
			if (hasBadge)
				wrapper.innerHTML += 'Nice badge my dude!'
			else
				wrapper.innerHTML += 'Where yo badge at?' 
		} else
			wrapper.innerHTML = 'Hey stranger!'

		return wrapper;
	},

	notificationReceived: function(notification, payload, sender) {
		if (notification === 'DOM_OBJECTS_CREATED') {
			MM.getModules().exceptModule(this).enumerate(function(module) {
				module.hide(1000);
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
		Log.log(prediction)
		if (!this.FaceRecognitionTimeOutPassed) return false;
		if (this.__previousPrediciton === prediction) return false; 
		else this.__previousPrediciton = prediction;

		if (prediction == 'no user' || prediction == 'Unknown')
			this.unknownFlow(prediction);
		else if(this.user.name !== prediction)
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
	setTimeoutFacialRecognition: function () {
		this.FaceRecognitionTimeOutPassed = false;
		setTimeout(() => this.FaceRecognitionTimeOutPassed = true, this.config.updateInterval);
	},
	unknownFlow: function (prediction) {
		// TODO: Handle user left situation for the GoogleAssistant module
		this.user = { name: "Hello stranger!", hasBadge: false };

		if (prediction === 'no user')
			this.sendNotification('PAGE_SELECT', 0); //Logo screen
		else if (prediction === 'Unknown') {
			this.sendNotification('PAGE_SELECT', 'Home'); //Demo mode
			this.setTimeoutFacialRecognition(); // Check user again after x amount of time
		}

		this.sendNotification('USER_LEFT'); // Signal that the authenticated user has left
		this.updateDom();
		// this.hideOtherModules();
	},
	userFlow: function (user) {
		const currentUser = this.findUser(user);
		this.user = { validUser: true, ...currentUser};
		this.sendNotification('PAGE_SELECT', 'Personal'); // Goto the correct page
		this.sendNotification('USER_FOUND', currentUser);
		this.updateDom();
		this.setTimeoutFacialRecognition(); // Check user again after x amount of time
		// this.showOtherModules();
	},
	// socketNotificationReceived from node helper
	socketNotificationReceived: function (notification, payload) {
		switch(notification)  {
			case 'USER_FOUND':
				this.processPrediction(payload)
				break;
			case 'BADGE_FOUND':
				this.user.hasBadge = true;
				this.updateDom();
				break;
			default:
				break;
		}
	}
});
