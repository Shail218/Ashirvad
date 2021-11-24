/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
  
    if ($("#BranchData_BranchID").val() != "") {
        if ($("#BranchData_BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchData_BranchID").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchData_BranchID").val(1);
        }
    } else {
        $("#BranchData_BranchID").val(0);
    }
});

function SaveAnnouncement() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Announcement + "SaveAnnouncement", $('#fAnnouncementDetail').serialize());
        postCall.done(function (data) {
            if (data) {
                HideLoader();
                ShowMessage("Announcement Inserted Successfully.", "Success");
                setTimeout(function () { window.location.href = "AnnouncementMaintenance?annoID=0"; }, 2000);
            } else {
                HideLoader();
            }           
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveAnnouncement(annoID) {
    if (confirm('Are you sure want to delete this Announcement?')) {
        ShowLoader();
        var postCall = $.post(commonData.Announcement + "RemoveAnnouncement", { "annoID": annoID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Announcement Removed Successfully.", "Success");
            window.location.href = "AnnouncementMaintenance?annoID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$('input[type=radio][name=Type]').change(function () {
    if (this.value == 'All') {
        $("#BranchData_BranchID").val(0);
    }
    else {
        $("#BranchData_BranchID").val(1);
    }
});