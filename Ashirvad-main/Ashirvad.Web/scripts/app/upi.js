/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadBranch(function () {
        if ($("#BranchData_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchData_BranchID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#BranchData_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchData_BranchID").val() + '"]').attr("selected", "selected");
    }
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
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveUPI() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.UPI + "SaveUPI", $('#fUPIDetail').serialize());
        postCall.done(function (data) {
            if (data) {
                if (data.Status) {
                    HideLoader();
                    ShowMessage(data.Message, "Success");
                    setTimeout(function () { window.location.href = "UPIMaintenance?upiID=0"; }, 2000);
                } else {
                    HideLoader();
                    ShowMessage(data.Message, "Error");
                }
            }
            else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveUPI(upiID) {
    if (confirm('Are you sure want to delete this UPI ID?')) {
        ShowLoader();
        var postCall = $.post(commonData.UPI + "RemoveUPI", { "upiID": upiID });
        postCall.done(function (data) {
            if (data.Status) {
                HideLoader();
                ShowMessage(data.Message, "Success");
                window.location.href = "UPIMaintenance?upiID=0";
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchData_BranchID').val(Data);
});