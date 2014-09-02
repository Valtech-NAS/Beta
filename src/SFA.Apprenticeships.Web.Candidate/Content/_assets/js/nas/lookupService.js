// TODO: get 'postcode' messages from C#

$(document).ready(function () {
    $("#address-select").change(function () {
        var option = $(this);
        var selected = $(this).find('option:selected');
        if (option && option.val() !== "") {
            $("#Address_AddressLine1").val(selected.attr("data-address-line1"));
            $("#Address_AddressLine2").val(selected.attr("data-address-line2"));
            $("#Address_AddressLine3").val(selected.attr("data-address-line3"));
            $("#Address_AddressLine4").val(selected.attr("data-address-line4"));
            $("#Address_Postcode").val(selected.attr("data-post-code"));
            $("#Address_Uprn").val(option.val());
            $("#Address_GeoPoint_Latitude").val(selected.attr("data-lat"));
            $("#Address_GeoPoint_Longitude").val(selected.attr("data-lon"));
        }
        //TODO: if needing validation groups this should be refactored too
        $("#Address_AddressLine1").focus();
        $("#Address_Postcode").focus();
        $("#EmailAddress").focus();
    });

    $(".address-item").change(function() {
        $("#Address_Uprn").val("");
        $("#Address_GeoPoint_Latitude").val("");
        $("#Address_GeoPoint_Longitude").val("");
    });
});

// provides the matching addresses from postcode
(function ($) {

    $.fn.addressLookup = function (options) {
        var self = this;
        var settings = options;

        var $postcodeSearchValidationError = $("#postcode-search-validation-error");
        var $addressSelect = $("#address-select");
        var $addressList = $("#address-list");
        var $findAddressButton = $('#find-addresses');

        self.click(function (e) {
            e.preventDefault();
            getAddresses($("#postcode-search").val());
        });

        var showErrorMessage = function (message) {
            $postcodeSearchValidationError.removeClass("field-validation-valid");
            $postcodeSearchValidationError.addClass("field-validation-error");
            $postcodeSearchValidationError.html(message);
        };

        var hideErrorMessage = function () {
            $postcodeSearchValidationError.removeClass("field-validation-error");
            $postcodeSearchValidationError.addClass("field-validation-valid");
            $postcodeSearchValidationError.html('');
        };

        var showFindAddressButton = function() {
            $findAddressButton.removeClass('disabled').text('Find address');
        };

        var $makeAddressOption = function (address) {
            var displayVal = address.AddressLine1;

            if (address.AddressLine2 && address.AddressLine2 !== "") {
                displayVal += ", " + address.AddressLine2;
            }

            return $('<option/>')
                .val(address.Uprn)
                .html(displayVal)
                .addClass("address-select-option")
                .attr("data-address-line1", _.escape(address.AddressLine1))
                .attr("data-address-line2", _.escape(address.AddressLine2))
                .attr("data-address-line3", _.escape(address.AddressLine3))
                .attr("data-address-line4", _.escape(address.AddressLine4))
                .attr("data-post-code", _.escape(address.Postcode))
                .attr("data-lat", address.GeoPoint.Latitude)
                .attr("data-lon", address.GeoPoint.Longitude);
        };

        var $makeAddressCountOption = function(count) {
            return $('<option/>')
                .val("")
                .html("-- " + count + " found --")
                .addClass("address-select-option");
        }

        var handleResponse = function(response) {
            if (response != null && response.length) {
                $addressSelect.empty();
                $addressSelect.append($makeAddressCountOption(response.length));

                $.each(response, function(i, address) {
                    $addressSelect.append($makeAddressOption(address));
                });

                $addressList.removeClass("toggle-content");

            } else {
                $addressList.addClass("toggle-content");
                showErrorMessage("TODO: 'Postcode' not found");
            }

            showFindAddressButton();
        };

        var handleMissingPostcode = function () {
            $addressList.addClass("toggle-content");
            showErrorMessage("TODO: 'Postcode' is required");
            showFindAddressButton();
        };

        var handleInvalidPostcode = function () {
            $addressList.addClass("toggle-content");
            showErrorMessage("TODO: 'Postcode' is invalid");
            showFindAddressButton();
        };

        var handleError = function (e) {
            // TODO: add retry message from 'Points of Failure'.
            // TODO: can we differentiate between proxy error and server-type error (e.g. postcode.io).
            console.error(e);
            showErrorMessage("TODO: service call failed - retry");
            showFindAddressButton();
        };

        var isEmptyString = function (s) {
            return !s || /^\s*$/.test(s);
        };

        var getAddresses = function (postcode) {
            jQuery.support.cors = true;

            hideErrorMessage();

            if (isEmptyString(postcode)) {
                handleMissingPostcode();
                return;
            }

            $.ajax({
                statusCode: {
                    400: handleInvalidPostcode
                },
                url: settings.url,
                type: 'GET',
                data: {
                    postcode: postcode
                },
                success: handleResponse,
                error: handleError
            });
        };
    };

    $.fn.usernameLookup = function (apiurl) {

        var self = this;

        self.focusout(function () {

            var username = $(this).val();

            $.ajax({
                url: apiurl,
                type: 'GET',
                data: { username: username },
                success: function (response) {
                    if (response.isUsernameAvailable === false) {
                        $('#email-available-message').html('<p class="text">Your email address has already been activated. Please try signing in again. If you’ve forgotten your password you can reset it.</p>');
                    } else {
                        $('#email-available-message').html('');
                    }
                },
                error: function (error) {
                    //Ignore, could be proxy issues so will work as 
                    //non-JS version.
                    //console.log(error);
                }
            });
        });

        self.focusin(function() {
            $('#display-message').html('');
        });
    };

})(jQuery);