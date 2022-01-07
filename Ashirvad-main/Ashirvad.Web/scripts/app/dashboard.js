/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    function chart() {
        var postCall = $.post(commonData.Home + "GetAllBranchChart");
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Subject Removed Successfully.", "Success");
            window.location.href = "SubjectMaintenance?branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
});