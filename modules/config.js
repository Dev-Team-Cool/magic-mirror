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
		{
			module: "MMM-Page-Selector",
			position: "top_center",
			config: {
				"defaultPage": "Detection",
				"displayTitle": false,
				"selectPageNotif": [],
				"incrementPageNotif": ["PAGE_UP"],
				"decrementPageNotif": ["PAGE_DOWN"]
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
		{
			module: "MMM-GoogleAssistant",
			position: "fullscreen_above",
			config: {
				micConfig: { // put there configuration generated by auto-installer
					recorder: "arecord",
					device: "plughw:0",
				},
				snowboy: {
					audioGain: 2.0,
					Frontend: true,
					Model: "computer",
					Sensitivity: null
				},
			}
		},
		{
			module: "MMM-googleCalendar",
			position: "top_left"
		},
		{
			module: "MMM-page-indicator",
			position: "bottom_bar",
			config: {
				pages: 5,
			}
		},
		{
			module: "alert",
		},
		{
			module: "updatenotification",
			position: "top_bar"
		},
		{
			module: "clock",
			position: "top_left"
		},
		{
			module: "currentweather",
			position: "top_right",
			config: {
				location: "Gent",
				locationID: "2797657", //ID from http://bulk.openweathermap.org/sample/city.list.json.gz; unzip the gz file and find your city
				appid: "bb0b98d94b0cfa95ac0085eff642dfac"
			}
		},
		{
			module: 'MMM-JokeAPI',
			position: 'bottom_bar',
			config: {
				category: "Programming"
			}
		},
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
		// {
		// 	module: 'MMM-Hello-Mirror',
		// 	position: 'lower_third',
		// 	config: {
		// 		// See 'Configuration options' for more information.
		// 		language: 'en',
		// 		voice: 'US English Female',
		// 	}
		// },
		{
			module: "MMM-EyeCandy",
			position: "top_center",
			config: {
				maxWidth: "75%",       // Sizes the images. Retains aspect ratio.
				//style: '15',            // Style number or use ownImagePath to override style
				ownImagePath: 'modules/MMM-EyeCandy/pix/logoML6.png',      // ex: 'modules/MMM-EyeCandy/pix/logoML6.png', or internet url to image
			}
		},
		{
			module: 'MMM-QRCode',
			config: {
				text: 'https://ml6.eu/',
				showRaw: false,
				imageSize: 450,
			}
		},
		{
			module: 'MMM-YouTube-API',
			position: 'middle_center',
			config: {
				videoID: 'tkgjOfJaCRQ',
				width: 960,
				height: 540,
				thumbnailUrl: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fmiro.medium.com%2Fmax%2F1000%2F1*x7P7gqjo8k2_bj2rTQWAfg.jpeg&f=1&nofb=1'
			},
		},
		{
			module: 'MMM-GoogleMapsTraffic',
			position: 'top_center',
			config: {
				key: 'AIzaSyBuFcMx75HSX_cte6cQfpAVhoYZBweAycY',
				lat: 51.0374114,
				lng: 3.7044893,
				zoom: 11,
				height: '800px',
				width: '500px',
				styledMapType: 'night',
				disableDefaultUI: true,
				backgroundColor: 'hsla(0, 0%, 0%, 0)',
				markers: [
					{
						lat: 51.0374114,
						lng: 3.7044893,
						fillColor: '#9966ff'
					},
				],
			},
		},
		{
			module: "MMM-NMBS-Connection",
			position: "bottom_right",
			config: {
				from: "Gent-Sint-Pieters",
				humanizeDuration: false,
				language: "en",
				results: 10,
				showStationNames: true,
				to: "Brussels-South",
				all: false
			}
		},
		{
			module: 'MMM-MyCommute',
			position: 'bottom_right',
			config: {
			  apikey: 'AIzaSyAsLMvF5YCyhCmCNfOjch50-8yAAmn84YA',
			  origin: 'Esplanade Oscar Van De Voorde 1, 9000 Gent, Belgium',
			  startTime: '00:00',
			  endTime: '23:59',
			  hideDays: [0,6],
			  destinations: [
				{
				  destination: 'Landergemstraat 21, 8570 Anzegem, Belgium',
				  label: 'Home',
				  mode: 'driving',
				  color: '#FF9900'
				}
			  ]
			}
		},
		// {
		// 	module: "MMM-smilerec",
		// 	position: "top_left",
		// 	config: {
		// 		pythonPath: '/home/florian/Documents/school/magic-mirror/project-env/bin/python',
		// 		scriptpath: '/home/florian/Documents/school/magic-mirror/python/smilerec'
		// 	}
		// }
	],
	pages: {
		Home: {
			"MMM-page-indicator": "bottom_bar",
			"MMM-EyeCandy": "middle_center",
			"MMM-JokeAPI": "bottom_center",
			"currentweather": "top_right"
		},
		Commuting: {
			"MMM-page-indicator": "bottom_bar",
			"MMM-NMBS-Connection": "top_left",
			"MMM-GoogleMapsTraffic": "top_right",
			// "MMM-MyCommute": "top_right",
		},
		QrCode: {
			"MMM-page-indicator": "bottom_bar",
			"MMM-QRCode": "middle_center"
		},
		Video: {
			// "MMM-Screencast": "bottom_right",
			"MMM-page-indicator": "bottom_bar",
			"MMM-YouTube-API": "middle_center"
		},
		SmileRec: {
			// "MMM-Screencast": "bottom_right",
			"MMM-page-indicator": "bottom_bar",
			// "MMM-MoodDetection": "middle_center",
			"MMM-smilerec": "top_left"
		},
		Detection: {
		},
		Personal: {
			"MMM-googleCalendar": "top_left",
			"MMM-NMBS-Connection": "bottom_right",
			"MMM-MyCommute": "bottom_right",
			"MMM-JokeAPI": "bottom_center",
		}
	},
	exclusions: {
		"MMM-facialrec": "top_right",
		"clock": "top_left",
		"MMM-Hello-Mirror": "lower_third",
		"MMM-GoogleAssistant": "fullscreen_above"
	}

};

/*************** DO NOT EDIT THE LINE BELOW ***************/
if (typeof module !== "undefined") { module.exports = config; }