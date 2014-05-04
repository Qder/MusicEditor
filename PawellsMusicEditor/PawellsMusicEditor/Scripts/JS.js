(function () {
    $(document).foundation();

    var audioArray = document.getElementsByClassName("playsong");
    var i = 0;
    var nowPlaying = audioArray[i];
    var intv;
    var s = 0; //seconds
    var m = 0; //minutes
    nowPlaying.load();

    $(".play").on("click", function () {
        nowPlaying.volume = $("#volume").slider("value");
        nowPlaying.play();
        $("#currentTimeSlider").slider({
            max: nowPlaying.duration
        });
        callMeta();
        intv = setInterval(update, 500);
    });

    function update() {
        $("#currentTime").html(millisToSec(nowPlaying.currentTime));
        $("#currentTimeSlider").slider({
            value: nowPlaying.currentTime
        })
    }

    function millisToSec(ms) {

        m = Math.floor(ms / 60);
        if (m >= 1) {
            s = Math.floor(ms) - m * 60;
        }
        else {
            s = Math.floor(ms);
        }

        if (s < 10 && m == 0) {
            return "00:0" + s;
        }
        if (s >= 10 && m == 0) {
            return "00:" + s;
        }
        if (s < 10 && m < 10 && m != 0) {
            return "0" + m + ":0" + s;
        }
        if (s >= 10 && m < 10 && m != 0) {
            return "0" + m + ":" + s;
        }
        if (s >= 10 && m >= 10) {
            return m + ":" + s;
        }

    }

    $(".stop").on("click", function () {
        nowPlaying.pause();
        nowPlaying.currentTime = 0;
        $("#currentTimeSlider").slider({
            value: 0
        })
        $("#currentTime").html();
        clearInterval(intv);
    })

    $(".pause").on("click", function () {
        nowPlaying.pause();
        clearInterval(intv);
    })

    $(".next").on("click", function () {
        if (audioArray[i+1] != null) {
            $.each($("audio.playsong"), function () {
                this.pause();
            });
            clearInterval(intv);
            nowPlaying.currentTime = 0;

            i++;
            nowPlaying = audioArray[i];
            nowPlaying.volume = 0.5;
            nowPlaying.play();
            $("#currentTimeSlider").slider({
                max: nowPlaying.duration
            });
            callMeta();
            intv = setInterval(update, 500);
        }
    })

    $(".previous").on("click", function () {
        if (i - 1 >= 0) {
            $.each($("audio.playsong"), function () {
                this.pause();
            });
            clearInterval(intv);
            nowPlaying.currentTime = 0;
            i--;
            nowPlaying = audioArray[i];
            nowPlaying.volume = 0.5;
            nowPlaying.play();
            $("#currentTimeSlider").slider({
                max: nowPlaying.duration
            });
            callMeta();
            intv = setInterval(update, 500);
        }
    });

    function callMeta() {
        var trackTitle = $(nowPlaying).attr("data-songtitle");
        $(".songtitle").html(trackTitle);
    }

    $("#volume").slider({
        min: 0.0,
        max: 1.0,
        step: 0.01,
        value: 0.5,
        orientation: "vertical",
        change: changeVolume
    });

    function changeVolume() {
        nowPlaying.volume = $("#volume").slider("value");
    }

    $("#currentTimeSlider").slider({
        min: 0,
        step: 0.01,
        value: nowPlaying.currentTime,
        slide: changeTime
    })

    function changeTime() {
        nowPlaying.currentTime = $("#currentTimeSlider").slider("value");
    }

    

})();
