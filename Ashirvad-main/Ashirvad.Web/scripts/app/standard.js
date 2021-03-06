/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
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
        HideLoader();
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
            if (data.Status) {
                HideLoader();
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "StandardMaintenance?branchID=0" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }          
        }).fail(function () {
            HideLoader();
            ShowMessage(data.Message, "Error");
        });
    }
}

function RemoveStandard(standardID) {
    if (confirm('Are you sure want to delete this Standard?')) {
        ShowLoader();
        var postCall = $.post(commonData.Standard + "RemoveStandard", { "standardID": standardID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                window.location.href = "StandardMaintenance?branchID=0";
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
    $('#BranchInfo_BranchID').val(Data);
});