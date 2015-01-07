$(function () {

    $("#Location").locationMatch({
        url: '@Url.Action("location", "Location")',
        longitude: '#Longitude',
        latitude: '#Latitude',
        latlonhash: '#Hash'
    });

    $('#sort-results').change(function () {

        $('#SearchAction').val("Sort");
        $("form").submit();
    });

    $('#results-per-page').change(function () {
        $('#SearchAction').val("Sort");
        $("form").submit();
    });

    $('#search-button').click(function () {
        $('#LocationType').val("NonNational");
    });

    /*Show map with radius in results*/

    var apprLatitude = Number($('#Latitude').val()),
        apprLongitude = Number($('#Longitude').val()),
        apprMiles = Number($('#loc-within').val()),
        apprZoom = 9,
        radiusCircle,
        vacancyLinks = $('.vacancy-link').toArray(),
        vacancies = [],
        vacancy = [];

    for (var i = 0; i < vacancyLinks.length; i++) {
        var lat = $(vacancyLinks[i]).attr('data-lat'),
            longi = $(vacancyLinks[i]).attr('data-lon'),
            title = $(vacancyLinks[i]).html(),
            id = $(vacancyLinks[i]).attr('data-vacancy-id');

        vacancies[i] = [lat, longi, title, id];
    }

    if (apprMiles <= 40) {
        apprZoom = 7
    }

    if (apprMiles <= 30) {
        apprZoom = 8
    }

    if (apprMiles < 20) {
        apprZoom = 9
    }

    if (apprMiles < 10) {
        apprZoom = 10
    }

    if (apprMiles < 5) {
        apprZoom = 11
    }

    if (apprLatitude == 0 || apprLongitude == 0) {
        $('#map-canvas').parent().hide();
    }

    function initialize() {

        var mapOptions = {
            center: { lat: apprLatitude, lng: apprLongitude },
            zoom: apprZoom,
            panControl: false,
            zoomControl: true,
            mapTypeControl: false,
            scaleControl: false,
            streetViewControl: false,
            overviewMapControl: false,
            scrollwheel: false
        };

        var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

        var distanceCircle = {
            strokeColor: '#005ea5',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#005ea5',
            fillOpacity: 0.25,
            map: map,
            center: mapOptions.center,
            radius: apprMiles * 1609.344
        }

        radiusCircle = new google.maps.Circle(distanceCircle);

        setMarkers(map, vacancies)

    }

    function setMarkers(map, locations) {
        var image = '/Content/_assets/img/icon-location.svg';

        for (var i = 0; i < locations.length; i++) {
            var appship = locations[i];
            var myLatLng = new google.maps.LatLng(appship[0], appship[1]);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                animation: google.maps.Animation.DROP,
                icon: image,
                title: appship[2]
            });

            var vacancyID = appship[3];

            bindMarkerClick(marker, map, vacancyID);

        }

    }

    function bindMarkerClick(marker, map, vacancyID) {
        google.maps.event.addListener(marker, 'mouseover', function () {
            $('#' + vacancyID).closest('.search-results__item').css('background', '#E5E5E5').addClass('map-hover');
            $('.vacancy-link').not('#' + vacancyID).closest('.search-results__item').css('background', 'none').removeClass('map-hover');
        });
    }

    google.maps.event.addDomListener(window, 'load', initialize);

    $(document).click(function (event) {
        if (!$(event.target).closest('#map-canvas').length) {
            if ($('.search-results__item').hasClass("map-hover")) {
                $('.search-results__item').removeClass('map-hover').css('background', 'none');
            }
        }
    });

    $('#editSearchToggle').on('click', function () {
        initialize();
    });

    $(window).resize(function () {
        initialize();
    });
});