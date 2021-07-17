/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    LoadBranch();
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
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadActiveStudent() {
    var Data = $('#BranchID').val();
    var postCall = $.post(commonData.ManageStudent + "GetAllActiveStudent", { "branchID": Data });
    postCall.done(function (data) {
        debugger;
        $('#studentData').html(data);
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadInActiveStudent() {
    var Data = $('#BranchID').val();
    var postCall = $.post(commonData.ManageStudent + "GetAllInActiveStudent", { "branchID": Data });
    postCall.done(function (data) {
        debugger;
        $('#studentData').html(data);
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchID').val(Data);
});