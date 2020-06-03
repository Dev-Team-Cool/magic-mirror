Module.register("MMM-googleCalendar",{
	// Default module config.
	defaults: {
        text: "Hello World!",
        url: 'modules/MMM-googleCalendar/events.json'
    },
    
    requiresVersion: "2.1.0",

    start: function(){
        //Flag for check if module is loaded
		this.loaded = false;

        this.getData()
    },

    getData: function() {
        // Loads the calendar events from the url specified in defaults.url, sends the loaded events to processData.
        self = this;
        var dataRequest = new XMLHttpRequest();
        dataRequest.open("GET", self.defaults.url, true);
        console.log(self.defaults.url)
		dataRequest.onreadystatechange = function() {
			console.log(this.readyState);
			if (this.readyState === 4) {
				console.log(this.status);
				if (this.status === 200) {
					self.processData(JSON.parse(this.response));
				} else if (this.status === 401) {
					self.updateDom(self.config.animationSpeed);
					Log.error(self.name, this.status);
					retry = false;
				} else {
					Log.error(self.name, "Could not load data.");
				}
			}
        };
        
		dataRequest.send();
    },

    	// Override dom generator.
	getDom: function() {
        var wrapper = document.createElement("div");
        wrapper.className = "o-row";
        var container = document.createElement("div");
        container.className = "o-container"
        wrapper.appendChild(container);
        var date = document.createElement('h3');
        date.innerHTML = this.getDate();
        container.appendChild(date);
        var events = document.createElement('div');
        events.className = 'calendar_events';
        container.appendChild(events);
        if (this.dataRequest){
            this.dataRequest.items.forEach(element => {
                var event_item = document.createElement('div');
                event_item.className = "c-card";
                events.appendChild(event_item);
                var body = document.createElement('div');
                body.className = "c-card__body";
                event_item.appendChild(body);
                var title = document.createElement('div');
                title.className = "c-card__title";
                title.innerHTML = this.getTime(element);
                body.appendChild(title);
                var summary = document.createElement('p');
                summary.className = "c-body__text";
                summary.innerHTML = element.summary;
                body.appendChild(summary);
            });
        }
        console.log(wrapper);
        return wrapper;
    },

    getStyles: function () {
		return [
			"MMM-googleCalendar.css",
		];
    },
    
    getDate: function(){
        // This function returns a string with the current date, used to display at the top of the calendar
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        today = 'Calendar items for: ' + dd + '/' + mm + '/' + yyyy;
        return today;
    },

    getTime: function(date){
        // This function returns a string with hh+mm for a calendar event
        var eventTime = new Date(date.start.dateTime);
        var hours = String(eventTime.getHours());
        var minutes = String(eventTime.getMinutes()).padStart(2, '0');
        var time = hours + ':' + minutes;
        var eventTime = new Date(date.end.dateTime);
        var hours_end = String(eventTime.getHours());
        var minutes_end = String(eventTime.getMinutes()).padStart(2, '0');
        var time = hours + ':' + minutes + ' - ' + hours_end + ':' + minutes_end;
        return time
    },


    processData: function(data) {
		var self = this;
		this.dataRequest = data;
		if (this.loaded === false) { self.updateDom(self.config.animationSpeed) ; }
        this.loaded = true;
        // console.log(this.dataRequest)

		// the data if load
		// send notification to helper
		this.sendSocketNotification("test_module-NOTIFICATION_TEST", data);
	}
});