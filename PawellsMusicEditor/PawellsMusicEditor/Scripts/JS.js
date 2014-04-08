jQuery(document).ready(function(){
    var audioArray = document.getElementsByClassName("playsong");
    var i = 0;
    var nowPlaying = audioArray [i];
    nowPlaying.load();

    $(".play").on("click", function () {
        nowPlaying.play();
    })

    $(".stop").on("click", function () {
        nowPlaying.pause();
    })

    $(".next").on("click", function () {
        $.each($("audio.playsong"), function () {
            this.pause();
        });
        i++;
        nowPlaying = audioArray[i];
        nowPlaying.load();
        nowPlaying.play();
    })
})
