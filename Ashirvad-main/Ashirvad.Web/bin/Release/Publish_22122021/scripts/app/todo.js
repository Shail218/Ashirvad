/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    if ($("#ToDoID").val() > 0) {
        $("#fuDocument").addClass("editForm");
    }

    $("#datepickertodo").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");    
        }
        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadUser(commonData.BranchID);
            HideLoader();
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
    
    var postCall = $.post(commonData.UserPermission + "GetAllUsers", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#UserName').empty();
        $('#UserName').select2();
        $("#UserName").append("<option value=" + 0 + ">---Select User---</option>");
        for (i = 0; i < data.length; i++) {
            $("#UserName").append("<option value=" + data[i].UserID + ">" + data[i].Username + "</option>");
        }
        if ($("#UserInfo_UserID").val() != "") {
            $('#UserName option[value="' + $("#UserInfo_UserID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function SaveToDo() {   
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#ToDoDate").val();
        $("#ToDoDate").val(ConvertData(date1));
        var frm = $('#fToDoDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.ToDo + 'SaveToDo', formData, function (data) {           
            if (data) {
                HideLoader();
                ShowMessage('ToDo details saved!', 'Success');
                window.location.href = "ToDoMaintenance?todoID=0";
            }
            else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemoveToDo(todoID) {
    if (confirm('Are you sure want to delete this ToDo Task?')) {
        ShowLoader();
        var postCall = $.post(commonData.ToDo + "RemoveToDo", { "todoID": todoID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("ToDo Removed Successfully.", "Success");
            window.location.href = "ToDoMaintenance?todoID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function DownloadToDo(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.ToDo + "Downloadtodo", { "todoid": branchID });
    postCall.done(function (data) {
        HideLoader();
        if (data != null) {
            var a = document.createElement("a"); //Create <a>
            a.href = "data:" + data[3] + ";base64," + data[1]; //Image Base64 Goes here
            a.download = data[2];//File name Here
            a.click(); //Downloaded file
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadUser(Data);
});

$("#UserName").change(function () {
    
    var Data = $("#UserName option:selected").val();
    $('#UserInfo_UserID').val(Data);
});