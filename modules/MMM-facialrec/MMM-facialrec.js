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
		animationSpeed: 0
	},

	requiresVersion: "2.1.0", // Required version of MagicMirror

	start: function() {
		var self = this;

		// Variable that keeps track of the current user.
		this.user = "No user found";
		
		// Sends a request to start the python script every 5 seconds, node helper ignores this request if python script is already started.
		setInterval(function() {
			self.sendSocketNotification("MMM-facialrec-NOTIFICATION_TEST", "start");
		}, this.config.updateInterval);
	},

	// Generates the DOM elements to be displayed, currently it only shows the name of the current user.
	getDom: function() {
		var self = this;

		// create element wrapper that will be displayed in the module.
		var wrapper = document.createElement("div");
		wrapper.innerHTML = this.user;
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

	// socketNotificationReceived from node helper
	socketNotificationReceived: function (notification, payload) {
		if(notification === "user" && payload !== this.user){
			console.log('user received')
			this.user = "Hello: " + payload;
			this.updateDom();
			MM.getModules().exceptModule(this).enumerate(function(module) {
				module.show(1000, function() {
					//Module shown.
				});
			});
		}
		if (notification === "user" && payload ===  "unknown"){
			this.user = "Hello stranger"
			this.updateDom();
			MM.getModules().exceptModule(this).enumerate(function(module) {
				module.hide(1000, function() {
					//Module hidden.
				});
			});
		};
		if (notification === "user" && payload === "no user"){
			this.user = "No user found";
			this.updateDom();
			MM.getModules().exceptModule(this).enumerate(function(module) {
				module.hide(1000, function() {
					//Module hidden.
				});
			});
		}
	},
});
