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
            $("#address-details").removeClass("toggle-content");
            $("#address-manual").addClass("hidden");
        }
    });

    $(".address-item").change(function() {
        $("#Address_Uprn").val("");
        $("#Address_GeoPoint_Latitude").val("");
        $("#Address_GeoPoint_Longitude").val("");
    });

    $("#address-manual-btn").click(function (e) {
        e.preventDefault();
        $("#address-details").show();
    });
});

// provides the matching addresses from postcode
(function ($) {

    $.fn.addressLookup = function (options) {
        var self = this;
        var settings = options;

        self.click(function (e) {
            e.preventDefault();
            $("#address-manual").removeClass("hidden");
            $("#address-list").addClass("toggle-content");
            $("#address-details").addClass("toggle-content");
            getAddresses($("#postcode-search").val());
        });

        function getAddresses(postcode) {
            jQuery.support.cors = true;
            $.ajax({
                url: settings.url,
                type: 'GET',
                data: { postcode: postcode },
                success: function (response) {
                    if (response != null) {
                        var addressList = $("#address-select");
                        addressList.empty();

                        var opt = $('<option/>').val("").html("-- " + response.length + " found --").addClass("address-select-option");
                        addressList.append(opt);

                        $.each(response, function (i, item) {
                            var displayVal = item.AddressLine1;
                            if (item.AddressLine2 && item.AddressLine2 != "") {
                                displayVal += ", " + item.AddressLine2;
                            }
                            var opt = $('<option/>')
                                            .val(item.Uprn)
                                            .html(displayVal)
                                            .addClass("address-select-option")
                                            .attr("data-address-line1", _.escape(item.AddressLine1))
                                            .attr("data-address-line2", _.escape(item.AddressLine2))
                                            .attr("data-address-line3", _.escape(item.AddressLine3))
                                            .attr("data-address-line4", _.escape(item.AddressLine4))
                                            .attr("data-post-code", _.escape(item.Postcode))
                                            .attr("data-lat", item.GeoPoint.Latitude)
                                            .attr("data-lon", item.GeoPoint.Longitude);
                            addressList.append(opt);
                        });

                        $("#address-list").removeClass("toggle-content");
                    }
                },
                error: function (error) {
                    //Ignore, could be proxy issues so will work as 
                    //non-JS version.
                    //console.log(error);
                }
            });
            return true;
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
                success: function(response) {
                    $('#email-available-message').html('<p> Username is available: ' + response.usernameIsAvailable + '</p>');
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