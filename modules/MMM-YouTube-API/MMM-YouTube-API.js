Module.register("MMM-YouTube-API", {
    defaults: {
        thumbnailUrl: '',
        videoID: "tkgjOfJaCRQ",
        videoLink: null,
        playbackRate: "1",
        volume: "100",
        height: "360",
        width: "480",
        loop: "true",
        videoHeight: "80vh",
        imgHeight: "40vh",
        updateInterval: 1 * 60 * 60 * 1000, //Once an hour
        autoPlay: true,
    },



    start: function () {

        this.videoRef;
        this.showingVideo = false;
        this.embedVid = "https://www.youtube.com/embed/"

    },

    playVideo() {
        if (this.videoRef) {
            this.videoRef.contentWindow.postMessage('{"event":"command","func":"' + 'playVideo' + '","args":""}', '*');
        }
    },

    pauseVideo() {
        if (this.videoRef) {
            this.videoRef.contentWindow.postMessage('{"event":"command","func":"' + 'pauseVideo' + '","args":""}', '*');
        }
    },

    getDom: function () {
        const container = document.createElement("div");
        container.classList.add("yt-container");

        if (this.showingVideo) {
            const video = document.createElement("iframe");
            if (this.config.videoLink != null) {
                if (this.config.videoLink.contains("embed")) {
                    video.src = this.config.videoLink + "?modestbranding=1&controls=0&enablejsapi=1&iv_load_policy=3&showinfo=0" + (this.config.autoPlay ? "&autoplay=1" : "")
                }
                else {
                    video.src = this.embedVid + this.config.videoID + "?modestbranding=1&controls=0&enablejsapi=1&iv_load_policy=3&showinfo=0" + (this.config.autoPlay ? "&autoplay=1" : "")
                }
            }
            else {
                video.src = this.embedVid + this.config.videoID + "?modestbranding=1&controls=0&enablejsapi=1&iv_load_policy=3&showinfo=0" + (this.config.autoPlay ? "&autoplay=1" : "")
            }
            video.setAttribute("frameborder", "0");
            video.setAttribute("allow", "autoplay");
            video.style.height = this.config.videoHeight;
            video.style.width = "50%";
            video.classList.add("yt-video");
            this.videoRef = video;

            container.appendChild(video);
        }
        else {
            //Put in picture
            //const title = document.createElement("h5");
            const image = document.createElement("img");
            image.src = this.config.thumbnailUrl;
            image.style.height = this.config.imgHeight;
            image.classList.add("yt-image");
            this.imageRef = image;
            this.videoRef = false;

            //container.appendChild(title);
            //container.appendChild(spacer);
            container.appendChild(image);
        }

        return container;
    },

    switchToPicture() {
        this.showingVideo = false;
        this.updateDom();
    },

    switchToVideo() {
        this.showingVideo = true;
        this.updateDom();
    },
    playVideo() {
        if (this.videoRef) {
            console.log("Playing")
            this.videoRef.contentWindow.postMessage('{"event":"command","func":"' + 'playVideo' + '","args":""}', '*');
        }
    },

    pauseVideo() {
        if (this.videoRef) {
            this.videoRef.contentWindow.postMessage('{"event":"command","func":"' + 'pauseVideo' + '","args":""}', '*');
        }
    },

    notificationReceived: function (notification, payload) {
        if (notification === "YT_PLAY_VIDEO") {
            this.switchToVideo();
        } else if (notification === "YT_SHOW_IMAGE") {
            this.switchToPicture();
        } else if (notification === "YT_PAUSE_VIDEO") {
            this.pauseVideo();
        } else if (notification === "YT_UNPAUSE_VIDEO") {
            this.playVideo();
        } else if (notification === "YT_NEW_IMAGE") {
            this.selectEpisode(payload);
        }
    }


});