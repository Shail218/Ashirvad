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

        LoadStandard($("#BranchInfo_BranchID").val());
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#BranchInfo_BranchID").val());
    }

    if ($("#BatchTime").val() != "") {
        $('#BatchName option[value="' + $("#BatchTime").val() + '"]').attr("selected", "selected");
    }

    if ($("#BatchID").val() > 0) {
        SpliteData();
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

        //$.each(data, function (i) {
        //    $("#BranchName").append($("<option></option>").val(data[i].BranchID).html(data[i].BranchName));
        //});

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

        if ($("#StandardInfo_StandardID").val() != "") {
            
            $('#StandardName option[value="' + $("#StandardInfo_StandardID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveBatch() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var start = $("#starttime").val();
        var end = $("#endtime").val();
        $('#MonFriBatchTime').val(start + " - " + end);
        var ststart = $("#sttimestart").val();
        var stend = $("#sttimeend").val();
        $('#SatBatchTime').val(ststart + " - " + stend);
        var snststart = $("#snstarttime").val();
        var snstend = $("#snendtime").val();
        $('#SunBatchTime').val(snststart + " - " + snstend);
        var postCall = $.post(commonData.Batch + "SaveBatch", $('#fBatchDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Batch added Successfully.", "Success");
            window.location.href = "BatchMaintenance?branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        }); 
    }
}

function RemoveBatch(batchID) {
    
    var postCall = $.post(commonData.Batch + "RemoveBatch", { "batchID": batchID });
    postCall.done(function (data) {
        
        ShowMessage("Batch Removed Successfully.", "Success");
        window.location.href = "BatchMaintenance?branchID=0";
    }).fail(function () {
        
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SpliteData() {
    var SplitData = $('#SunBatchTime').val().split('-');
    var SplitData1 = $('#MonFriBatchTime').val().split('-');
    var SplitData2 = $('#SatBatchTime').val().split('-');
    var snststart = $("#snstarttime").val(SplitData[0]);
    var snstend = $("#snendtime").val(SplitData[1]);
    var start = $("#starttime").val(SplitData1[0]);
    var end = $("#endtime").val(SplitData1[1]);   
    var ststart = $("#sttimestart").val(SplitData2[0]);
    var stend = $("#sttimeend").val(SplitData2[1]);   
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#StandardInfo_StandardID').val(Data);
});

$("#BatchName").change(function () {
    var Data = $("#BatchName option:selected").val();
    $('#BatchType').val(Data);
});