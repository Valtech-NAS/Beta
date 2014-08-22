(function () {

    var qualificationTypeModel = function (name) {
        var self = this;

        self.qualificationTypeName = name;
    }

    var groupedQualificationsModel = function (key) {
        var self = this;

        self.groupKey = ko.observable(key);
        self.groupItems = ko.observableArray();
    }

    groupedQualificationsModel.prototype = {
        addItem: function (item) {
            var self = this;
            self.groupItems.push(item);
        },
        removeItem: function (item) {
            var self = this;
            self.groupItems.remove(item);
        }
    }

    var listItemModel = function (itemType, itemOtherType, itemYear, itemSubject, itemGrade, itemPredicted, itemRegexPattern) {

        var self = this;

        self.itemIndex = ko.observable(0);
        self.itemRegexPattern = ko.observable(itemRegexPattern);

        self.qualificationType = ko.observable(itemType).extend({
            required: { message: "Select qualification type" }
        });
        self.otherQualificationType = ko.observable(itemOtherType).extend({
            pattern: {
                message: "'Other qualification type' contains some invalid characters",
                params: self.itemRegexPattern
            }
        });
        self.qualificationYear = ko.observable(itemYear).extend({
            required: { message: "Qualification Year is required" }, number: true
        }).extend({
            minLength: {
                message: "'Qualification Year' must be 4 digits",
                params: 4
            }
        });

        self.qualificationSubject = ko.observable(itemSubject).extend({
            required: { message: "Subject is required" }
        }).extend({
            pattern: {
                message: "'Subject' contains some invalid characters",
                params: self.itemRegexPattern
            }
        });
        self.qualificationGrade = ko.observable(itemGrade).extend({
            required: { message: "Grade is required" }
        }).extend({
            pattern: {
                message: "'Grade' contains some invalid characters",
                params: self.itemRegexPattern
            }
        });
        self.qualificationPredicted = ko.observable(itemPredicted);
        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);

        self.itemErrors = ko.validation.group(self);

        self.gradeDisplay = ko.computed(function () {
            return self.qualificationPredicted() ? self.qualificationGrade() + " (Predicted)" : self.qualificationGrade();
        }, self);
    };

    function addQualification(qualifications, typeSelected, typeOther, year, subject, grade, predicted, regex) {

        var qualificationArrays = qualifications;

        if (grade.indexOf("(Predicted)") > -1) {
            var gradeIndex = grade.indexOf("(Predicted)");
            grade = grade.slice(0, gradeIndex).trim();
        } 

        if (typeSelected === "Other") {
            if (!typeOther || 0 === typeOther.length) {

            } else {
                typeSelected = typeOther;
            }
        }

        var qualificationItemModel = new listItemModel(typeSelected, typeOther, year, subject, grade, predicted, regex);

        var match = ko.utils.arrayFirst(qualificationArrays, function (item) {
            return item.groupKey() === typeSelected;
        });

        if (!match) {
            var group = new groupedQualificationsModel(typeSelected);
            group.addItem(qualificationItemModel);
            qualificationArrays.push(group);
        } else {
            match.addItem(qualificationItemModel);
        }

        return qualificationArrays;
    }

    var qualificationViewModel = function () {

        var self = this;

        self.hasQualifications = ko.observable(undefined);
        self.hasNoQualifications = ko.observable(undefined);
        self.showQualifications = ko.observable(false);

        self.qualificationStatus = ko.computed(function () {
            return self.showQualifications() ? "block" : "none";
        }, qualificationViewModel);
        self.regexPattern = ko.observable();

        //TODO get values from config.
        self.qualificationTypes = ko.observableArray([
            new qualificationTypeModel("GCSE"),
            new qualificationTypeModel("AS Level"),
            new qualificationTypeModel("A Level"),
            new qualificationTypeModel("NVQ or SVQ Level 1"),
            new qualificationTypeModel("NVQ or SVQ Level 2"),
            new qualificationTypeModel("NVQ or SVQ Level 3"),
            new qualificationTypeModel("Other")
        ]);

        self.selectedQualification = ko.observable().extend({ required: { message: "Select qualification type" } });

        self.otherQualification = ko.observable().extend({
            required: {
                message: "Other qualification is required",
                onlyIf: function () { return (self.selectedQualification() === "Other"); }
            }
        }).extend({
            pattern: {
                message: "'Other qualification' contains some invalid characters",
                params: self.regexPattern,
                onlyIf: function () { return (self.selectedQualification() === "Other"); }
            }
        });

        self.showOtherQualification = ko.observable(false);

        self.year = ko.observable().extend({
            required: { message: "'Qualification Year' is required" }, number: true
        }).extend({
            minLength: {
                message: "'Qualification Year' must be 4 digits",
                params: 4
            }
        });

        self.subject = ko.observable().extend({
            required: { message: "Subject is required" }
        }).extend({
            pattern: {
                message: "'Subject' contains some invalid characters",
                params: self.regexPattern
            }
        });
        self.grade = ko.observable().extend({
            required: { message: "Grade is required" }
        }).extend({
            pattern: {
                message: "'Grade' contains some invalid characters",
                params: self.regexPattern
            }
        });
        self.predicted = ko.observable(false);

        self.qualifications = ko.observableArray();

        self.errors = ko.validation.group(self);

        self.displayErrors = ko.observable(false);

        self.selectedQualification.subscribe(function (selectedValue) {
            if (selectedValue === "Other") {
                self.showOtherQualification(true);
            } else {
                self.otherQualification("");
                self.showOtherQualification(false);
            }
        });

        self.removeQualification = function (qualification) {

            var match = ko.utils.arrayFirst(self.qualifications(), function (item) {
                return item.groupKey() === qualification.qualificationType();
            });

            if (match) {

                match.removeItem(qualification);
                self.reIndexQualifications(); //re-index qualifications
            }
        };

        self.addQualification = function () {

            if (self.errors().length == 0) {

                var typeSelected = self.selectedQualification();
                var typeOther = self.otherQualification();
                var year = self.year();
                var subject = self.subject();
                var grade = self.grade();
                var predicted = self.predicted();

                var result = addQualification(self.qualifications(), typeSelected, typeOther, year, subject, grade, predicted, self.regexPattern());
                self.qualifications(result);

                self.reIndexQualifications(); //re-index qualifications

                self.subject("");
                self.grade("");
                self.predicted(false);

                self.displayErrors(false);
                self.errors.showAllMessages(false);
            } else {
                self.errors.showAllMessages();
                self.displayErrors(true);
            }
        };

        self.editQualification = function (qualification) {
            qualification.readOnly(undefined);
            qualification.showEditButton(false);
        };

        self.saveQualificationItem = function (qualification) {
            if (qualification.itemErrors().length == 0) {
                qualification.readOnly('readonly');
                qualification.showEditButton(true);
            } else {
                qualification.itemErrors.showAllMessages();
            }
        };

        self.getqualifications = function (data) {

            $(data).each(function (index, item) {
                var result = addQualification(self.qualifications(), item.QualificationType, "", item.Year, item.Subject, item.Grade, item.IsPredicted, self.regexPattern());
                self.qualifications(result);
            });

            if (self.qualifications().length > 0) {
                self.reIndexQualifications(); //re-index qualifications
                self.showQualifications(true);
                self.hasQualifications("checked");
                self.hasNoQualifications(undefined);
                self.errors.showAllMessages(false);
            } else {
                self.showQualifications(false);
                self.hasQualifications(undefined);
                self.hasNoQualifications("checked");
            }
        };

        self.reIndexQualifications = ko.computed(function () {

            var index = 0;

            ko.utils.arrayForEach(self.qualifications(), function (qualification) {

                ko.utils.arrayForEach(qualification.groupItems(), function (item) {
                    item.itemIndex(index);
                    index++;

                });
            });

            return index;

        }, self);
    };

    var monthOfTheYear = function (monthName, monthNo) {
        var self = this;

        self.monthName = monthName;
        self.monthNumber = monthNo;
    };

    ko.validation.rules['mustBeGreaterThanOrEqual'] = {
        validator: function (val, otherVal) {
            return val === otherVal || val > otherVal;
        },
        message: 'To Year must be greater than or equal to From Year'
    };
    ko.validation.registerExtenders();

    var workExperienceItemModel = function (itemEmployer, itemJobTitle, itemDuties, itemFromMonth, itemFromYear, itemToMonth, itemToYear, itemIsCurrentEmployment, itemCurrentYear, itemRegex) {

        var self = this;

        self.itemRegexPattern = ko.observable(itemRegex);

        self.itemEmployer = ko.observable(itemEmployer).extend({
            required: { message: "Employer is required" }
        }).extend({
            pattern: {
                message: "'Employer' contains some invalid characters",
                params: self.itemRegexPattern
            }
        });

        self.itemJobTitle = ko.observable(itemJobTitle).extend({
            required: { message: "Job Title is required" }
        }).extend({
            pattern: {
                message: "'Job Title' contains some invalid characters",
                params: self.itemRegexPattern
            }
        });

        self.itemMainDuties = ko.observable(itemDuties).extend({
            required: { message: "'Main duties' required" }
        }).extend({
            maxLength: {
                message: "Main duties must not exceed 200 characters",
                params: 200
            }
        }).extend({
            pattern: {
                message: "'Main duties' contains some invalid characters",
                params: self.itemRegexPattern
            }
        });

        self.itemIsCurrentEmployment = ko.observable(itemIsCurrentEmployment);
        self.itemCurrentYear = ko.observable(itemCurrentYear);

        self.itemFromMonth = ko.observable(itemFromMonth).extend({ required: { message: "From month is required" } });
        self.itemFromYear = ko.observable(itemFromYear).extend({
            required: { message: "From year is required" }
        }).extend({
            number: {
                message: "'From Year' must be a number"
            }
        }).extend({
            max: {
                message: "'From Year' must not be in the future",
                params: self.itemCurrentYear
            }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= (fromYearValue - 100);
                },
                message: "'From Year' must not be less than 100 years ago",
                params: self.itemCurrentYear,
            }
        });

        self.itemToYear = ko.observable(itemToYear).extend({
            required: {
                message: "To year is required",
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            number: {
                message: "'To Year' must be a number",
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: "'To Year' must be greater than or equal to 'From Year'",
                params: self.itemFromYear,
                onlyIf: function () {
                    return (self.itemIsCurrentEmployment() === false);
                }
            }
        }).extend({
            max: {
                message: "'To Year' must not be in the future",
                params: self.itemCurrentYear,
                onlyIf: function () {
                    return (self.itemIsCurrentEmployment() === false);
                }
            }
        });

        self.itemToMonth = ko.observable(itemToMonth).extend({
            required: {
                message: "To month is required",
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function (val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: "'To Month and Year' must be after 'From Month and Year'",
                params: self.itemFromMonth,
                onlyIf: function () {
                    return (self.itemFromYear() === self.itemToYear());
                }
            }
        });

        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);

        self.toItemDateReadonly = ko.observable(undefined);

        self.itemIsCurrentEmployment.subscribe(function (selectedValue) {

            if (selectedValue === true) {
                self.itemToYear(null);
                self.toItemDateReadonly("disabled");
                self.errors.showAllMessages(false);
            } else {
                self.toItemDateReadonly(undefined);
            }
        });

        self.disableToDateIfCurrent = function() {           
            if (self.itemIsCurrentEmployment()) {              
                self.toItemDateReadonly("disabled");
            }
        };

        self.itemErrors = ko.validation.group(self);
    }

    var workExperienceViewModel = function () {

        var self = this;
        //TODO get this from config too
        self.months = ko.observableArray([
            new monthOfTheYear('Jan', 1), new monthOfTheYear('Feb', 2), new monthOfTheYear('Apr', 3),
            new monthOfTheYear('Mar', 4), new monthOfTheYear('May', 5), new monthOfTheYear('June', 6),
            new monthOfTheYear('July', 7), new monthOfTheYear('Aug', 8), new monthOfTheYear('Sept', 9),
            new monthOfTheYear('Oct', 10), new monthOfTheYear('Nov', 11), new monthOfTheYear('Dec', 12)
        ]);

        self.hasWorkExperience = ko.observable(undefined);
        self.hasNoWorkExperience = ko.observable(undefined);
        self.showWorkExperience = ko.observable(false);

        self.workExperienceStatus = ko.computed(function () {
            return self.showWorkExperience() ? "block" : "none";
        }, self);

        self.regexPattern = ko.observable();

        self.employer = ko.observable().extend({
            required: { message: "Employer is required" }
        }).extend({
            maxLength: {
                message: "'Employer' must not exceed 50 characters",
                params: 50
            }
        }).extend({
            pattern: {
                message: "'Employer' contains some invalid characters",
                params: self.regexPattern
            }
        });

        self.jobTitle = ko.observable().extend({
            required: { message: "Job Title is required" }
        }).extend({
            maxLength: {
                message: "'Job Title' must not exceed 50 characters",
                params: 50
            }
        }).extend({
            pattern: {
                message: "'Job Title' contains some invalid characters",
                params: self.regexPattern
            }
        });

        self.mainDuties = ko.observable().extend({
            required: { message: "'Main duties' required" }
        }).extend({
            maxLength: {
                message: "'Main duties' must not exceed 200 characters",
                params: 200
            }
        }).extend({
            pattern: {
                message: "'Main duties' contains some invalid characters",
                params: self.regexPattern
            }
        });

        self.isCurrentEmployment = ko.observable(false);
        self.currentYear = ko.observable();

        self.fromMonth = ko.observable().extend({ required: { message: "'From Month' is required" } });
        self.fromYear = ko.observable().extend({
            required: { message: "'From Year' is required" }, number: true
        }).extend({
            max: {
                message: "'From Year' must not be in the future",
                params: self.currentYear
            }
        }).extend({
            minLength: {
                message: "'From Year' must be 4 digits",
                params: 4
            }
        });

        self.toMonth = ko.observable().extend({
            required: {
                message: "To month is required",
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function (val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: "'To Month and Year' must be after 'From Month and Year'",
                params: self.fromMonth,
                onlyIf: function () {
                    return (self.fromYear() === self.toYear());
                }
            }
        });

        self.toYear = ko.observable().extend({
            required: {
                message: "To year is required",
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            number: {
                message: "'To Year' must be a number",
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            minLength: 4,
            onlyIf: function () { return (self.isCurrentEmployment() === false); }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: "'To Year' must be greater than or equal to 'From Year'",
                params: self.fromYear,
                onlyIf: function () {
                    return (self.isCurrentEmployment() === false);
                }
            }
        }).extend({
            max: {
                message: "'To Year' must not be in the future",
                params: self.currentYear,
                onlyIf: function () {
                    return (self.isCurrentEmployment() === false);
                }
            }
        });;

        self.toDateReadonly = ko.observable(undefined);

        self.isCurrentEmployment.subscribe(function (selectedValue) {
            if (selectedValue === true) {

                self.toYear(null);
                self.toDateReadonly("disabled");
            } else {
                self.toDateReadonly(undefined);
            }
        });

        self.workExperiences = ko.observableArray();

        self.errors = ko.validation.group(self);

        self.addWorkExperience = function () {

            if (self.errors().length == 0) {

                var toMonth = self.toMonth();
                var toYear = self.toYear();

                if (self.isCurrentEmployment() === true) {
                    toMonth = 0;
                    toYear = 0;
                }

                var experience = new workExperienceItemModel(self.employer(), self.jobTitle(), self.mainDuties(), self.fromMonth(), self.fromYear(), toMonth, toYear, self.isCurrentEmployment(), self.currentYear(), self.regexPattern());
                experience.disableToDateIfCurrent();
                self.workExperiences.push(experience);

                self.employer("");
                self.jobTitle("");
                self.mainDuties("");
                self.fromYear(null);
                self.toYear(null);
                self.isCurrentEmployment(false);
                self.toDateReadonly(undefined);

                self.errors.showAllMessages(false);

            } else {
                self.errors.showAllMessages();
            }

        };

        self.editWorkExperience = function (workExperience) {
            workExperience.readOnly(undefined);
            workExperience.showEditButton(false);
        };

        self.saveWorkExperience = function (workExperience) {
            if (workExperience.itemErrors().length == 0) {
                workExperience.readOnly('readonly');
                workExperience.showEditButton(true);
            } else {
                workExperience.itemErrors.showAllMessages();
            }
        };

        self.removeWorkExperience = function (workExperience) {
            self.workExperiences.remove(workExperience);
        };

        self.getWorkExperiences = function (data) {

            $(data).each(function (index, item) {

                var currentEmployer = false;
                var myToYear = item.ToYear;

                if (myToYear <= 1) {
                    currentEmployer = true;
                    myToYear = null;
                }

                var experienceItemModel = new workExperienceItemModel(item.Employer, item.JobTitle, item.Description, item.FromMonth, item.FromYear, item.ToMonth, myToYear, currentEmployer, self.currentYear(), self.regexPattern());
                experienceItemModel.disableToDateIfCurrent();
                self.workExperiences.push(experienceItemModel);
            });

            if (self.workExperiences().length > 0) {
                self.showWorkExperience(true);
                self.hasWorkExperience("checked");
                self.hasNoWorkExperience(undefined);

            } else {
                self.showWorkExperience(false);
                self.hasWorkExperience(undefined);
                self.hasNoWorkExperience("checked");
            }

        };

    };

    ko.bindingHandlers.parentvalElement = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueIsValid = valueAccessor().isValid();
            if (!valueIsValid && viewModel.isAnyMessageShown()) {
                $(element).addClass("input-validation-error");
            }
            else {
                $(element).removeClass("input-validation-error");
            }
        }
    };

    $(function () {

        var model = new qualificationViewModel();

        if (window.getWhiteListRegex()) {
            model.regexPattern(window.getWhiteListRegex());
        }

        if (window.getQualificationData()) {
            model.getqualifications(window.getQualificationData());
        }

        ko.applyBindings(model, document.getElementById('applyQualifications'));

        var experienceModel = new workExperienceViewModel();

        if (window.getCurrentYear()) {
            experienceModel.currentYear(window.getCurrentYear());
        }

        if (window.getWhiteListRegex()) {
            experienceModel.regexPattern(window.getWhiteListRegex());
        }

        if (window.getWorkExperienceData()) {
            experienceModel.getWorkExperiences(window.getWorkExperienceData());
        }

        ko.applyBindings(experienceModel, document.getElementById('applyWorkExperience'));

    });

}());