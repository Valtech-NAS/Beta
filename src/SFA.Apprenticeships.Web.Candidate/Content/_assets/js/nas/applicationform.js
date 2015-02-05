﻿(function () {
    //Qualification Validation Messages
    var validationMessageSelectQualificationType = "Please select qualification type";
    var validationMessageOtherQualificationRequired = "Please enter other qualification";
    var validationMessageOtherQualificationContainsInvalidCharacters = "Other qualification can't contain invalid characters, eg '/'";
    var validationMessageQualificationYearRequired = "Please enter year of qualification";
    var validationMessageQualificationYearMustBeAFourDigitNumber = "Year must be 4 digits, eg 1990";
    var validationMessageQualificationYearMustBeAfter = "Year must be 4 digits, and not before " + ( new Date().getFullYear() - 100 );
    var validationMessageSubjectRequired = "Please enter subject";
    var validationMessageSubjectContainsInvalidCharacters = "Subject can't contain invalid characters, eg '/'";
    var validationMessageGradeRequired = "Please enter grade";
    var validationMessageGradeContainsInvalidCharacters = "Grade can't contain invalid characters, eg '/'";
    var validationMessageQualificationFutureYear = "Please select 'Predicted' if you're adding a grade in the future";

    //Work Experience Validation Messages
    var validationMessageEmployerRequired = "Please enter employer name";
    var validationMessageEmployerExceedsFiftyCharacters = "Employer name can't exceed 50 characters";
    var validationMessageEmployerContainsInvalidCharacters = "Employer name can't contain invalid characters, eg '/'";
    var validationMessageJobTitleRequired = "Please enter job title";
    var validationMessageJobTitleExceedsFiftyCharacters = "Job title can't exceed 50 characters";
    var validationMessageJobTitleContainsInvalidCharacters = "Job title can't contain invalid characters, eg '/'";
    var validationMessageMainDutiesRequired = "Please provide main duties";
    var validationMessageMainDutiesExceedsTwoHundredCharacters = "Main duties can't exceed 200 characters";
    var validationMessageMainDutiesContainInvalidCharacters = "Main duties can't contain invalid characters, eg '/'";
    var validationMessageFromMonthRequired = "Please enter month started";
    var validationMessageFromYearRequired = "Please enter year started";
    var validationMessageFromYearMustBeNumeric = "Year started must contain 4 digits, eg 1990";
    var validationMessageFromYearMustNotBeInFuture = "Year started can't be in the future";
    var validationMessageYearMustBeARange = "Year must be 4 digits, between " + (new Date().getFullYear() - 100) + " and " + (new Date().getFullYear());
    var validationMessageToMonthRequired = "Please enter month you finished";
    var validationMessageToYearRequired = "Please enter year finished";
    var validationMessageToYearMustBeNumeric = "Year finished must contain 4 digits, eg 1990";
    var validationMessageToYearMustNotBeInFuture = "Year finished can't be in the future";
    var validationMessageToYearMustBeAfterFromYear = 'Year finished must be after year started';
    var validationMessageYearMustBeAfter = "Year must be 4 digits, and not before 1915";
    var validationMessageDateFinishedMustBeAfterDateStarted = "Date finished must be after date started";

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

    var listItemModel = function (itemType, itemOtherType, itemYear, itemSubject, itemGrade, itemPredicted, itemRegexPattern, itemYearRegexPattern) {

        var self = this;
        self.itemIndex = ko.observable(0);
        self.itemRegexPattern = ko.observable(itemRegexPattern);
        self.qualificationPredicted = ko.observable(itemPredicted);
        self.itemYearRegexPattern = ko.observable(itemYearRegexPattern);
        self.qualificationType = ko.observable(itemType).extend({
            required: { message: validationMessageSelectQualificationType }
        });
        self.otherQualificationType = ko.observable(itemOtherType).extend({
            pattern: {
                message: validationMessageOtherQualificationContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.qualificationYear = ko.observable(itemYear).extend({
            required: { message: validationMessageQualificationYearRequired }
        }).extend({
            number: {
                message: validationMessageQualificationYearMustBeAFourDigitNumber
            }
        }).extend({
            max: {
                params: new Date().getFullYear(),
                message: validationMessageQualificationFutureYear,
                onlyIf: function () { return (self.qualificationPredicted() === false); }
            }
        }).extend({
            min: {
                message: validationMessageQualificationYearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.qualificationSubject = ko.observable(itemSubject).extend({
            required: { message: validationMessageSubjectRequired }
        }).extend({
            pattern: {
                message: validationMessageSubjectContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });
        self.qualificationGrade = ko.observable(itemGrade).extend({
            required: { message: validationMessageGradeRequired }
        }).extend({
            pattern: {
                message: validationMessageGradeContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });
        
        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);

        self.itemErrors = ko.validation.group(self);

        self.gradeDisplay = ko.computed(function () {
            return self.qualificationPredicted() ? self.qualificationGrade() + " (Predicted)" : self.qualificationGrade();
        }, self);
    };

    function addQualification(qualifications, typeSelected, typeOther, year, subject, grade, predicted, regex, yearRegex) {

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

        var qualificationItemModel = new listItemModel(typeSelected, typeOther, year, subject, grade, predicted, regex, yearRegex);

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
        self.yearRegexPattern = ko.observable();

        //TODO get values from config.
        self.qualificationTypes = ko.observableArray([
            new qualificationTypeModel("GCSE"),
            new qualificationTypeModel("AS Level"),
            new qualificationTypeModel("A Level"),
            new qualificationTypeModel("BTEC"),
            new qualificationTypeModel("NVQ or SVQ Level 1"),
            new qualificationTypeModel("NVQ or SVQ Level 2"),
            new qualificationTypeModel("NVQ or SVQ Level 3"),
            new qualificationTypeModel("Other")
        ]);

        self.selectedQualification = ko.observable().extend({ required: { message: validationMessageSelectQualificationType } });
       
        self.otherQualification = ko.observable().extend({
            required: {
                message: validationMessageOtherQualificationRequired,
                onlyIf: function () { return (self.selectedQualification() === "Other"); }
            }
        }).extend({
            pattern: {
                message: validationMessageOtherQualificationContainsInvalidCharacters,
                params: self.regexPattern,
                onlyIf: function () { return (self.selectedQualification() === "Other"); }
            }
        });

        self.showOtherQualification = ko.observable(false);

        self.year = ko.observable().extend({
            required: { message: validationMessageQualificationYearRequired }
        }).extend({
            number: {
                message: validationMessageQualificationYearMustBeAFourDigitNumber
            }
        }).extend({
           max: {
               params: new Date().getFullYear(),
               message: validationMessageQualificationFutureYear,
               onlyIf: function () { return (self.predicted() === false && self.yearValidationActivated() === true); }
           }
        }).extend({
            min: {
                message: validationMessageQualificationYearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.subject = ko.observable().extend({
            required: { message: validationMessageSubjectRequired }
        }).extend({
            pattern: {
                message: validationMessageSubjectContainsInvalidCharacters,
                params: self.regexPattern
            }
        });
        self.grade = ko.observable().extend({
            required: { message: validationMessageGradeRequired}
        }).extend({
            pattern: {
                message: validationMessageGradeContainsInvalidCharacters,
                params: self.regexPattern
            }
        });
        self.predicted = ko.observable(false);

        self.yearValidationActivated = ko.observable(false);

        self.qualifications = ko.observableArray();

        self.errors = ko.validation.group(self);

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
                $('#qualAddConfirmText').text('Qualification removed from table');
            }
        };

        self.addQualification = function () {
            self.yearValidationActivated(true);

            if (self.errors().length == 0) {

                var typeSelected = self.selectedQualification();
                var typeOther = self.otherQualification();
                var year = self.year();
                var subject = self.subject();
                var grade = self.grade();
                var predicted = self.predicted();

                var result = addQualification(self.qualifications(), typeSelected, typeOther, year, subject, grade, predicted, self.regexPattern(), self.yearRegexPattern());
                self.qualifications(result);

                $('#qualAddConfirmText').text('Qualification has been added to table below');

                self.reIndexQualifications(); //re-index qualifications

                self.subject("");
                self.grade("");
                self.predicted(false);

                self.errors.showAllMessages(false);
                self.yearValidationActivated(false);
            } else {
                self.errors.showAllMessages();
                $('#qualAddConfirmText').text('There has been a problem adding this qualification, check errors above');
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

        self.checkHasNoQualifications = function() {
            self.showQualifications(false);
            self.hasQualifications(undefined);
            self.hasNoQualifications("checked");
        };

        self.getqualifications = function (data) {

            $(data).each(function (index, item) {
                var result = addQualification(self.qualifications(), item.QualificationType, "", item.Year, item.Subject, item.Grade, item.IsPredicted, self.regexPattern(), self.yearRegexPattern());
                self.qualifications(result);
            });

            if (self.qualifications().length > 0) {
                self.reIndexQualifications(); //re-index qualifications
                self.showQualifications(true);
                self.hasQualifications("checked");
                self.hasNoQualifications(undefined);
                self.errors.showAllMessages(false);
            } else {
                self.checkHasNoQualifications();
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
        message: validationMessageToYearMustBeAfterFromYear
    };

    ko.validation.registerExtenders();

    var workExperienceItemModel = function (itemEmployer, itemJobTitle, itemDuties, itemFromMonth, itemFromYear, itemToMonth, itemToYear, itemIsCurrentEmployment, itemCurrentYear, itemRegex, itemYearRegexPattern) {

        var self = this;

        self.itemRegexPattern = ko.observable(itemRegex);

        self.itemYearRegexPattern = ko.observable(itemYearRegexPattern);

        self.itemEmployer = ko.observable(itemEmployer).extend({
            required: { message: validationMessageEmployerRequired }
        }).extend({
            maxLength: {
                message: validationMessageEmployerExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: validationMessageEmployerContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemJobTitle = ko.observable(itemJobTitle).extend({
            required: { message: validationMessageJobTitleRequired }
        }).extend({
            maxLength: {
                message: validationMessageJobTitleExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: validationMessageJobTitleContainsInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemMainDuties = ko.observable(itemDuties).extend({
            required: { message: validationMessageMainDutiesRequired }
        }).extend({
            maxLength: {
                message: validationMessageMainDutiesExceedsTwoHundredCharacters,
                params: 200
            }
        }).extend({
            pattern: {
                message: validationMessageMainDutiesContainInvalidCharacters,
                params: self.itemRegexPattern
            }
        });

        self.itemIsCurrentEmployment = ko.observable(itemIsCurrentEmployment);
        self.itemCurrentYear = ko.observable(itemCurrentYear);

        self.itemFromMonth = ko.observable(itemFromMonth).extend({ required: { message: validationMessageFromMonthRequired } });
        self.itemFromYear = ko.observable(itemFromYear).extend({
            required: { message: validationMessageFromYearRequired }
        }).extend({
            number: {
                message: validationMessageFromYearMustBeNumeric
            }
        }).extend({
            min: {
                message: validationMessageYearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            max: {
                message: validationMessageFromYearMustNotBeInFuture,
                params: self.itemCurrentYear
            }
        });

        self.itemToYear = ko.observable(itemToYear).extend({
            required: {
                message: validationMessageToYearRequired,
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            number: {
                message: validationMessageToYearMustBeNumeric,
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            min: {
                message: validationMessageYearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            max: {
                message: validationMessageToYearMustNotBeInFuture,
                params: self.itemCurrentYear,
                onlyIf: function () {
                    return (self.itemIsCurrentEmployment() === false);
                }
            }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: validationMessageToYearMustBeAfterFromYear,
                params: self.fromYear,
                onlyIf: function () {
                    return (self.itemIsCurrentEmployment() === false);
                }
            }
        });

        self.itemToMonth = ko.observable(itemToMonth).extend({
            required: {
                message: validationMessageToMonthRequired,
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function (val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: validationMessageDateFinishedMustBeAfterDateStarted,
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
            new monthOfTheYear('Jan', 1), new monthOfTheYear('Feb', 2), new monthOfTheYear('Mar', 3),
            new monthOfTheYear('Apr', 4), new monthOfTheYear('May', 5), new monthOfTheYear('June', 6),
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
        self.yearRegexPattern = ko.observable();

        self.employer = ko.observable().extend({
            required: { message: validationMessageEmployerRequired }
        }).extend({
            maxLength: {
                message: validationMessageEmployerExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: validationMessageEmployerContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.jobTitle = ko.observable().extend({
            required: { message: validationMessageJobTitleRequired }
        }).extend({
            maxLength: {
                message: validationMessageJobTitleExceedsFiftyCharacters,
                params: 50
            }
        }).extend({
            pattern: {
                message: validationMessageJobTitleContainsInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.mainDuties = ko.observable().extend({
            required: { message: validationMessageMainDutiesRequired }
        }).extend({
            maxLength: {
                message: validationMessageMainDutiesExceedsTwoHundredCharacters,
                params: 200
            }
        }).extend({
            pattern: {
                message: validationMessageMainDutiesContainInvalidCharacters,
                params: self.regexPattern
            }
        });

        self.isCurrentEmployment = ko.observable(false);

        self.currentYear = ko.observable();

        self.fromMonth = ko.observable().extend({
             required: { message: validationMessageFromMonthRequired }
        });

        self.fromYear = ko.observable().extend({
            required: { message: validationMessageFromYearRequired }
        }).extend({
             number: {
                 message: validationMessageFromYearMustBeNumeric
             }
        }).extend({
            max: {
                message: validationMessageFromYearMustNotBeInFuture,
                params: self.currentYear
            }
        }).extend({
            min: {
                message: validationMessageYearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        });

        self.toMonth = ko.observable().extend({
            required: {
                message: validationMessageToMonthRequired,
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            validation: {
                validator: function (val, fromMonthValue) {
                    return val >= fromMonthValue;
                },
                message: validationMessageDateFinishedMustBeAfterDateStarted,
                params: self.fromMonth,
                onlyIf: function () {
                    return (self.fromYear() === self.toYear());
                }
            }
        });

        self.toYear = ko.observable().extend({
            required: {
                message: validationMessageToYearRequired,
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            number: {
                message: validationMessageToYearMustBeNumeric,
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        }).extend({
            max: {
                message: validationMessageToYearMustNotBeInFuture,
                params: self.currentYear,
                onlyIf: function () {
                    return (self.isCurrentEmployment() === false);
                }
            }
        }).extend({
            min: {
                message: validationMessageYearMustBeAfter,
                params: new Date().getFullYear() - 100
            }
        }).extend({
            validation: {
                validator: function (val, fromYearValue) {
                    return val >= fromYearValue;
                },
                message: validationMessageToYearMustBeAfterFromYear,
                params: self.fromYear,
                onlyIf: function () {
                    return (self.isCurrentEmployment() === false);
                }
            }
        });

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

                var experience = new workExperienceItemModel(self.employer(), self.jobTitle(), self.mainDuties(), self.fromMonth(), self.fromYear(), toMonth, toYear, self.isCurrentEmployment(), self.currentYear(), self.regexPattern(), self.yearRegexPattern());
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

                $('#workAddConfirmText').text("Work experience has been added to table below");

            } else {
                self.errors.showAllMessages();
                $('#workAddConfirmText').text("There has been a problem adding work experience, check you've entered all details correctly");
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

        self.checkHasNoWorkExperience = function (){
            self.showWorkExperience(false);
            self.hasWorkExperience(undefined);
            self.hasNoWorkExperience("checked");
        };

        self.getWorkExperiences = function (data) {

            $(data).each(function (index, item) {

                var currentEmployer = false;
                var myToYear = item.ToYear;

                if (myToYear <= 1) {
                    currentEmployer = true;
                    myToYear = null;
                }

                var experienceItemModel = new workExperienceItemModel(item.Employer, item.JobTitle, item.Description, item.FromMonth, item.FromYear, item.ToMonth, myToYear, currentEmployer, self.currentYear(), self.regexPattern(), self.yearRegexPattern());
                experienceItemModel.disableToDateIfCurrent();
                self.workExperiences.push(experienceItemModel);
            });

            if (self.workExperiences().length > 0) {
                self.showWorkExperience(true);
                self.hasWorkExperience("checked");
                self.hasNoWorkExperience(undefined);

            } else {
                self.checkHasNoWorkExperience();
            }

        };

    };
    //Change this to modify where the vertical bar is placed
    ko.bindingHandlers.parentvalElement = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var valueIsValid = valueAccessor().isValid();
            if (!valueIsValid && viewModel.isAnyMessageShown()) {
                //adds the vertical bar to the input element when it is invalid
                $(element).addClass("input-validation-error");
            }
            else {
                //removes the vertical bar when valid
                $(element).removeClass("input-validation-error");
            }
        }
    };

    $(function () {
        //override default knockout validation - insert validation message
        ko.validation.insertValidationMessage = function (element) {

            var span = document.createElement('span');

            span.className = "field-validation-error";
            $(span).attr('aria-live', 'polite');

            var inputFormControls = $(element).closest(".form-control");

            if (inputFormControls.length > 0) {

                $(span).insertAfter(inputFormControls);

            } else {
                element.parentNode.insertBefore(span, element.nextSibling);
            }
            return span;
        };

        ko.validation.rules.pattern.message = 'Invalid.';
        ko.validation.configure({
            decorateElement: true,
            registerExtenders: true,
            messagesOnModified: true,
            insertMessages: true,
            parseInputAttributes: true,
            errorClass: 'input-validation-error',
            errorElementClass: 'input-validation-error'//,
            //messageTemplate: 'errorMessage',
            //grouping: { deep: true, observable: false }
        });

        var qualificationModel = new qualificationViewModel();
        var experienceViewModel = new workExperienceViewModel();

        if (window.getCurrentYear()) {
            experienceViewModel.currentYear(window.getCurrentYear());
        }

        if (window.getWhiteListRegex()) {
            qualificationModel.regexPattern(window.getWhiteListRegex());
            experienceViewModel.regexPattern(window.getWhiteListRegex());
        }

        if (window.getYearRegex()) {
            qualificationModel.yearRegexPattern(window.getYearRegex());
            experienceViewModel.yearRegexPattern(window.getYearRegex());
        }

        if (window.getQualificationData()) {
            qualificationModel.getqualifications(window.getQualificationData());
        }
        else {
            qualificationModel.checkHasNoQualifications();
        }

        if (window.getWorkExperienceData()) {
            experienceViewModel.getWorkExperiences(window.getWorkExperienceData());
        } else {
            experienceViewModel.checkHasNoWorkExperience();
        }

        ko.applyBindings(qualificationModel, document.getElementById('applyQualifications'));

        ko.applyBindings(experienceViewModel, document.getElementById('applyWorkExperience'));

    });
}());