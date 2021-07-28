/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            if ($("#Branch_BranchID").val() != "0") {
                $("#rowStaBranch").attr('checked', 'checked');
                $("#BranchDiv").show();
                $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            } else {
                $("#rowStaAll").attr('checked', 'checked');
                $("#BranchDiv").hide();
                $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
                $("#Branch_BranchID").val(0);
            }
        } else {
            $("#BranchDiv").hide();
            $("#Branch_BranchID").val(0);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        if ($("#Branch_BranchID").val() != "0") {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchDiv").show();
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        } else {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchDiv").hide();
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(0);
        }
    } else {
        $("#BranchDiv").hide();
        $("#Branch_BranchID").val(0);
    }
    
    $('input[type=radio][name=Status]').change(function () {
        if (this.value == 'Active') {
            $("#RowStatus_RowStatusId").val(1);
        }
        else {
            $("#RowStatus_RowStatusId").val(2);
        }
    });

    $('input[type=radio][name=Type]').change(function () {
        if (this.value == 'Branch') {
            $('#BranchName option[value="0"]').attr("selected", "selected");
            $("#BranchDiv").show();
        }
        else {
            $("#BranchDiv").hide();
            $("#Branch_BranchID").val(0);
        }
    });


    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }

    $("#studenttbl tr").each(function () {
        var elemImg = $(this).find("#branchImg");
        var branchID = $(this).find("#item_BranchID").val();
        if (elemImg.length > 0 && branchID.length > 0) {
            var postCall = $.post(commonData.Branch + "GetBranchLogo", { "branchID": branchID });
            postCall.done(function (data) {
                $(elemImg).attr('src', data);
            }).fail(function () {
                $(elemImg).attr('src', "../ThemeData/images/Default.png");
            });
        }
    });

});

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

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


$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
});
