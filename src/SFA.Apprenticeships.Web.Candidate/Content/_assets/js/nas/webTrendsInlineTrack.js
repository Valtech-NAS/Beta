$(document).ready(function () {
    //Fires inline Webtrends tracking events
    //Needs to happen after both jquery and Webtrends have loaded
    $(".webtrendsTrack").each(function () {
        var args = $.parseJSON($(this).attr("webTrendsMultiTrackArgs"));
        args.element = this;
        Webtrends.multiTrack(args);
    });
});
