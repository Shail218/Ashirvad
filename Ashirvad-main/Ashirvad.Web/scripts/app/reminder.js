/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    $("#datepickerreminder").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });
});

function SaveReminder() {
    debugger;
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        var date1 = $("#ReminderDate").val();
        $("#ReminderDate").val(ConvertData(date1));
        var postCall = $.post(commonData.Reminder + "SaveReminder", $('#fReminderDetail').serialize());
        postCall.done(function (data) {
            ShowMessage("Reminder added Successfully.", "Success");
            window.location.href = "ReminderMaintenance?reminderID=0";
        }).fail(function () {
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveReminder(reminderID) {
    debugger;
    var postCall = $.post(commonData.Reminder + "RemoveReminder", { "reminderID": reminderID });
    postCall.done(function (data) {
        debugger;
        ShowMessage("Reminder Removed Successfully.", "Success");
        window.location.href = "ReminderMaintenance?reminderID=0";
    }).fail(function () {
        debugger;
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}