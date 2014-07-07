$(document).ready(function() {

    $("form").each(function () {
        var validator = $.data(this, 'validator');
        var settings = validator.settings;
        var oldErrorFunction = settings.errorPlacement;
        var oldSuccessFunction = settings.success;
        settings.errorPlacement = function (error, element) {
            $(element).parent().addClass("input-validation-error");
            oldErrorFunction(error, element);
        };
        settings.success = function (label, element) {
            $(element).parent().removeClass("input-validation-error");
            oldSuccessFunction(label, element);

        };
        settings.showErrors = function(errorMap, errorList) {
            //this.defaultShowErrors();
            //$(".validation-summary-errors > div.panel-body > ul").empty();
            $.each(errorList, function(index, error) {
                //$(".validation-summary-errors > div.panel-body > ul").append("<li><a href='#" + error.element.id.toLowerCase() + "'>" + error.message + "</a></li>");
                error.message = "<b>" + error.message + "</b>";
            });
            this.defaultShowErrors();
        };
        settings.errorPlacement = function (error, element) {
            if (showErrorMessage) {
                var li = document.createElement("li");
                li.appendChild(document.createTextNode(error.html()));
                $ul.append(li);
            }
        },
    });

    //$.validator.addMethod("fraction", function (value, element, param) {
    //    return value != "";
    //}, "Error message here");

    //$.validator.addClassRules("fraction", {
    //    fraction: true
    //});
});