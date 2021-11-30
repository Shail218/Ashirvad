﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function saveLibraryApproval(libraryid, librarystatus, approvalid) {
    ShowLoader();
    var postCall = $.post(commonData.ManageLibrary + "SaveLibraryApproval", { "LibraryID": libraryid, "LibraryStatus": librarystatus, "ApprovalID": approvalid });
    postCall.done(function (data) {
        HideLoader();
        if (data) {
            ShowMessage("Library Approve Successfully.", "Success");
            setTimeout(function () { window.location.href = "ManageLibraryMaintenance" }, 2000);
        } else {
            ShowMessage("Please try again!!", "Error");
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}