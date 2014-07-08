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
        settings.showErrors = function (errorMap, errorList) {
            //See http://stackoverflow.com/questions/7935568/jquery-validation-show-validation-summary-during-eager-validation
            this.defaultShowErrors();
            var errors = $(".field-validation-error span");

            if (errors && errors.length == 0) {
                $(".validation-summary-errors > div.panel-body > ul").empty();
                $(".validation-summary-errors").removeClass("validation-summary-errors").addClass("validation-summary-valid");
                return;
            }

            var errorSummary = "";
            $.each(errors, function(index, error) {
                var li = "<li><a href='#" + $(error).attr("for").toLowerCase()  + "'>" + $(error).html() + "</a></li>";
                errorSummary += li;
            });
            
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");
            $(".validation-summary-errors > div.panel-body > ul").empty();
            $(".validation-summary-errors > div.panel-body > ul").html(errorSummary);
        };
    });
});