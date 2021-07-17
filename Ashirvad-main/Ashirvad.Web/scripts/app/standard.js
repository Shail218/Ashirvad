/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
       $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
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

//        //$.each(data, function (i) {
//        //    $("#BranchName").append($("<option></option>").val(data[i].BranchID).html(data[i].BranchName));
//        //});

        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveStandard() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Standard + "SaveStandard", $('#fStandardDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Standard added Successfully.", "Success");
            window.location.href = "StandardMaintenance?branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveStandard(standardID) {
    debugger;
    var postCall = $.post(commonData.Standard + "RemoveStandard", { "standardID": standardID });
    postCall.done(function (data) {
        debugger;
        ShowMessage("Standard Removed Successfully.", "Success");
        window.location.href = "StandardMaintenance?branchID=0";
    }).fail(function () {
        debugger;
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    debugger;
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});