// provides the matching for location town/city or postcode
(function ($) {
    $.fn.locationMatch = function (options) {

        var settings = $.extend({delay: 300, minLength: 3}, options);
        var tags = [];

        this.autocomplete({
            delay: settings.delay,
            minLength: settings.minLength,
            source: function(request, response) {
                getLocationResults(response, request.term);
            }
        });

        function getLocationResults(callback, term) {
            jQuery.support.cors = true;

            //var data = $("form").serialize();
            $.ajax({
                url: settings.url,
                type: 'POST',
                data: {term: term},
                success: function(response) {
                    tags = [];
                    if (response != null) {
                        response.forEach(function(item) {
                            tags.push(item.Name);
                        });
                    }
                    callback(tags);
                },
                error: function(error) {
                    tags = ['invalid'];
                    callback(tags);
                }
            });

            return true;
        };
    };
})(jQuery);