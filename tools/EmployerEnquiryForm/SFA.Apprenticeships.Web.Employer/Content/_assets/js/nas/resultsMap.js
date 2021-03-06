﻿$(function () {

    /*Show map with radius in results*/

    var apprLatitude       = Number($('#Latitude').val()),
        apprLongitude      = Number($('#Longitude').val()),
        apprMiles          = Number($('#loc-within').val()),
        resultsPage        = $('#results-per-page').val(),
        numberOfResults    = $('.vacancy-link').length,
        distanceOfLast     = $('.search-results__item:last-child .distance-value').html(),
        sortResultsControl = $('#sort-results').val(),
        apprZoom           = 9,
        radiusCircle,
        vacancyLinks       = $('.vacancy-link').toArray(),
        vacancies          = [],
        vacancy            = [],
        theMarkers         = [],
        markersFeature     = true,
        mapCenter          = { lat: apprLatitude, lng: apprLongitude };

    for (var i = 0; i < vacancyLinks.length; i++) {
        var lat = $(vacancyLinks[i]).attr('data-lat'),
            longi = $(vacancyLinks[i]).attr('data-lon'),
            title = $(vacancyLinks[i]).html(),
            id = $(vacancyLinks[i]).attr('data-vacancy-id');

        vacancies[i] = [lat, longi, title, id];
    }

    if (apprMiles <= 40) {
        apprZoom = 7;
    }

    if (apprMiles <= 30) {
        apprZoom = 8;
    }

    if (apprMiles < 20) {
        apprZoom = 9;
    }

    if (apprMiles < 10) {
        apprZoom = 10;
    }

    if (apprMiles < 5) {
        apprZoom = 11;
    }

    if (apprMiles == 0) {
        mapCenter = { lat: 52.56192833333333, lng: -1.464853888888889 };
        apprZoom = 5;
    }

    if (apprLatitude == 0 || apprLongitude == 0) {
        $('#map-canvas').parent().hide();
    }

    function initialize() {

        var mapOptions = {
            center: mapCenter,
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

        if (markersFeature) {
            setMarkers(map, vacancies)
        }

    }

    function setMarkers(map, locations) {
        var image1 = new google.maps.MarkerImage(
                        '/Content/_assets/img/icon-location.png',
                        null, /* size is determined at runtime */
                        null, /* origin is 0,0 */
                        null, /* anchor is bottom center of the scaled image */
                        new google.maps.Size(20, 32));
        var image2 = new google.maps.MarkerImage(
                        '/Content/_assets/img/icon-location-selected.png',
                        null, /* size is determined at runtime */
                        null, /* origin is 0,0 */
                        null, /* anchor is bottom center of the scaled image */
                        new google.maps.Size(20, 32));


        for (var i = 0; i < locations.length; i++) {
            var appship = locations[i];
            var myLatLng = new google.maps.LatLng(appship[0], appship[1]);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                animation: google.maps.Animation.DROP,
                icon: image1,
                title: appship[2]
            });

            theMarkers.push(marker);

            var vacancyID = appship[3];

            bindMarkerClick(marker, map, vacancyID, image1, image2);

            itemHover(image1, image2);

        }

    }

    function bindMarkerClick(marker, map, vacancyID, image1, image2) {
        google.maps.event.addListener(marker, 'mouseover', function () {
            marker.setIcon(image2);
            marker.setZIndex(1000);
            $('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item').css('background', '#E5E5E5').addClass('map-hover');
            $('.vacancy-link').not('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item').css('background', 'none').removeClass('map-hover');
        });

        google.maps.event.addListener(marker, 'click', function () {
            marker.setIcon(image2);
            marker.setZIndex(1000);
            $('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item')[0].scrollIntoView();
            $('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item').css('background', '#E5E5E5').addClass('map-hover');
            $('.vacancy-link').not('[data-vacancy-id="' + vacancyID + '"]').closest('.search-results__item').css('background', 'none').removeClass('map-hover');
        });

        google.maps.event.addListener(marker, 'mouseout', function () {
            marker.setIcon(image1);
            marker.setZIndex(0);
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

    function itemHover(image1, image2) {
        $('.search-results__item').mouseover(function () {
            var thisPosition = $(this).index();
            theMarkers[thisPosition].setIcon(image2);
            theMarkers[thisPosition].setZIndex(1000);
            $(this).css('background', '#E5E5E5').addClass('map-hover');

        });

        $('.search-results__item').mouseleave(function () {
            var thisPosition = $(this).index();
            theMarkers[thisPosition].setIcon(image1);
            theMarkers[thisPosition].setZIndex(0);

            $(this).css('background', 'none').removeClass('map-hover');
        });

    }

    $('#editSearchToggle').on('click', function () {
        initialize();
    });
});