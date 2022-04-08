/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#hdnBranchID").val() == 0) {
        ShowLoader();
        LoadBranch();
    } else {
        //LoadStudent(0);
        //LoadStudent(0);
            LoadCourse();
        GetAllClassDDL(0);
    }
    document.getElementById("StandardDiv").style.display = 'none';
    document.getElementById("ChartDiv").style.display = 'none';
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
        HideLoader();
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
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#StandardName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
            }
        }
        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
            LoadStudentByStandard($("#BranchClass_Class_dtl_id").val());
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
    });
}
$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    LoadStudentByStandard(Data);
    document.getElementById("ChartDiv").style.display = 'block';
});

function LoadStudentByStandard(stdid) {
    ShowLoader();
    var courseid = $("#CourseName option:selected").val()
    var postCall = $.post(commonData.StandardWiseChart + "StudentDataByStandard", { "StdID": stdid, "courseid": courseid });
    postCall.done(function (data) {
        $('#StudentName').empty();
        $('#StudentName').select2();
        $("#StudentName").append("<option value=" + 0 + ">---Select Student---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StudentName").append("<option value='" + data[i].StudentID + "'>" + data[i].Name + "</option>");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}
function LoadStudent(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.StandardWiseChart + "StudentData", { "branchID": branchID });
    postCall.done(function (data) {
        $('#StudentName').empty();
        $('#StudentName').select2();
        $("#StudentName").append("<option value=" + 0 + ">---Select Student---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StudentName").append("<option value='" + data[i].StudentID + "'>" + data[i].Name + "</option>");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetAllClassDDL(branchID) {
    var postCall = $.post(commonData.StandardWiseChart + "GetAllClassDDL", { "branchID": branchID });
    postCall.done(function (data) {
        HideLoader();
        document.getElementById("StandardDiv").style.display = 'block';
        document.getElementById("ChartDiv").style.display = 'block';
        $('#StandardData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    LoadStudent(Data);
    GetAllClassDDL(Data);
});

$("#StudentName").change(function () {
    window.location.href = "ProgressReportChart?StudentID=" + $("#StudentName option:selected").val();
});


