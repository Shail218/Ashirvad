/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    
    LoadBranch();
    /*arrayBufferToBase64()*/
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
    ShowLoader();
    var Data = commonData.BranchID;
    var postCall = $.post(commonData.ManageStudent + "GetAllActiveStudent", { "branchID": Data });
    postCall.done(function (data) {
        HideLoader();
        $('#studentData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadInActiveStudent() {
    ShowLoader();
    var Data = commonData.BranchID;
    var postCall = $.post(commonData.ManageStudent + "GetAllInActiveStudent", { "branchID": Data });
    postCall.done(function (data) {
        HideLoader();
        $('#studentData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function RemoveStudent(studentID) {
    if (confirm('Are you sure want to delete this Student?')) {
        ShowLoader();
        var postCall = $.post(commonData.ManageStudent + "Removestudent", { "studentID": studentID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Student Removed Successfully.", "Success");
            window.location.href = "ManageStudentMaintenance?branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchID').val(Data);
});