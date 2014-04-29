(function () {
    $(document).foundation();

    var audioArray = document.getElementsByClassName("playsong");
    var i = 0;
    var nowPlaying = audioArray[i];
    var intv;
    nowPlaying.load();

    $(".play").on("click", function () {
        nowPlaying.volume = $("#volume").slider("value")
        nowPlaying.play();
        callMeta();
        intv = setInterval(update, 100);
        $("#currentTimeSlider").slider({
            max: nowPlaying.duration
        })
    })

    function update() {
        $("#currentTime").html(millisToSec(nowPlaying.currentTime));
        $("#currentTimeSlider").slider({
            value: nowPlaying.currentTime
        })
    }

    function millisToSec(ms) {
        return Math.floor(ms);
    }

    $(".stop").on("click", function () {
        nowPlaying.pause();
        nowPlaying.currentTime = 0;
        $("#currentTimeSlider").slider({
            value: 0
        })
        $("#currentTime").html(0);
        clearInterval(intv);
    })

    $(".pause").on("click", function () {
        nowPlaying.pause();
        clearInterval(intv);
    })

    $(".next").on("click", function () {
        $.each($("audio.playsong"), function () {
            this.pause();
        });
        i++;
        nowPlaying = audioArray[i];
        nowPlaying.load();
        nowPlaying.play();
        callMeta();
    })

    function callMeta() {
        var trackTitle = $(nowPlaying).attr("data-songtitle");
        $(".songtitle").html(trackTitle);
    }

    $("#volume").slider({
        min: 0.0,
        max: 1.0,
        step: 0.1,
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
        slide:changeTime
    })
    
    function changeTime() {
        nowPlaying.currentTime = $("#currentTimeSlider").slider("value");
    }

})();
