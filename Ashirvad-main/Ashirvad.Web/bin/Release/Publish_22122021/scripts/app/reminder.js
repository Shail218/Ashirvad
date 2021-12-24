/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    $("#datepickerreminder").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });
    HideLoader();
});

function SaveReminder() {   
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#ReminderDate").val();
        $("#ReminderDate").val(ConvertData(date1));
        var postCall = $.post(commonData.Reminder + "SaveReminder", $('#fReminderDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Reminder added Successfully.", "Success");
            window.location.href = "ReminderMaintenance?reminderID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveReminder(reminderID) {
    if (confirm('Are you sure want to delete this Reminder?')) {
        ShowLoader();
        var postCall = $.post(commonData.Reminder + "RemoveReminder", { "reminderID": reminderID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Reminder Removed Successfully.", "Success");
            window.location.href = "ReminderMaintenance?reminderID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}