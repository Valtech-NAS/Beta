// Hooks up the pagination control.
(function () {
    $(".pagination-container a").on("click", function () {
        var container = $(this.attributes["data-ajax-update"].value);
        var method = this.attributes["data-ajax-method"].value;
        $.ajax({
            url: this.href,
            type: method,
            success: function (response) {
                container.html(response);
            },
            error: function (error) {

            }
        });

        return false;
    });
})(jQuery);