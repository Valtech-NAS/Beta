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

    function checkForDateError() {
        if ($('.date-input > .input-validation-error').length > 0) {
            $('.date-input').addClass('input-validation-error');
        } else {
            $('.date-input').removeClass('input-validation-error');
        }
    }

    $('.date-input').on('blur keyup', '.form-control', function () {
        setTimeout(function () {
            checkForDateError();
        }, 100);
    });

});

/*
 * This is a fix to support client side validation of checkboxes being 
 * specific values, see links below for more details
 * http://stackoverflow.com/questions/9808794/validate-checkbox-on-the-client-with-fluentvalidation-mvc-3
 * http://pastebin.com/7uzUJz71
 */
(function ($) {
    $.validator.unobtrusive.adapters.add('equaltovalue', ['valuetocompare'], function (options) {
        options.rules['equaltovalue'] = options.params;
        if (options.message != null) {
            options.messages['equaltovalue'] = options.message;
        }
    });

    $.validator.addMethod('equaltovalue', function (value, element, params) {
        if ($(element).is(':checkbox')) {
            if ($(element).is(':checked')) {
                return params.valuetocompare.toLowerCase() === 'true';
            } else {
                return params.valuetocompare.toLowerCase() === 'false';
            }
        }
        return params.valuetocompare.toLowerCase() === value.toLowerCase();
    });
})(jQuery);