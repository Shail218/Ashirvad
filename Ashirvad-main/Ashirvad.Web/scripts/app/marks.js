/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    $("#datepickermarks").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

   
  

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadSchoolName(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadSchoolName($("#BranchInfo_BranchID").val());
        LoadStandard($("#BranchInfo_BranchID").val());
    }

    if ($("#SchoolTime").val() != "") {
        $('#SchoolTimeDDL option[value="' + $("#SchoolTime").val() + '"]').attr("selected", "selected");
    }

    if ($("#BatchInfo_BatchTime").val() != "") {
        $('#BatchTime option[value="' + $("#BatchInfo_BatchTime").val() + '"]').attr("selected", "selected");
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

function LoadSchoolName(branchID) {
    var postCall = $.post(commonData.School + "SchoolData", { "branchID": branchID });
    postCall.done(function (data) {
        $('#SchoolName').empty();
        $('#SchoolName').select2();
        $("#SchoolName").append("<option value=" + 0 + ">---Select School Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SchoolName").append("<option value=" + data[i].SchoolID + ">" + data[i].SchoolName + "</option>");
        }
        if ($("#SchoolInfo_SchoolID").val() != "") {
            $('#SchoolName option[value="' + $("#SchoolInfo_SchoolID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function (e) {
        console.log(e);
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
        if ($("#StandardInfo_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#StandardInfo_StandardID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}