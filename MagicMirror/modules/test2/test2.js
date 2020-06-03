Module.register("test2",{
	// Default module config.
	defaults: {
        text: "Hello World!",
        url: 'modules/test2/events.json'
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
        wrapper.className = "calendar dark"
        var list = document.createElement('li');
        wrapper.appendChild(list);
        if (this.dataRequest){
            this.dataRequest.items.forEach(element => {
                var list_element = document.createElement('li');
                list_element.innerHTML += element.summary;
                wrapper.appendChild(list_element);

            });
        }
        console.log(wrapper);
        return wrapper;
    },

    getStyles: function () {
		return [
			"test2.css",
		];
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