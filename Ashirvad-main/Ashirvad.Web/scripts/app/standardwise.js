/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#hdnBranchID").val() == 0) {
        ShowLoader();
        LoadBranch();
    } else {
        LoadStudent(0);
        LoadStudent(0);
        GetAllClassDDL(0);
    }
    document.getElementById("StandardDiv").style.display = 'none';
    document.getElementById("ChartDiv").style.display = 'none';
});

function LoadBranch() {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStudent(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.StandardWiseChart + "StudentData", { "branchID": branchID });
    postCall.done(function (data) {
        $('#StudentName').empty();
        $('#StudentName').select2();
        $("#StudentName").append("<option value=" + 0 + ">---Select Student---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StudentName").append("<option value='" + data[i].StudentID + "'>" + data[i].Name + "</option>");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetAllClassDDL(branchID) {
    var postCall = $.post(commonData.StandardWiseChart + "GetAllClassDDL", { "branchID": branchID });
    postCall.done(function (data) {
        HideLoader();
        document.getElementById("StandardDiv").style.display = 'block';
        document.getElementById("ChartDiv").style.display = 'block';
        $('#StandardData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    LoadStudent(Data);
    GetAllClassDDL(Data);
});

$("#StudentName").change(function () {
    window.location.href = "ProgressReportChart?StudentID=" + $("#StudentName option:selected").val();
});


