/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    $("#notification").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
    });

    if ($("#Branch_BranchID").val() != "") {
        if ($("#Branch_BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#Branch_BranchID").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#Branch_BranchID").val(1);
        }
    } else {
        $("#Branch_BranchID").val(0);
    }

    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }

});

function SaveNotification() {    
    var isSuccess = ValidateData('dInformation');
    var NotificationTypeList = [];
    if ($('input[type=checkbox][id=rowStaAdmin]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Admin",
            TypeID: 1
        });
    }
    if ($('input[type=checkbox][id=rowStaTeacher]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Teacher",
            TypeID: 2
        });
    }
    if ($('input[type=checkbox][id=rowStaStudent]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Student",
            TypeID: 3
        });
    }
    $('#JSONData').val(JSON.stringify(NotificationTypeList));
    var date2 = $("#Notification_Date").val();
    $("#Notification_Date").val(ConvertData(date2));
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Notification + "SaveNotification", $('#fNotificationDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage('Notification details saved!', 'Success');
            window.location.href = "NotificationMaintenance?notificationID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function RemoveNotification(branchID) {
    if (confirm('Are you sure want to delete this Notification?')) {
        ShowLoader();
        var postCall = $.post(commonData.Notification + "RemoveNotification", { "notificationID": branchID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Notification Removed Successfully.", "Success");
            window.location.href = "NotificationMaintenance?notificationID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}


$('input[type=radio][name=Type]').change(function () {
    if (this.value == 'All') {
        $("#Branch_BranchID").val(0);
    }
    else {
        $("#Branch_BranchID").val(1);
    }
});
