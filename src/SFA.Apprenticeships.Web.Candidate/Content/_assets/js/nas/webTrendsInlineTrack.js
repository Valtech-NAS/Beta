$(document).ready(function () {
    //Fires inline Webtrends tracking events
    //Needs to happen after both jquery and Webtrends have loaded
    $(".webtrendsTrack").each(function(e) {
        Webtrends.multiTrack({ "argsa": $.parseJSON($(".webtrendsTrack").attr("webTrendsMultiTrackArgs")) });
    });
});
