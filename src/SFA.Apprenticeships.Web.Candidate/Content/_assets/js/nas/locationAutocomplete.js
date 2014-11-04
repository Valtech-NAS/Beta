// provides the matching for location town/city or postcode
(function($) {
    $.fn.locationMatch = function(options) {
        var self = $(this);
        var settings = $.extend({ delay: 300, minLength: 3, maxListSize: 25, url: '', longitude: null, latitude: null, latlonhash: null }, options);
        var tags = [];
        var locations;

        var location = function(name) {
            this.Name = name;
            this.Longitude = null;
            this.Latitude = null;
        };

        var clearLatLonFields = function() {
            if (settings.longitude != null) {
                $(settings.longitude).val(null);
            }
            if (settings.latitude != null) {
                $(settings.latitude).val(null);
            }
        };

        var isNavigationKeyCode = function (keyCode) {
            var navigationKeyCodes = [
                $.ui.keyCode.UP,
                $.ui.keyCode.DOWN,
                $.ui.keyCode.ESCAPE,
                $.ui.keyCode.ENTER,
                $.ui.keyCode.TAB,
                $.ui.keyCode.PAGE_UP,
                $.ui.keyCode.PAGE_DOWN,
                $.ui.keyCode.HOME,
                $.ui.keyCode.END
            ];

            return $.inArray(keyCode, navigationKeyCodes) !== -1;
        };

        self.keyup(function (event) {
            if (!isNavigationKeyCode(event.keyCode)) {
                // Consider non-navigation keyCodes to be user input.
                clearLatLonFields();
            }
        });

        self.autocomplete({
            delay: settings.delay,
            minLength: settings.minLength,
            source: function (request, response) {
                var digits = /[0-9]+/;

                if (request.term.match(digits)) {
                    // Search term looks like a postcode.
                    this.close();
                    return;
                }

                getLocationResults(response, request.term);
            },
            select: function(event, ui) {
                this.value = ui.item.value;
                var longLat = getLonLatFromName(ui.item.value);

                if (settings.latlonhash != null) {
                    $(settings.latlonhash).val(0);
                }

                if (settings.longitude != null) {
                    $(settings.longitude).val(longLat.Longitude);
                }
                if (settings.latitude != null) {
                    $(settings.latitude).val(longLat.Latitude);
                }
            },
            change: function(event, ui) {
                if (ui.item == null) {
                    clearLatLonFields();
                }
            },
        });

        function getLocationResults(callback, term) {
            jQuery.support.cors = true;
            $.ajax({
                url: settings.url,
                type: 'GET',
                data: { term: term },
                success: function(response) {
                    tags = [];
                    locations = response;
                    if (response != null) {
                        $.each(response, function (i, item){
                            if (i < settings.maxListSize) {
                                tags.push(item.Name);
                            } 
                        });
                    }
                    callback(tags);
                },
                error: function (error) {
                    //Ignore, could be proxy issues so will work as 
                    //non-JS version.
                }
            });
            return true;
        };

        function getLonLatFromName(name) {
            if (locations != null) {
                var index = $.inArray(name, tags);
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