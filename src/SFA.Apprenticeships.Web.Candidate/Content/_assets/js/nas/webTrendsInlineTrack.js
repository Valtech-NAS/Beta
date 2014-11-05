$(document).ready(function () {
    //Fires inline Webtrends tracking events
    //Needs to happen after both jquery and Webtrends have loaded
    $(".webtrendsTrack").each(function (e) {
        var args = $.parseJSON($(".webtrendsTrack").attr("webTrendsMultiTrackArgs"));
        args.element = this;

        _.delay(function (args) {
            //Needs delay to esnure Webtrend object is processed.
            Webtrends.multiTrack(args);
        }, 200, args);        
    });
});
