$(document).ready(function() {

    var settngs = $.data($('form')[0], 'validator').settings;
    var oldErrorFunction = settngs.errorPlacement;
    var oldSucessFunction = settngs.success;
    settngs.errorPlacement = function (error, element) {
        $(element).parent().addClass("input-validation-error");
        oldErrorFunction(error, element);
    };
    settngs.success = function (label, element) {
        $(element).parent().removeClass("input-validation-error");
        oldSucessFunction(label, element);
    };
});