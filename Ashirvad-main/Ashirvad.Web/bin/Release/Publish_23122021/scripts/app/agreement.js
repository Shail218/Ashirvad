/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    $("#RowStatusData_RowStatusId").val(1);

    $("#datepickerfromdate").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
    });

    $("#datepickertodate").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
    });

    LoadBranch(function () {
        if ($("#BranchData_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchData_BranchID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#BranchData_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchData_BranchID").val() + '"]').attr("selected", "selected");
    }

    if ($("#RowStatusData_RowStatusId").val() != "") {

        var rowStatus = $("#RowStatusData_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
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

function SaveAgreement() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#AgreementFromDate").val();
        $("#AgreementFromDate").val(ConvertData(date1));
        var date2 = $("#AgreementToDate").val();
        $("#AgreementToDate").val(ConvertData(date2));
        var postCall = $.post(commonData.Agreement + "SaveAgreement", $('#fAgreementDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Agreement Details Inserted Successfully.", "Success");
                setTimeout(function () { window.location.href = "AgreementMaintenance?agreeID=0"; }, 2000);
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

$('input[type=radio][name=Status]').change(function () {
    if (this.value == 'Active') {
        $("#RowStatusData_RowStatusId").val(1);
    }
    else {
        $("#RowStatusData_RowStatusId").val(2);
    }
});