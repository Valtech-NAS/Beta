(function () {
    var contactSettingsViewModel = function () {

        var self = this;

        self.hasQualifications = ko.observable(undefined);
        self.hasNoQualifications = ko.observable(undefined);
        self.showQualifications = ko.observable(false);
    };

    $(function () {
        var contactSettingsModel = new contactSettingsViewModel();

        contactSettingsModel.initialise(window.getContactSettingsData());

        ko.applyBindings(contactSettingsModel, document.getElementById('contactSettings'));
    });
}());