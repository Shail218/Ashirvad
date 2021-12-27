/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
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

    LoadStudent(function () {
        if ($("#studentEntity_Name").val() != "") {
            $('#StudentName option[value="' + $("#studentEntity_Name").val() + '"]').attr("selected", "selected");
        }
    });

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#Branch_BranchID").val());
    }

    if ($("#BatchTypeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTypeID").val() + '"]').attr("selected", "selected");
    }
});

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value='" + data[i].BranchID + "'>" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStudent(onLoaded) {
    var postCall = $.post(commonData.AttendanceEntry + "GetAllStudentByBranchStdBatch");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value='" + data[i].BranchID + "'>" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStandard(branchID) {

    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID });
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if ($("#Standard_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#Standard_StandardID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveReport() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#From_Date").val();
        var fromdate= ConvertData(date1);
        var date2 = $("#To_Date").val();
        var todate = ConvertData(date2);
        var branch = $("#Branch_BranchID").val();
        var standard = $("#Standard_StandardID").val();
        var batchtime = $("#BatchTypeID").val();
        var postCall = $.post(commonData.AttendanceReport + "SaveReport", { "FromDate": fromdate, "ToDate": todate, "StandardId": standard, "BatchTime": batchtime });
        postCall.done(function (data) {
            HideLoader();
            $('#ReportData').html(data);
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#Standard_StandardID').val(Data);
});

$("#StudentName").change(function () {
    var Data = $("#StudentName option:selected").val();
    $('#studentEntity_Name').val(Data);
});

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $('#BatchTypeID').val(Data);
});