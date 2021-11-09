/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function SaveClass() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Class + "SaveClass", $('#fClassDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.ClassID >= 0) {
                ShowMessage("Class details saved!", "Success");
                setTimeout(function () { window.location.href = "ClassMaintenance?classID=0" }, 2000);
            } else {
                ShowMessage("Class Already Exists!!", "Error");
            }
        }).fail(function () {
            HideLoader();
        });
    }
}

function RemoveClass(classId) {
    if (confirm('Are you sure want to delete this Class?')) {
        ShowLoader();
        var postCall = $.post(commonData.Class + "RemoveClass", { "classID": classId });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Class Removed Successfully.", "Success");
            window.location.href = "ClassMaintenance?classID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}