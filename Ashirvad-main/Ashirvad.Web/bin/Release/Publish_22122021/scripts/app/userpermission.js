﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    LoadUser(commonData.BranchID);
});

function LoadUser(branchId) {
    var postCall = $.post(commonData.UserPermission + "GetAllUsers", { "branchID": branchId });
    postCall.done(function (data) {
        $('#UserName').empty();
        $('#UserName').select2();
        $("#UserName").append("<option value=" + 0 + ">---Select User---</option>");
        for (i = 0; i < data.length; i++) {
            $("#UserName").append("<option value=" + data[i].UserID + ">" + data[i].Username + "</option>");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveUserPermission() {
    var RoleList = [];
    $("#studenttbl tbody tr").each(function () {
        var RoleID = $(this).find("#item_Key").val();
        var UserID = $('#UserID').val();
        var HasAccessVal = $(this).find("#userrole").is(":checked");
        if (RoleID != null) {
            RoleList.push({
                Permission: RoleID,
                UserID: UserID,
                HasAccess: HasAccessVal
            });
        }
    });
    $('#JSONData').val(JSON.stringify(RoleList));
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fUserPermissionDetail');
        var formData = new FormData(frm[0]);
        //formData.append('Roles', RoleList);
        
        var postCall = $.post(commonData.UserPermission + "UserRoleManagement", $('#fUserPermissionDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("User Permission added Successfully.", "Success");
            window.location.href = "UserPermissionMaintenance";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}


$("#UserName").change(function () {
    
    var Data = $("#UserName option:selected").val();
    $('#UserID').val(Data);
});
