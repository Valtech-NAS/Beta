var dirtyFormDialog = (function () {

    var hasToShowMessage,
        initialFormValue,
        linkClicked = false,
        // settings is composed by:
        //  - formSelector: selector pointing to the form we want 
        //    to serialize and compare
        //  - classToExclude: class of the elements that we don't want to check for dirty
        //  - timeout: timeout to reset the hasToShowMessage variable. When the timeout
        //    finishes, we assume that an event can happen due to session timeout
        //  - confirmationMessage: confirmation message to show when leaving the page
        initialise = function(settings) {
            initialFormValue = $(settings.formSelector).serialize();
            hasToShowMessage = true;
            setTimeout(function() {
                hasToShowMessage = false;
            }, settings.timeout);
            setBeforeUnloadEvent(settings.formSelector, settings.classToExclude, settings.confirmationMessage);
            setKeyUpEvent(settings.formSelector);
            setClickEvent(settings.classToExclude);
        },
        setBeforeUnloadEvent = function(formSelector, classToExclude, confirmationMessage) {
            window.addEventListener("beforeunload", function(e) {
                if (!hasToShowMessage || linkClicked || $(e.target.activeElement).hasClass(classToExclude)) {
                    return;
                } else {
                    //https://developer.mozilla.org/en-US/docs/Web/Events/beforeunload

                    var actualFormValue = $(formSelector).serialize();

                    if (initialFormValue !== actualFormValue) {
                        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
                        return confirmationMessage; //Webkit, Safari, Chrome etc.
                    }
                    return;
                }
            });
        },
        setKeyUpEvent = function(formSelector) {
            $('input').keypress(function(e) {
                if (e.which == 13) {
                    hasToShowMessage = false;
                    $(formSelector).submit();
                    return false;
                }
            });
        },
        setClickEvent = function(classToHandle) {
            $('.' + classToHandle).on('click', function() {
                linkClicked = true;
            });
        };


    return {
        initialise: initialise
    };
})();