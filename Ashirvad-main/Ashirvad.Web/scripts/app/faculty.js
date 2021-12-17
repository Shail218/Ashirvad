﻿/// <reference path="common.js" />
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
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadUser(commonData.BranchID);
        }
    });

    LoadCourse();
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

function LoadCourse() {
    var postCall = $.post(commonData.BranchCourse + "GetCourseDDL");
    postCall.done(function (data) {
        $('#CourseName').empty();
        $('#CourseName').select2();
        $("#CourseName").append("<option value=" + 0 + ">---Select Course---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
            }
        }
       
        if ($("#BranchCourse_course_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
            LoadClass($("#BranchCourse_course_dtl_id").val());
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadClass(CourseID) {
    ShowLoader();
    var postCall = $.post(commonData.BranchClass + "GetClassDDL", { "CourseID": CourseID });
    postCall.done(function (data) {
        $('#ClassName').empty();
        $('#ClassName').select2();
        $("#ClassName").append("<option value=" + 0 + ">---Select Class---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#ClassName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
            }
        }

        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#ClassName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
            LoadSubject($("#BranchClass_Class_dtl_id").val(), CourseID);
        }

        HideLoader();
    }).fail(function () {
        HideLoader();
    });
}

function LoadSubject(ClassID,CourseID) {
    ShowLoader();
    var postCall = $.post(commonData.BranchSubject + "GetSubjectDDL", { "ClassID": ClassID, "CourseID": CourseID});
    postCall.done(function (data) {
        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#SubjectName").append("<option value='" + data[i].Subject_dtl_id + "'>" + data[i].Subject.SubjectName + "</option>");
            }
        }
        if ($("#branchSubject_Subject_dtl_id").val() != "") {
            $('#SubjectName option[value="' + $("#branchSubject_Subject_dtl_id").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
        
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

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#ClassName").change(function () {
    var Data = $("#ClassName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
    LoadSubject(Data, $("#CourseName option:selected").val());
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    $('#branchSubject_Subject_dtl_id').val(Data);

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
            HideLoader();
            if (data.FacultyID >= 0) {
                ShowMessage("Faculty added Successfully.", "Success");
                window.location.href = "FacultyMaintenance?facultyID=0";
            } else {                
                ShowMessage('Faculty Already inserted!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
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

