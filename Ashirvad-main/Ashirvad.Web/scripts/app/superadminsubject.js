/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function SaveSubject() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.SuperAdminSubject + "SaveSubject", $('#fSubjectDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.SubjectID >= 0) {
                ShowMessage("Subject details saved!", "Success");
                setTimeout(function () { window.location.href = "SubjectMaintenance?subjectID=0" }, 2000);
            } else {
                ShowMessage("Subject Already Exists!!", "Error");
            }
        }).fail(function () {
            HideLoader();
        });
    }
}

function RemoveSubject(subjectId) {
    if (confirm('Are you sure want to delete this Subject?')) {
        ShowLoader();
        var postCall = $.post(commonData.SuperAdminSubject + "RemoveSubject", { "subjectID": subjectId });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Subject Removed Successfully.", "Success");
            window.location.href = "SubjectMaintenance?subjectID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}