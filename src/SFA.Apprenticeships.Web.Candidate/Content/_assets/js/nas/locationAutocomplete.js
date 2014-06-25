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
            },
            change: function(event, ui) {
                if (ui.item == null) {
                    if (settings.longitude != null) {
                        $(settings.longitude).val(null);
                    }
                    if (settings.latitude != null) {
                        $(settings.latitude).val(null);
                    }
                }
            },
        });

        function getLocationResults(callbackImp, term) {
            jQuery.support.cors = true;
            $.ajax({
                url: settings.url,
                type: 'GET',
                data: { term: term },
                dataType: 'jsonp',
                jsonp: 'callback',
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
                    callbackImp(tags);
                },
                error: function(error) {
                    tags = ['invalid'];
                    callbackImp(tags);
                }
            });
            //return true;
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