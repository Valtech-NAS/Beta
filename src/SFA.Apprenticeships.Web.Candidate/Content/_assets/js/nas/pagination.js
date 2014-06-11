// Hooks up the pagination control.
(function () {
    $("a.page-navigation__btn").on("click", function (e) {
        var pagenavigation = $("div.page-navigation");
        var container = pagenavigation.attr("data-ajax-update");
        var method = pagenavigation.attr("data-ajax-method");
        $.ajax({
            url: this.href,
            type: method,
            success: function (response) {
                $(container).html(response);
            },
            error: function (error) {

            }
        });

        e.preventDefault();
    });
})(jQuery);