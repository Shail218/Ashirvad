﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            LoadStandard($("#Branch_BranchID").val());
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

function LoadStandard(branchID) {
    
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if ($("#StandardID").val() != "") {
            $('#StandardName option[value="' + $("#StandardID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveYoutube() {    
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Youtube + "SaveYoutube", $('#fyoutubeDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Youtube added Successfully.", "Success");
            window.location.href = "YoutubeMaintenance?linkID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveYoutube(schoolID) {
    ShowLoader();
    var postCall = $.post(commonData.Youtube + "RemoveYoutube", { "linkID": schoolID });
    postCall.done(function (data) {
        HideLoader();
        ShowMessage("Youtube Removed Successfully.", "Success");
        window.location.href = "YoutubeMaintenance?linkID=0";
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    
    var Data = $("#StandardName option:selected").val();
    $('#StandardID').val(Data);
});