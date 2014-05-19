(function () {
    $(document).foundation();

    var audioArray = document.getElementsByClassName("playsong");
    var i = 0;
    var nowPlaying = audioArray[i];
    var intv;
    var s = 0; //seconds
    if (nowPlaying != null) {
        nowPlaying.load();
    }

    var songsCount = $("#selectable").length;

    function playSong(song)
    {
        $.each($("audio.playsong"), function () {
            this.pause();
        });
        clearInterval(intv);

        selectSong(song);
        nowPlaying = audioArray[song];
        nowPlaying.volume = 0.5;
        nowPlaying.play();
        $("#currentTimeSlider").slider({
            min: 0,
            max: nowPlaying.duration,
            step: 0.01,
            value: nowPlaying.currentTime,
            slide: changeTime
        });
        callMeta();
        intv = setInterval(update, 500);

        $("#range-slider").slider({
            range: true,
            min: 0.0,
            max: nowPlaying.duration,
            step: 0.01,
            values: [0.0, nowPlaying.duration],
            slide: function (event, ui) {
                $("#timeFrom").html(millisToSec(ui.values[0]));
                $("#timeTo").html(millisToSec(ui.values[1]));
            }
        });
    }

    $(".play").on("click", function () {
        playSong(i);
    });


    function update() {
        $("#currentTime").html(millisToSec(nowPlaying.currentTime));
        $("#currentTimeSlider").slider({
            value: nowPlaying.currentTime
        });
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

    

    function selectSong(song)
    {
        i = song;
        $("#selectable > li").removeClass("ui-selected");
        $("#selectable > li").eq(song).addClass("ui-selected");
    }

    $(".stop").on("click", function () {
        nowPlaying.pause();
        nowPlaying.currentTime = 0;
        $("#currentTimeSlider").slider({
            value: 0
        })
        $("#currentTime").html();
        clearInterval(intv);
        update();
    })

    $(".pause").on("click", function () {
        nowPlaying.pause();
        clearInterval(intv);
    })

    $(".next").on("click", function () {
        if (audioArray[i + 1] != null) {
            nowPlaying.currentTime = 0;
            playSong(++i);
        }
    })

    $(".previous").on("click", function () {
        if (i - 1 >= 0) {
            nowPlaying.currentTime = 0;
            playSong(--i);
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

    

    function changeTime() {
        nowPlaying.currentTime = $("#currentTimeSlider").slider("value");
    }

    $("#File").on("change", function () {
        $(this).parent("form").submit();
    });
    
    $("#button-to-upload").on("click", function (e) {
        e.preventDefault();
        $("#File").trigger("click");
    })

    //------------2-row---------------------

    var selected = false;

    $("#selectable").selectable({
        selected: function (event, ui) {

            selected = true;

            var songId = ui.selected.children[0].attributes[2].nodeValue;
            var songTitle = ui.selected.children[0].attributes[1].nodeValue;
            var c = ui.selected.attributes[0].nodeValue;

            i = c;
            
            $(".edit-button-group .edit-button").removeClass("disabled");
            //REMOVE
            $("#button-to-remove").on("click", function (event) {
                event.preventDefault();
                var link = $(this).attr("href");
                if (selected == true) {
                    $.ajax({
                        type: "GET",
                        url: link,
                        data: { id: songId, songTitle:songTitle }
                    }).done(function () {
                        console.log("deleted");
                        location.reload(true);
                    }).fail(function () {
                        console.log("failed");
                    });
                }
            });
            //CROP

            $("#crop").on("click", function (e) {
                e.preventDefault();
                $("#TimeAndSlider").removeClass("hidden");
            });

            $("#range-slider").slider({
                range: true,
                min: 0.0,
                max: nowPlaying.duration,
                step: 0.01,
                values: [0.0, nowPlaying.duration],
                slide: function (event, ui) {
                    $("#timeFrom").html(millisToSec(ui.values[0]));
                    $("#timeTo").html(millisToSec(ui.values[1]));
                }
            });

            $("#crop-button").on("click", function (e) {
                e.preventDefault();
                var fromValue = Math.floor($("#range-slider").slider("values")[0]);
                var toValue = Math.floor(nowPlaying.duration - $("#range-slider").slider("values")[1]);

                if (selected == true) {
                    var href = $(this).attr("href");
                    $.ajax({
                        url: href,
                        type: "GET",
                        data: { path: songTitle, from: fromValue, to: toValue }
                    }).done(function () {
                        console.log("croped");
                        location.reload(true);
                    }).fail(function () {
                        console.log("failed");
                    });
                }
            });
            // save file
            $("#saveFile").attr("href", "/MusicEditor/Download?fileName=" + songTitle);


            //Filters
            $("#filter").on("click", function (e) {
                e.preventDefault();
                $("#LowPassFilter").removeClass("hidden");
                $("#HighPassFilter").removeClass("hidden");
            });

            //LowPassFilter
            $("#LowPassFilter").attr("href", "/MusicEditor/LowPassFilter?path=" + songTitle);

            //LowPassFilter
            $("#HighPassFilter").attr("href", "/MusicEditor/HighPassFilter?path=" + songTitle);
        }
    });

    $(".edit-button-group .edit-button").on('click', function (e) {
        if (selected == false) {
            e.preventDefault();
        }
    });


})();
