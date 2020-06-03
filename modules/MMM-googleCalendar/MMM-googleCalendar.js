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
        wrapper.className = "calendar"
        var date = document.createElement('h3');
        date.innerHTML = this.getDate();
        wrapper.appendChild(date);
        var events = document.createElement('div');
        events.className = 'calendar_events';
        wrapper.appendChild(events);
        if (this.dataRequest){
            this.dataRequest.items.forEach(element => {
                var event_item = document.createElement('div');
                event_item.className = "event_item";
                wrapper.appendChild(event_item);
                var ei_dot = document.createElement('div')
                ei_dot.className = "ei_dot";
                wrapper.appendChild(ei_dot);
                var title = document.createElement('div')
                title.className = "ei_title";
                title.innerHTML = this.getTime(element);
                wrapper.appendChild(title);
                var summary = document.createElement('div');
                summary.className = "ei_copy";
                summary.innerHTML = element.summary;
                wrapper.appendChild(summary);

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
        var eventTime = new Date(date.start.dateTime);
        var hours = String(eventTime.getHours());
        var minutes = String(eventTime.getMinutes()).padStart(2, '0');
        var time = hours + ':' + minutes;
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