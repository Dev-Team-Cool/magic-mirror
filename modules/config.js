/* Magic Mirror Config Sample
 *
 * By Michael Teeuw http://michaelteeuw.nl
 * MIT Licensed.
 *
 * For more information on how you can configure this file
 * See https://github.com/MichMich/MagicMirror#configuration
 *
 */

var config = {
	address: "localhost", // Address to listen on, can be:
	                      // - "localhost", "127.0.0.1", "::1" to listen on loopback interface
	                      // - another specific IPv4/6 to listen on a specific interface
	                      // - "0.0.0.0", "::" to listen on any interface
	                      // Default, when address config is left out or empty, is "localhost"
	port: 8080,
	ipWhitelist: ["127.0.0.1", "::ffff:127.0.0.1", "::1"], // Set [] to allow all IP addresses
	                                                       // or add a specific IPv4 of 192.168.1.5 :
	                                                       // ["127.0.0.1", "::ffff:127.0.0.1", "::1", "::ffff:192.168.1.5"],
	                                                       // or IPv4 range of 192.168.3.0 --> 192.168.3.15 use CIDR format :
	                                                       // ["127.0.0.1", "::ffff:127.0.0.1", "::1", "::ffff:192.168.3.0/28"],

	useHttps: false, 		// Support HTTPS or not, default "false" will use HTTP
	httpsPrivateKey: "", 	// HTTPS private key path, only require when useHttps is true
	httpsCertificate: "", 	// HTTPS Certificate path, only require when useHttps is true

	language: "en",
	timeFormat: 24,
	units: "metric",
	// serverOnly:  true/false/"local" ,
			     // local for armv6l processors, default
			     //   starts serveronly and then starts chrome browser
			     // false, default for all  NON-armv6l devices
			     // true, force serveronly mode, because you want to.. no UI on this device

	modules: [
		// {
		// 	module: "alert",
		// },
		// {
		// 	module: "updatenotification",
		// 	position: "top_bar"
		// },
		// {
		// 	module: "clock",
		// 	position: "top_left"
		// },
		// {
		// 	module: "calendar",
		// 	header: "US Holidays",
		// 	position: "top_left",
		// 	config: {
		// 		calendars: [
		// 			{
		// 				symbol: "calendar-check",
		// 				url: "webcal://www.calendarlabs.com/ical-calendar/ics/76/US_Holidays.ics"					}
		// 		]
		// 	}
		// },
		// {
		// 	module: "compliments",
		// 	position: "lower_third"
		// },
		// {
		// 	module: "currentweather",
		// 	position: "top_right",
		// 	config: {
		// 		location: "New York",
		// 		locationID: "", //ID from http://bulk.openweathermap.org/sample/city.list.json.gz; unzip the gz file and find your city
		// 		appid: "YOUR_OPENWEATHER_API_KEY"
		// 	}
		// },
		// {
		// 	module: "weatherforecast",
		// 	position: "top_right",
		// 	header: "Weather Forecast",
		// 	config: {
		// 		location: "New York",
		// 		locationID: "5128581", //ID from http://bulk.openweathermap.org/sample/city.list.json.gz; unzip the gz file and find your city
		// 		appid: "YOUR_OPENWEATHER_API_KEY"
		// 	}
		// },
		// {
		// 	module: "newsfeed",
		// 	position: "bottom_bar",
		// 	config: {
		// 		feeds: [
		// 			{
		// 				title: "New York Times",
		// 				url: "http://www.nytimes.com/services/xml/rss/nyt/HomePage.xml"
		// 			}
		// 		],
		// 		showSourceTitle: true,
		// 		showPublishDate: true,
		// 		broadcastNewsFeeds: true,
		// 		broadcastNewsUpdates: true
		// 	}
		// },
		{
			module: 'MMM-MyCommute',
			position: 'top_right',
			config: {
			  apiKey: "YOUR API KEY",
			  origin: 'Edgard Tinelstraat 91, Sinaai-Waas, 9112',
			  startTime: '00:00',
			  endTime: '23:59',
			  hideDays: [0,6],
			  destinations: [
				{
				  destination: 'Graaf Karel de Goedelaan 5, 8500 Kortrijk',
				  label: 'Work',
				  mode: 'driving',
				  color: '#b103fc'
				},
				{
					destination: 'Graaf Karel de Goedelaan 5, 8500 Kortrijk',
					label: 'Work',
					mode: 'transit',
					color: '#82E5AA'
				  },
				  {
					destination: 'Graaf Karel de Goedelaan 5, 8500 Kortrijk',
					label: 'Work',
					mode: 'bicycling',
					color: '#69aaf0'
				  }
			  ]
			}
		  },
		{
			module: "MMM-googleCalendar",
			position: "top_left",
			classes: "user"
		},
		{
			module: "MMM-NMBS-Connection",
			position: "bottom_right",
			config: {
			  from: "Antwerp-Central",
			  humanizeDuration: false,
			  language: "en",
			  results: 3,
			  showStationNames: true,
			  to: "Brussels-South"
			}
		},
		{
			module: "MMM-facialrec",
			position: "top_right",
			config: {
				pythonPath: '/home/florian/Documents/school/magic-mirror/project-env/bin/python',
				scriptPath: '/home/florian/Documents/school/MagicMirror/modules/MMM-facialrec'
			}
		},
		
	]

};

/*************** DO NOT EDIT THE LINE BELOW ***************/
if (typeof module !== "undefined") {module.exports = config;}
