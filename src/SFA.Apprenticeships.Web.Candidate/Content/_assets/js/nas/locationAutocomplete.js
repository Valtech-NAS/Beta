// provides the matching for location town/city or postcode
(function($) {
    $.fn.locationMatch = function(options) {

        var settings = $.extend({ delay: 300, minLength: 3, maxListSize: 25, url: '', longitude: null, latitude: null }, options);
        var tags = [];
        var locations;

        var location = function(name) {
            this.Name = name;
            this.Longitude = null;
            this.Latitude = null;
        };

        this.autocomplete({
            delay: settings.delay,
            minLength: settings.minLength,
            source: function(request, response) {
                getLocationResults(response, request.term);
            },
            select: function(event, ui) {
                this.value = ui.item.value;
                var longLat = getLonLatFromName(ui.item.value);

                if (settings.longitude != null) {
                    $(settings.longitude).val(longLat.Longitude);
                }
                if (settings.latitude != null) {
                    $(settings.latitude).val(longLat.Latitude);
                }
            }
        });

        function getLocationResults(callback, term) {
            jQuery.support.cors = true;

            //var data = $("form").serialize();
            $.ajax({
                url: settings.url,
                type: 'GET',
                data: { term: term },
                success: function(response) {
                    tags = [];
                    locations = response;
                    if (response != null) {
                        var i = 0;
                        response.forEach(function(item) {
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

        function getLonLatFromName(name) {

            if (locations != null) {
                var index = tags.indexOf(name);
                if (index != -1) {
                    var search = locations[index];

                    if (search != null) {
                        return search;
                    }
                }
            }

            return location(name);
        };
    };
})(jQuery);