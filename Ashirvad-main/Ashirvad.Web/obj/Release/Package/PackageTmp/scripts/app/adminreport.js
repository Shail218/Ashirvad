/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadBranch(function () {
        if ($("#hdnBranchID").val() != "") {
            $('#BranchName option[value="' + $("#hdnBranchID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchID").val() + '"]').attr("selected", "selected");
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

$("#BranchName").change(function () {
    var id = $("#BranchName option:selected").val();
    $('#BranchID').val(id);
    ShowLoader();
    var postCall = $.post(commonData.AdminReport + "GetReportDataBranchWise", { "branchId": id });
    postCall.done(function (data) {
       
        $('#dataUsage').html(data);
        HideLoader();
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
});