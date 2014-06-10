// provides the matching for location town/city or postcode
(function ($) {
    $.fn.locationMatch = function (options) {

        var settings = $.extend({ delay: 300, minLength: 3, maxListSize: 25, url: '' }, options);
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
                        var i = 0;
                        response.forEach(function (item) {
                            if (i++ < settings.maxListSize) {
                                tags.push(item.Name);
                            } else {
                                return;
                            }
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