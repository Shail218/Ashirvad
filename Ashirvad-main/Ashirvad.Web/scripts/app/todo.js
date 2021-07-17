﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    $("#datepickertodo").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");    
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadUser($("#BranchInfo_BranchID").val());
    }

});

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        debugger;
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

function LoadUser(branchID) {
    debugger;
    var postCall = $.post(commonData.UserPermission + "GetAllUsers", { "branchID": branchID });
    postCall.done(function (data) {
        debugger;
        $('#UserName').empty();
        $('#UserName').select2();
        $("#UserName").append("<option value=" + 0 + ">---Select User---</option>");
        for (i = 0; i < data.length; i++) {
            $("#UserName").append("<option value=" + data[i].UserID + ">" + data[i].Username + "</option>");
        }
        if ($("#UserInfo_UserID").val() != "") {
            $('#UserName option[value="' + $("#UserInfo_UserID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function SaveToDo() {
    debugger;
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        var date1 = $("#ToDoDate").val();
        $("#ToDoDate").val(ConvertData(date1));
        var frm = $('#fToDoDetail');
        var formData = new FormData(frm[0]);
        formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        AjaxCallWithFileUpload(commonData.ToDo + 'SaveToDo', formData, function (data) {
            debugger;
            if (data) {
                ShowMessage('ToDo details saved!', 'Success');
                window.location.href = "ToDoMaintenance?todoID=0";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {

        });
    }
}

function RemoveToDo(todoID) {
    var postCall = $.post(commonData.ToDo + "RemoveToDo", { "todoID": todoID });
    postCall.done(function (data) {
        ShowMessage("ToDo Removed Successfully.", "Success");
        window.location.href = "ToDoMaintenance?todoID=0";
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    debugger;
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadUser(Data);
});

$("#UserName").change(function () {
    debugger;
    var Data = $("#UserName option:selected").val();
    $('#UserInfo_UserID').val(Data);
});