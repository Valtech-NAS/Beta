$(document).ready(function() {

    //$.validator.prototype.resetSummary = function () {
    //    var form = $(this.currentForm);
    //    form.find("[data-valmsg-summary=true]")
    //        .removeClass("validation-summary-errors")
    //        .addClass("validation-summary-valid")
    //        .find("ul")
    //        .empty();
    //    return this;
    //};

    //$.validator.setDefaults({
    //    showErrors: function (errorMap, errorList) {
    //        this.defaultShowErrors();
    //        this.checkForm();
    //        if (this.errorList.length) {
    //            $(this.currentForm).triggerHandler("invalid-form", [this]);
    //        } else {
    //            this.resetSummary();
    //        }
    //    }
    //});

    //$("form").each(function() {
    //    var validator = $.data(this, 'validator');
    //    validator.setDefaults({
    //        debug: true
    //    });
    //});

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
    });

    //$.validator.addMethod("fraction", function (value, element, param) {
    //    return value != "";
    //}, "Error message here");

    //$.validator.addClassRules("fraction", {
    //    fraction: true
    //});
});