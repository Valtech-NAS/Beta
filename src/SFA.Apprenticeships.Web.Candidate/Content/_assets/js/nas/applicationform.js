(function() {

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

    var listItemModel = function (itemType, itemOtherType, itemYear, itemSubject, itemGrade, itemPredicted) {

        var self = this;

        self.qualificationType = ko.observable(itemType).extend({ required: { message: "Select qualification type" } });
        self.otherQualificationType = ko.observable(itemOtherType);
        self.qualificationYear = ko.observable(itemYear).extend({ required: { message: "Year is required" }, number: true });
        self.qualificationSubject = ko.observable(itemSubject).extend({ required: { message: "Subject is required" } });
        self.qualificationGrade = ko.observable(itemGrade).extend({ required: { message: "Grade is required" } });
        self.qualificationPredicted = ko.observable(itemPredicted);
        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);
    };

    function addQualification(qualifications, typeSelected, typeOther, year, subject, grade, predicted) {

        var qualificationArrays = qualifications;

        if (predicted === true) {
            grade += " (Predicted)";
        }

        if (typeSelected === "Other") {
            if (!typeOther || 0 === typeOther.length) {

            } else {
                typeSelected = typeOther;
            }
        }

        var qualificationItemModel = new listItemModel(typeSelected, typeOther, year, subject, grade, predicted);

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

        self.yesHasQualifications = ko.observable(undefined);
        self.noHasQualifications = ko.observable(undefined);
        self.showQualifications = ko.observable(false);

        self.qualificationStatus = ko.computed(function() {
            return self.showQualifications() ? "block" : "none";
        }, qualificationViewModel);

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
        });
        self.showOtherQualification = ko.observable(false);

        self.year = ko.observable().extend({ required: { message: "Year is required" }, number: true });
        self.subject = ko.observable().extend({ required: { message: "Subject is required" } });
        self.grade = ko.observable().extend({ required: { message: "Grade is required" } });
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
 
            //alert("Removing " + qualification.qualificationType() + " array length " + self.qualifications().length);
            var match = ko.utils.arrayFirst(self.qualifications(), function (item) {
                //alert("Item Key " + item.groupKey() + " Item to remove " + qualification.qualificationType());
                return item.groupKey() === qualification.qualificationType();
            });

            if (match) {
                //alert("matched");
                match.removeItem(qualification);
            }
        };

        self.addQualification = function () {

            // var self = this;

            if (self.errors().length == 0) {

                var typeSelected = self.selectedQualification();
                var typeOther = self.otherQualification();
                var year = self.year();
                var subject = self.subject();
                var grade = self.grade();
                var predicted = self.predicted();

                var result = addQualification(self.qualifications(), typeSelected, typeOther, year, subject, grade, predicted);
                self.qualifications(result);
                self.subject("");
                self.grade("");
                self.predicted(false);

                self.displayErrors(false);
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
            qualification.readOnly('readonly');
            qualification.showEditButton(true);
        };

        self.getqualifications = function (data) {
            //var self = this;

            $(data).each(function (index, item) {
                var result = addQualification(self.qualifications(), item.QualificationType, "", item.Year, item.Subject, item.Grade, item.IsPredicted);

                self.qualifications(result);
               
            });

            if (self.qualifications().length > 0) {
                self.showQualifications(true);
                self.yesHasQualifications("checked");
                self.noHasQualifications(undefined);
            } else {
                self.showQualifications(false);
                self.yesHasQualifications(undefined);
                self.noHasQualifications("checked");
            }
        };
    };

    var monthOfTheYear = function(monthName, monthNo) {
        var self = this;

        self.monthName = monthName;
        self.monthNumber = monthNo;
    };

    ko.validation.rules['mustBeGreaterThanOrEqual'] = {
        validator: function (val, otherVal) {          
            return val === otherVal || val > otherVal ;
        },
        message: 'To Year must be greater than or equal to From Year'
    };
    ko.validation.registerExtenders();

    var workExperienceItemModel = function (itemEmployer, itemJobTitle, itemDuties, itemFromMonth, itemFromYear, itemToMonth, itemToYear, itemIsCurrentEmployment) {

        var self = this;

        self.itemEmployer = ko.observable(itemEmployer).extend({ required: { message: "Employer is required" } });
        self.itemJobTitle = ko.observable(itemJobTitle).extend({ required: { message: "Job Title is required" } });
        self.itemMainDuties = ko.observable(itemDuties).extend({ required: { message: "Enter some of your main duties" } });

        self.itemIsCurrentEmployment = ko.observable(itemIsCurrentEmployment);

        self.itemFromMonth = ko.observable(itemFromMonth).extend({ required: { message: "From month is required" } });
        self.itemFromYear = ko.observable(itemFromYear).extend({ required: { message: "From year is required" } });

        self.itemToMonth = ko.observable(itemToMonth).extend({
            required: {
                message: "To month is required",
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            }
        });
        self.itemToYear = ko.observable(itemToYear).extend({
            required: {
                message: "To year is required",
                onlyIf: function () { return (self.itemIsCurrentEmployment() === false); }
            },
            min: {
                params: self.itemFromYear()
            }
        });

        self.readOnly = ko.observable("readonly");
        self.showEditButton = ko.observable(true);

        self.toItemDateReadonly = ko.observable("disabled");

        self.itemIsCurrentEmployment.subscribe(function (selectedValue) {
            if (selectedValue === true) {
                self.itemToYear(null);
                self.toItemDateReadonly("disabled");
            } else {
                self.toItemDateReadonly(undefined);
            }
        });

        self.errors = ko.validation.group(self);
    }

    var workExperienceViewModel = function() {

        var self = this;

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

        self.employer = ko.observable().extend({ required: { message: "Employer is required" } });
        self.jobTitle = ko.observable().extend({ required: { message: "Job Title is required" } });
        self.mainDuties = ko.observable().extend({ required: { message: "Enter some of your main duties" } });

        self.isCurrentEmployment = ko.observable(false);

        self.fromMonth = ko.observable().extend({ required: { message: "From month is required" } });
        self.fromYear = ko.observable().extend({ required: { message: "From year is required" }, number: true });

        self.toMonth = ko.observable().extend({
            required: {
                message: "To month is required",
                onlyIf: function () { return (self.isCurrentEmployment() === false); }
            }
        });

        self.toYear = ko.observable().extend({
            required: {
                message: "To year is required",
                onlyIf: function () { return (self.isCurrentEmployment() === false); },
                number: true 
            }
        });

        self.toYear.extend({mustBeGreaterThanOrEqual : self.fromYear});

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

        self.addWorkExperience = function() {

            if (self.errors().length == 0) {

                var toMonth = self.toMonth();
                var toYear = self.toYear();

                if (self.isCurrentEmployment() === true) {
                    //alert(self.isCurrentEmployment());
                    toMonth = 0;
                    toYear = 0;
                }

                var experience = new workExperienceItemModel(self.employer(), self.jobTitle(), self.mainDuties(), self.fromMonth(), self.fromYear(), toMonth, toYear, self.isCurrentEmployment());

                self.workExperiences.push(experience);

            } else {
                self.errors.showAllMessages();
            }

        };

        self.editWorkExperience = function(workExperience) {
            workExperience.readOnly(undefined);
            workExperience.showEditButton(false);
        };

        self.saveWorkExperience = function(workExperience) {
            workExperience.readOnly('readonly');
            workExperience.showEditButton(true);
        };

        self.removeWorkExperience = function(workExperience) {
            self.workExperiences.remove(workExperience);
        };

        self.getWorkExperiences = function(data) {

            $(data).each(function (index, item) {
                //alert(JSON.stringify(item));
                var currentEmployer = false;

                if (isNaN(item.ToYear) && item.toYear > 0) {
                    currentEmployer = true;
                }

                var experienceItemModel = new workExperienceItemModel(item.Employer, item.JobTitle, item.Description, item.FromMonth, item.FromYear, item.ToMonth, item.ToYear, currentEmployer);
                self.workExperiences.push(experienceItemModel);
            });

            //alert(JSON.stringify(self.workExperiences()));

            if (self.workExperiences().length > 0) {
                //alert(self.workExperiences().length);
                self.showWorkExperience(true);
                self.hasWorkExperience("checked");
                self.hasNoWorkExperience(undefined);
            } else {
                //alert(self.workExperiences().length);
                self.showWorkExperience(false);
                self.hasWorkExperience(undefined);
                self.hasNoWorkExperience("checked");
            }

        };

    };

    //var educationViewModel = function() {
    //    var self = this;

    //    self.collegeName = ko.observable().extend({ required: { message: "School or College is required" } });
    //    self.fromYear = ko.observable().extend({ required: { message: "From year is required" } });
    //    self.toYear = ko.observable().extend({ required: { message: "To year is required" } });

    //    self.errors = ko.validation.group(self);
    //};

    //var aboutYouViewModel = function() {
    //    var self = this;

    //    self.strengths = ko.observable().extend({ required: { message: "Please enter some of your strengths" } });
    //    self.improvements = ko.observable().extend({ required: { message: "Please enter some area you want to improve" } });
    //    self.hobbiesandInterests = ko.observable().extend({ required: { message: "Please enter hobbies and interests" } });
    //    self.supportrequired = ko.observable().extend({ required: { message: "Do you require any support" } });

    //    self.errors = ko.validation.group(self);
    //};


    $(function() {

        var model = new qualificationViewModel();

        if (window.getQualificationData()) {
            model.getqualifications(window.getQualificationData());
        }
       
        ko.applyBindings(model, document.getElementById('applyQualifications'));

        var experienceModel = new workExperienceViewModel();

        if (window.getWorkExperienceData()) {
            experienceModel.getWorkExperiences(window.getWorkExperienceData());
        }

        ko.applyBindings(experienceModel, document.getElementById('applyWorkExperience'));

    });   

}());