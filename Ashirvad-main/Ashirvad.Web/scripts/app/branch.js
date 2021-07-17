/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    $('input[type=radio][name=Status]').change(function () {
        if (this.value == 'Active') {
            $("#RowStatus_RowStatusId").val(1);
        }
        else {
            $("#RowStatus_RowStatusId").val(2);
        }
    });

    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }

    $("#studenttbl tr").each(function () {
        var elemImg = $(this).find("#branchImg");
        var branchID = $(this).find("#item_BranchID").val();
        if (elemImg.length > 0 && branchID.length > 0) {
            //AjaxCall(commonData.Branch + "GetBranchLogo", '{ "branchID": ' + branchID +'}', function (data) {
            //    $(elemImg).attr('src', data);
            //}, function (xhr) {
            //    $(elemImg).attr('src', "../ThemeData/images/Default.png");
            //});
            var postCall = $.post(commonData.Branch + "GetBranchLogo", { "branchID": branchID });
            postCall.done(function (data) {
                $(elemImg).attr('src', data);
            }).fail(function () {
                $(elemImg).attr('src', "../ThemeData/images/Default.png");
            });
        }
    });

});

function SaveBranch() {
    
    var isSuccess = ValidateData('dInformation');

    if (isSuccess) {
        
        var frm = $('#fBranchDetail');
        var formData = new FormData(frm[0]);
        formData.append('ImageFile', $('input[type=file]')[0].files[0]);

        AjaxCallWithFileUpload(commonData.Branch + 'SaveBranch', formData, function (data) {
            
            if (data) {
                ShowMessage('Branch details saved!', 'Success');
                window.location.href = "BranchMaintenance?branchID=0";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {

        });
    }
}

function RemoveBranch(branchID) {
    var postCall = $.post(commonData.Branch + "RemoveBranch", { "branchID": branchID });
    postCall.done(function (data) {
        ShowMessage("Branch Removed Successfully.", "Success");
        window.location.href = "BranchMaintenance?branchID=0";
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}