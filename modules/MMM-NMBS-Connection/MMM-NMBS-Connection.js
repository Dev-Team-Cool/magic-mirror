Module.register("MMM-NMBS-Connection", {

	defaults: {
		from: "http://irail.be/stations/NMBS/008893120",
		humanizeDuration: true,
		initialLoadDelay: 1000, // 1 second delay
		language: 'nl',
		results: 3,
		showStationNames: false,
		text: "",
		to: "http://irail.be/stations/NMBS/008821196",
		updateInterval: 10 * 60 * 1000, // 10 * 60 * 1000 = every 10 minutes
		url: "https://api.irail.be/connections",
		all: false, // True -> all users, False -> authenticated users
	},
	getScripts: function () {
		return ["moment.js"];
	},
	getStyles: function () {
		return ["MMM-NMBS-Connection.css", "font-awesome.css"];
	},
	getTranslations: function () {
		return {
			en: "translations/en.json",
			fr: "translations/fr.json",
			nl: "translations/nl.json",
		};
	},
	start: function () {
		this.loaded = false;
		this.forecast = this.config.text;
		this.updateTimer = null;
	},
	startSchedualing: function(user) {
		if (user.commuteInfo.commutingWay !== 'Train' || !user.settings.commute) {
			this.updateDom();
			this.hide();
			return; // User doesn't commute with train or hasn't opt-in for real-time commute info
		}

		if (user.commuteInfo && user.commuteInfo.address)
			this.config.to = user.commuteInfo.address.city;

		this.scheduleUpdate(this.config.initialLoadDelay);
	},
	notificationReceived: function(notification, payload) {
		switch(notification) {
			case 'USER_FOUND':
				this.startSchedualing(payload);
				break;
			default:
				break;
		}
	},
	getDom: function () {
		let wrapper = document.createElement("div");
		if (!this.loaded) {
			wrapper.innerHTML = this.forecast;
			wrapper.className = "MMM-NMBS-Connection dimmed light small";
			return wrapper;
		}

		wrapper = this.forecast;
		return wrapper;
	},
	updateTemp: function () {
		if (this.config.all) {
			url = `https://api.irail.be/liveboard/?station=${this.config.from}&arrdep=departure&format=json&lang=${this.config.language}`;
		}
		else {
			url = `${this.config.url}/?to=${this.config.to}&from=${this.config.from}&timeSel=depart&format=json&lang=${this.config.language}`;
		}

		fetch(url, {
			headers: {
				"User-Agent": "Mozilla/5.0 (Node.js) MagicMirror (https://github.com/MichMich/MagicMirror/)"
			}
		})
			.then(function (response) {
				return response.json();
			})
			.then((json) => {
				this.scheduleUpdate((this.loaded) ? -1 : this.config.updateInterval);

				return this.processConnections(json);
			})
			.catch(error => Log.error("Fetch Error =\n", error));
	},
	processConnections: function (data) {
		if (this.config.all) {
			let table = document.createElement("table");
			table.className = "MMM-NMBS-Connection";
			let tHead = document.createElement("thead");
			let headerRow = document.createElement("tr");
			let headerDeparture = document.createElement("td");
			headerDeparture.innerHTML = this.translate("DEPARTURE");
			let headerLine = document.createElement("td");
			// let headerArrival = document.createElement("td");
			// headerArrival.innerHTML = this.translate("ARRIVAL");

			if (this.config.showStationNames && data && data.stationinfo) {
				let departureStation = document.createElement("span");
				departureStation.className = "xsmall station-name";
				departureStation.innerHTML = data.stationinfo.standardname;
				headerDeparture.appendChild(departureStation);
			}

			// if (this.config.showStationNames && data && data.connection && data.connection[0]) {
			// 	let arrivalStation = document.createElement("span");
			// 	arrivalStation.className = "xsmall station-name";
			// 	arrivalStation.innerHTML = data.connection[0].arrival.station;
			// 	headerArrival.appendChild(arrivalStation);
			// }

			headerRow.appendChild(headerDeparture);
			headerRow.appendChild(headerLine);
			//headerRow.appendChild(headerArrival);
			tHead.appendChild(headerRow);
			table.appendChild(tHead);

			let connections = data.departures.departure;

			if (!Number.isFinite(this.config.results) || this.config.results > 6) {
				this.config.results = 6;
			}

			for (let i = 0; i < this.config.results; i++) {
				let connection = connections[i];
				let connectionRow = document.createElement("tr");
				let departureTime = document.createElement("td");
				departureTime.className = "title bright";
				if (parseInt(connection.canceled, 10) > 0) {
					departureTime.className = "dimmed line-through";
				}
				departureTime.innerHTML = moment.unix(connection.time).format("HH:mm");
				let departureDelay = document.createElement("span");
				departureDelay.className = "xsmall ontime";
				let delayMinutes = moment.utc(connection.delay * 1000).format("m");
				departureDelay.innerHTML = ` +${delayMinutes}`;
				if (parseInt(delayMinutes, 10) > 0) {
					departureDelay.className = "xsmall delayed";
				}
				departureTime.appendChild(departureDelay);
				connectionRow.appendChild(departureTime);

				// let line = document.createElement("td");
				// line.className = "dimmed";
				// let trainIcon = document.createElement("span");
				// trainIcon.className = "fa fa-train";
				// line.innerHTML = "&boxh;&boxh;&boxh;&boxh;&boxh;&boxh; ";
				// connectionRow.appendChild(line);

				// let arrivalTime = document.createElement("td");
				// arrivalTime.className = "title bright";
				// if (parseInt(connection.canceled, 10) > 0) {
				// 	arrivalTime.className = "dimmed line-through";
				// }
				// arrivalTime.innerHTML = moment.unix(connection.time).format("HH:mm");
				// let arrivalDelay = document.createElement("span");
				// arrivalDelay.className = "xsmall ontime";
				// let delayArrivalMinutes = moment.utc(connection.delay * 1000).format("m");
				// arrivalDelay.innerHTML = ` +${delayArrivalMinutes}`;
				// if (parseInt(delayArrivalMinutes, 10) > 0) {
				// 	arrivalDelay.className = "xsmall delayed";
				// }
				// arrivalTime.appendChild(arrivalDelay);
				// connectionRow.appendChild(arrivalTime);
				let arrivalTime = document.createElement("td");
				arrivalTime.className = "title bright";
				arrivalTime.innerHTML = connection.stationinfo.standardname;
				connectionRow.appendChild(arrivalTime);

				let infoRow = document.createElement("tr");
				let departurePlatform = document.createElement("td");
				departurePlatform.className = "xsmall";
				departurePlatform.innerHTML = `${this.translate("PLATFORM")} ${connection.platform}`;
				infoRow.appendChild(departurePlatform);

				let emptyLine = document.createElement("td");
				infoRow.appendChild(emptyLine);

				// let duration = document.createElement("td");
				// let durationTime = moment.utc(connection.duration * 1000).format('HH:mm');
				// if (this.config.humanizeDuration) {
				// 	durationTime = moment.duration(connection.duration * 1000).humanize();
				// }
				// duration.className = "xsmall";
				// duration.innerHTML = durationTime;

				// if (connection.vias && parseInt(connection.vias.number, 10) > 0) {
				// 	duration.innerHTML += `, ${connection.vias.number} ${this.translate("CHANGE")}`;
				// 	line.innerHTML = "&boxh;&boxh; ";
				// 	line.appendChild(trainIcon);
				// 	line.innerHTML += " &boxh;&boxh; ";
				// }

				// infoRow.appendChild(duration);
				table.appendChild(connectionRow);
				table.appendChild(infoRow);
			}

			this.forecast = table;

			this.show(this.config.animationSpeed, { lockString: this.identifier });
			this.loaded = true;
			this.updateDom(this.config.animationSpeed);
		}
		else {
			let table = document.createElement("table");
			table.className = "MMM-NMBS-Connection";
			let tHead = document.createElement("thead");
			let headerRow = document.createElement("tr");
			let headerDeparture = document.createElement("td");
			headerDeparture.innerHTML = this.translate("DEPARTURE");
			let headerLine = document.createElement("td");
			let headerArrival = document.createElement("td");
			headerArrival.innerHTML = this.translate("ARRIVAL");

			if (this.config.showStationNames && data && data.connection && data.connection[0]) {
				let departureStation = document.createElement("span");
				departureStation.className = "xsmall station-name";
				departureStation.innerHTML = data.connection[0].departure.station;
				headerDeparture.appendChild(departureStation);
			}

			if (this.config.showStationNames && data && data.connection && data.connection[0]) {
				let arrivalStation = document.createElement("span");
				arrivalStation.className = "xsmall station-name";
				arrivalStation.innerHTML = data.connection[0].arrival.station;
				headerArrival.appendChild(arrivalStation);
			}

			headerRow.appendChild(headerDeparture);
			headerRow.appendChild(headerLine);
			headerRow.appendChild(headerArrival);
			tHead.appendChild(headerRow);
			table.appendChild(tHead);

			let connections = data.connection;

			if (!Number.isFinite(this.config.results) || this.config.results > 6) {
				this.config.results = 6;
			}

			for (let i = 0; i < this.config.results; i++) {
				let connection = connections[i];
				let connectionRow = document.createElement("tr");
				let departureTime = document.createElement("td");
				departureTime.className = "title bright";
				if (parseInt(connection.departure.canceled, 10) > 0) {
					departureTime.className = "dimmed line-through";
				}
				departureTime.innerHTML = moment.unix(connection.departure.time).format("HH:mm");
				let departureDelay = document.createElement("span");
				departureDelay.className = "xsmall ontime";
				let delayMinutes = moment.utc(connection.departure.delay * 1000).format("m");
				departureDelay.innerHTML = ` +${delayMinutes}`;
				if (parseInt(delayMinutes, 10) > 0) {
					departureDelay.className = "xsmall delayed";
				}
				departureTime.appendChild(departureDelay);
				connectionRow.appendChild(departureTime);

				let line = document.createElement("td");
				line.className = "dimmed";
				let trainIcon = document.createElement("span");
				trainIcon.className = "fa fa-train";
				line.innerHTML = "&boxh;&boxh;&boxh;&boxh;&boxh;&boxh; ";
				connectionRow.appendChild(line);

				let arrivalTime = document.createElement("td");
				arrivalTime.className = "title bright";
				if (parseInt(connection.arrival.canceled, 10) > 0) {
					arrivalTime.className = "dimmed line-through";
				}
				arrivalTime.innerHTML = moment.unix(connection.arrival.time).format("HH:mm");
				let arrivalDelay = document.createElement("span");
				arrivalDelay.className = "xsmall ontime";
				let delayArrivalMinutes = moment.utc(connection.arrival.delay * 1000).format("m");
				arrivalDelay.innerHTML = ` +${delayArrivalMinutes}`;
				if (parseInt(delayArrivalMinutes, 10) > 0) {
					arrivalDelay.className = "xsmall delayed";
				}
				arrivalTime.appendChild(arrivalDelay);
				connectionRow.appendChild(arrivalTime);

				let infoRow = document.createElement("tr");
				let departurePlatform = document.createElement("td");
				departurePlatform.className = "xsmall";
				departurePlatform.innerHTML = `${this.translate("PLATFORM")} ${connection.departure.platform}`;
				infoRow.appendChild(departurePlatform);

				let emptyLine = document.createElement("td");
				infoRow.appendChild(emptyLine);

				let duration = document.createElement("td");
				let durationTime = moment.utc(connection.duration * 1000).format('HH:mm');
				if (this.config.humanizeDuration) {
					durationTime = moment.duration(connection.duration * 1000).humanize();
				}
				duration.className = "xsmall";
				duration.innerHTML = durationTime;

				if (connection.vias && parseInt(connection.vias.number, 10) > 0) {
					duration.innerHTML += `, ${connection.vias.number} ${this.translate("CHANGE")}`;
					line.innerHTML = "&boxh;&boxh; ";
					line.appendChild(trainIcon);
					line.innerHTML += " &boxh;&boxh; ";
				}

				infoRow.appendChild(duration);
				table.appendChild(connectionRow);
				table.appendChild(infoRow);
			}

			this.forecast = table;

			this.show(this.config.animationSpeed, { lockString: this.identifier });
			this.loaded = true;
			this.updateDom(this.config.animationSpeed);
		}

	},
	scheduleUpdate: function (delay) {
		let nextLoad = this.config.updateInterval;
		if (typeof delay !== "undefined" && delay >= 0) {
			nextLoad = delay;
		}

		clearTimeout(this.updateTimer);
		this.updateTimer = setTimeout(() => {
			this.updateTemp();
		}, nextLoad);
	},

});