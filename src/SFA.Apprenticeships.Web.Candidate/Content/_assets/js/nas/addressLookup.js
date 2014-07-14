$(document).ready(function() {
});

// provides the matching addresses from postcode
(function ($) {
    $.fn.addressLookup = function (options) {

        var settings = $.extend({ url: '' }, options);

        this.click(function (e) {
            e.preventDefault();
            getAddresses($("#post-code").val());
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
                            var opt = $('<option/>')
                                            .val(item)
                                            .html(item.AddressLine1)
                                            .addClass("address-select-option")
                                            .data("address-line2", item.AddressLine2)
                                            .data("address-line3", item.AddressLine2)
                                            .data("address-line4", item.AddressLine2)
                                            .data("post-code", item.AddressLine2)
                                            .data("uprn", item.Uprn)
                                            .data("lat", item.Latitude)
                                            .data("lon", item.Longitude);
                            addressList.append(opt);
                        });

                        $("#address-list").removeClass("toggle-content");
                    }
                },
                error: function (error) {
                    //Ignore, could be proxy issues so will work as 
                    //non-JS version.
                    console.log(error);
                }
            });
            return true;
        };
    };
})(jQuery);