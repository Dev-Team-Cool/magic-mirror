/* global Module */

/* Magic Mirror
 * Module: MMM-Button
 *
 * MIT Licensed.
 */

Module.register('MMM-Button', {
	requiresVersion: "2.1.0",
	defaults: {
		buttonPIN: 5,
		notificationMessage: "BUTTON_PRESSED",
		//time in miliseconds before another button click is recognized
		clickDelay: 500,
		button2PIN: 27,
		notificationMessage2: "PAGE_DOWN",
	},
	// Override socket notification handler.
	socketNotificationReceived: function (notification, payload) {
		if (notification === this.config.notificationMessage) {
			Log.info('Button Pressed');
			this.sendNotification(this.config.notificationMessage, payload);
		}
		if (notification === this.config.notificationMessage2) {
			Log.info('Button 2 Pressed');
			this.sendNotification(this.config.notificationMessage2, payload);
		}
	},
	start: function () {
		this.sendSocketNotification('BUTTON_CONFIG', this.config);
		Log.info('Starting module: ' + this.name);
	}
});
