$(function () {
    var geocoder;

    function initialize() {
        geocoder = new google.maps.Geocoder();
    }

    function geoFindMe() {
        var output = document.getElementById("Location"),
            latVal,
            longVal;


        if (!navigator.geolocation) {
            output.value = "Geolocation is not supported by your browser";
            return;
        }

        function success(position) {
            var latVal = position.coords.latitude,
                longVal = position.coords.longitude,
                latlng = new google.maps.LatLng(latVal, longVal);


            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        var myPostcode;

                        for (var i = 0; i < results[1].address_components.length; i++) {
                            for (var j = 0; j < results[1].address_components[i].types.length; j++) {
                                if (results[1].address_components[i].types[j] == "postal_code") {
                                    myPostcode = results[1].address_components[i].long_name;
                                    break;
                                }
                            }
                        }

                        output.value = myPostcode;

                    } else {
                        alert('No results found');
                    }
                } else {
                    alert('Geocoder failed due to: ' + status);
                }
            });
        };

        function error() {
            output.value = "Unable to retrieve your location";
        };

        output.value = "Locating…";

        navigator.geolocation.getCurrentPosition(success, error, { maximumAge: 600000 });

    }

    $('.geolocation #geoLocateContainer').append('. <span class="fake-link geolocater inl-block hide-nojs" id="getLocation">Use current location</span>');

    $('.geolocation').on('click', '#getLocation', function () {
        geoFindMe();
    });

    google.maps.event.addDomListener(window, 'load', initialize);

});

