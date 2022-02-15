/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#hdnBranchID").val() == 0) {
        ShowLoader();
        LoadBranch();
    } else {
        LoadStudent(0);
        LoadStandard(0);
        LoadBatch();
    }
    LoadCourse();
    document.getElementById("StudentDiv").style.display = 'none';
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

        if ($("#hdnCourseID").val() != "") {
            $('#CourseName option[value="' + $("#hdnCourseID").val() + '"]').attr("selected", "selected");
            LoadClass($("#hdnCourseID").val());
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

        HideLoader();
    }).fail(function () {
        HideLoader();
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

function LoadStandard(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.BatchWiseChart + "GetAllStandard", { "branchID": branchID });
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value='" + data[i].StandardID + "'>" + data[i].Standard + "</option>");
        }
        var url = new URL(window.location.href);
        var search_params = url.searchParams;
        var StandardID = search_params.get('StandardID');
        if (StandardID != 0) {
            $('#StandardName option[value="' + StandardID + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadBatch() {
    $('#BatchTime').empty();
    $('#BatchTime').select2();
    $("#BatchTime").append("<option value='-1'>---Select BatchTime---</option>");
    $("#BatchTime").append("<option value='0'>All</option>");
    $("#BatchTime").append("<option value='1'>Morning</option>");
    $("#BatchTime").append("<option value='2'>Afternoon</option>");
    $("#BatchTime").append("<option value='3'>Evening</option>");
    var url = new URL(window.location.href);
    var search_params = url.searchParams;
    var batchID = search_params.get('batchID');
    var StandardID = search_params.get('StandardID');
    if (batchID != -1) {
        GetAllStudent(StandardID, $("#hdnBranchID").val(), batchID);
        $('#BatchTime option[value="' + batchID + '"]').attr("selected", "selected");
        $("#hdnStandardID").val(StandardID);
        document.getElementById("StudentDiv").style.display = 'block';
    }
}

function GetAllClassDDL(branchID) {
    var postCall = $.post(commonData.BatchWiseChart + "GetAllStandard", { "branchID": branchID });
    postCall.done(function (data) {
        HideLoader();
        $('#StandardData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

function GetAllStudent(stdID, branchID, batchID) {
    var postCall = $.post(commonData.ListOfStudent + "GetStudentContent", {
        "stdID": stdID, "branchID": branchID, "batchID": batchID
    });
    postCall.done(function (data) {
        HideLoader();
        document.getElementById("StudentDiv").style.display = 'block';
        $('#StudentData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $("#hdnBranchID").val(Data);
    document.getElementById("StudentDiv").style.display = 'none';
    $('#BatchTime').val(-1);
    LoadStudent(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $("#hdnStandardID").val(Data);
    document.getElementById("StudentDiv").style.display = 'none';
    LoadBatch();
});

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $("#hdnBatchTimeID").val(Data);
    document.getElementById("StudentDiv").style.display = 'block';
    GetAllStudent($("#hdnStandardID").val(), $("#hdnBranchID").val(), Data);
});

function GoToList() {
    window.location.href = "ProgressReportChart?StudentID=" + $("#StandardName option:selected").val();
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#hdnCourseID').val(Data);
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#hdnStandardID').val(Data);
});


