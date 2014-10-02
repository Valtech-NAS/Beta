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
            $('#addressesFound').text('Address has been entered into the fields below');
        }
        //TODO: if needing validation groups this should be refactored too
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
        var self = this,
            settings = options;

        var $postcodeSearchValidationError = $("#postcode-search-validation-error"),
            $addressSelect = $("#address-select"),
            $addressList = $("#address-list"),
            $findAddressButton = $('#find-addresses'),
            $postCodeSearch = $('#postcode-search'),
            $ariaFoundText = $('#addressesFound');

        self.click(function (e) {
            e.preventDefault();
            getAddresses($postCodeSearch.val());
        });

        $postCodeSearch.keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);

            if (keycode == 13) {
                getAddresses($postCodeSearch.val());
                e.preventDefault();
                $findAddressButton.addClass('disabled').text('Loading');
            }

            e.stopPropagation();
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

        var handleResponse = function (response) {

            if (response.HasError) {
                showErrorMessage("Sorry, there’s a problem with the service. Please try entering your address manually.");
            } else {
                var addresses = response.Addresses;
                if (addresses != null && addresses.length) {
                    $addressSelect.empty();
                    $addressSelect.append($makeAddressCountOption(addresses.length));

                    $.each(addresses, function (i, address) {
                        $addressSelect.append($makeAddressOption(address));
                    });

                    $addressList.removeClass("toggle-content").removeAttr('hidden');

                    $ariaFoundText.text(addresses.length + ' addresses have been found, please select from the dropdown');

                } else {
                    $addressList.addClass("toggle-content").attr('hidden', true);
                    showErrorMessage("Postcode not found. Please enter address manually.");
                    $ariaFoundText.text('');
                }
            }

            showFindAddressButton();
        };

        var handleMissingPostcode = function () {
            $addressList.addClass("toggle-content").attr('hidden', true);
            showErrorMessage("Please enter a postcode");
            showFindAddressButton();
            $ariaFoundText.text('');
        };

        var handleInvalidPostcode = function () {
            $addressList.addClass("toggle-content").attr('hidden', true);
            showErrorMessage("Please enter a valid postcode");
            showFindAddressButton();
            $ariaFoundText.text('');
        };

        var handleError = function (e) {
            // TODO: can we differentiate between proxy error and server-type error (e.g. postcode.io).
            console.error(e);
            showErrorMessage("Sorry, there’s a problem with the service. Please try entering your address manually.");
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

        var $emailAvailableMessage = $('#email-available-message');

        var setErrorMessage = function () {
            $emailAvailableMessage.html('<p class="text">Your email address has already been activated. Please try signing in again. If you’ve forgotten your password you can reset it.</p>');
        };

        var cleanErrorMessage = function () {
            $emailAvailableMessage.html('');
        }

        var handleSucess = function (response) {
            if (!response.HasError) {
                if (response.IsUserNameAvailable == false) {
                    setErrorMessage();
                }
                else {
                    cleanErrorMessage();
                }
            }
        };

        var handleError = function (error) {
            //Ignore, could be proxy issues so will work as 
            //non-JS version.
            //console.log(error);
        };

        self.focusout(function () {
            var username = $(this).val();
            cleanErrorMessage();

            $.ajax({
                url: apiurl,
                type: 'GET',
                data: { username: username },
                success: handleSucess,
                error: handleError
            });
        });

        self.focusin(function() {
            $('#display-message').html('');
        });
    };

})(jQuery);