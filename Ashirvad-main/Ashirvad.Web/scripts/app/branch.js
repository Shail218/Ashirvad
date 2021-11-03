/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {

    if ($("#BranchID").val() > 0) {
        $("#fuBranchImage").addClass("editForm");
    }

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

    if ($("#board").val() != "") {
        var rowStatus = $("#board").val();
        if (rowStatus == "1") {
            $("#rowStaGujarat").attr('checked', 'checked');
        }
        else if (rowStatus == "2") {
            $("#rowStaCBSC").attr('checked', 'checked');

        }
        else {
            $("#rowStaBoth").attr('checked', 'checked');
        }
    }

});

$('input[type=radio][name=Board]').change(function () {
    if (this.value == 1) {
        $("#board").val(parseInt(1));
    }
    else if (this.value == 2) {
        $("#board").val(parseInt(2));
    }
    else {
        $("#board").val(parseInt(3));
    }
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