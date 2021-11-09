/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    if ($("#FacultyID").val() > 0) {
        $("#fuHeaderImageDetail").addClass("editForm");
    }

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
            LoadUser($("#BranchInfo_BranchID").val());
            LoadStandard($("#BranchInfo_BranchID").val());
            LoadSubject($("#BranchInfo_BranchID").val());
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadUser(commonData.BranchID);
            LoadStandard(commonData.BranchID);
            LoadSubject(commonData.BranchID);
        }
    });

    if ($("#FacultyID").val() > 0) {
        if ($("#board").val() != "") {
            var rowStatus = $("#board").val();
            debugger;
            if (rowStatus == "GujaratBoard") {
                $("#rowStaGujarat").attr('checked', 'checked');
            }
            else if (rowStatus == "CBSC") {
                $("#rowStaCBSC").attr('checked', 'checked');

            }
            else {
                $("#rowStaBoth").attr('checked', 'checked');
            }
        }
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

        $('#facultyName').empty();
        $('#facultyName').select2();
        $("#facultyName").append("<option value=" + 0 + ">---Select Faculty---</option>");
        for (i = 0; i < data.length; i++) {
            $("#facultyName").append("<option value=" + data[i].UserID + ">" + data[i].Username + "</option>");
        }
        if ($("#staff_StaffID").val() != "") {
            $('#facultyName option[value="' + $("#staff_StaffID").val() + '"]').attr("selected", "selected");
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
        if ($("#standard_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#standard_StandardID").val() + '"]').attr("selected", "selected");
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
        if ($("#subject_SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#subject_SubjectID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});


$("#facultyName").change(function () {
    var Data = $("#facultyName option:selected").val();
    $('#staff_StaffID').val(Data);
});


$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    $('#subject_SubjectID').val(Data);
});


$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#standard_StandardID').val(Data);
});

$('input[type=radio][name=Board]').change(function () {
    if (this.value == 1) {
        $("#board").val(1);
    }
    else if (this.value==2) {
        $("#board").val(2);
    }
    else {
        $("#board").val(parseInt(3));
    }
});

function SaveFaculty() {
    debugger;
    var isSuccess = ValidateData('dInformation');

    if (isSuccess) {
        ShowLoader();
        var frm = $('#ffacultydetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Faculty + 'SaveFaculty', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage("Faculty added Successfully.", "Success");
                window.location.href = "FacultyMaintenance?facultyID=0";
            } else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
        //var postCall = $.post(commonData.Faculty + "SaveFaculty", $('#ffacultydetail').serialize());
        //postCall.done(function (data) {
        //    HideLoader();
        //    ShowMessage('Faculty details saved!', 'Success');
        //    window.location.href = "FacultyMaintenance?facultyID=0";
        //}).fail(function () {
        //    HideLoader();
        //    ShowMessage("An unexpected error occcurred while processing request!", "Error");
        //});

    }
}

function RemoveFaculty(facultyID) {
    if (confirm('Are you sure want to delete this Faculty?')) {
        ShowLoader();
        var postCall = $.post(commonData.Faculty + "RemoveFaculty", { "FacultyID": facultyID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Faculty Removed Successfully.", "Success");
            window.location.href = "FacultyMaintenance?facultyID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

