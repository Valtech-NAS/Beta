(function() {

    var QualificationItemModel = function(qualType,qualOtherType, qualYear, qualSubject, qualGrade, qualPredicted) {

        var self = this;

        self.qualificationType = ko.observable(qualType).extend({ required: true });
        self.otherQualificationType = ko.observable(qualOtherType);
        self.qualificationYear = ko.observable(qualYear).extend({ required: true, min: 1914 });
        self.qualificationSubject = ko.observable(qualSubject).extend({ required: true });
        self.qualificationGrade = ko.observable(qualGrade).extend({ required: true });
        self.qualificationPredicted = ko.observable(qualPredicted);
        self.editMode = ko.observable(false);
    };

    var QualificationModel = function() {

        var self = this;

        self.hasQualifications = ko.observable("no");
        self.selectedQualification = ko.observable("").extend({ required: true });
        self.otherQualification = ko.observable("");
        self.year = ko.observable(0).extend({ required: true, min: 1914 });
        self.subject = ko.observable("").extend({ required: true });
        self.grade = ko.observable("").extend({ required: true });
        self.predicted = ko.observable(false);

        self.qualifications = ko.observableArray();

        self.errors = ko.validation.group(self);

    };

    QualificationModel.prototype = {

        addQualification: function() {

            var self = this;

            if (self.errors().length == 0) {
                var qual = new QualificationItemModel(self.selectedQualification(),self.otherQualification(), self.year(), self.subject(), self.grade(), self.predicted());
                self.qualifications.push(qual);
            }
        },
        removeQualification: function(qualification) {

            var self = this;

            self.qualifications.remove(qualification);

        },
        editQualification: function (qualification) {
            qualification.editMode(true);
        },
        getqualifications:function(data) {

            alert(JSON.stringify(data));
            var self = this;

            $(data).each(function(index, item) {

                var mappedQual = new QualificationItemModel(item.QualificationType, "", item.Year,item.Subject, item.Grade, item.IsPredicted); 
                self.qualifications.push(mappedQual);
            });

        }
    };

    $(function() {

        var qualModel = new QualificationModel();
        qualModel.getqualifications(qualificationData);
        ko.applyBindings(qualModel, document.getElementById('applyQualifications'));

    });

}());