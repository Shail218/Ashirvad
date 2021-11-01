/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    
  
    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            LoadUser($("#Branch_BranchID").val());
            LoadStandard($("#Branch_BranchID").val());
            LoadSubject($("#Branch_BranchID").val());
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadUser(commonData.BranchID);
            LoadStandard(commonData.BranchID);
            LoadSubject(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadUser($("#Branch_BranchID").val());
        LoadStandard($("#Branch_BranchID").val());
        LoadSubject($("#Branch_BranchID").val());
        HideLoader();
    }

    if ($("#BatchTimeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTimeID").val() + '"]').attr("selected", "selected");
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

        $('#BoardName').empty();
        $('#BoardName').select2();
        $("#BoardName").append("<option value=" + 0 + ">---Select Faculty---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BoardName").append("<option value=" + data[i].UserID + ">" + data[i].Username + "</option>");
        }
        if ($("#Faculty_FacultyID").val() != "") {
            $('#BoardName option[value="' + $("#Faculty_FacultyID").val() + '"]').attr("selected", "selected");
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
        if ($("#Standard_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#Standard_StandardID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadSubject(branchID) {
    var postCall = $.post(commonData.Subject + "SubjectDataByBranch", { "branchID": branchID });
    postCall.done(function (data) {

        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SubjectName").append("<option value=" + data[i].SubjectID + ">" + data[i].Subject + "</option>");
        }
        if ($("#Subject_SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#Subject_SubjectID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BoardName").change(function () {
    var Data = $("#BoardName option:selected").val();
    $('#Faculty_FacultyID').val(Data);
});


$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    $('#Subject_SubjectID').val(Data);
});


$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#Standard_StandardID').val(Data);
});
