/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#hdnBranchID").val() == 0) {
        ShowLoader();
        LoadBranch();
    } else {
        LoadStudent(0);
    }
    document.getElementById("PersonalDiv").style.display = 'none';
    document.getElementById("ReportDiv").style.display = 'none';
    document.getElementById("TestDiv").style.display = 'none';
    document.getElementById("HomeworkDiv").style.display = 'none';
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
        var url = new URL(window.location.href);
        var search_params = url.searchParams;
        var StudentID = search_params.get('StudentID');
        if (StudentID != 0) {
            $('#StudentName option[value="' + StudentID + '"]').attr("selected", "selected");
            GetAllStudent(StudentID);
        } else {
            HideLoader();
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetAllStudent(studentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetStudentContentByID", {
        "StudentID": studentID
    });
    postCall.done(function (data) {
        document.getElementById("PersonalDiv").style.display = 'block';
        GetTotalCountList(studentID);
        $('#PersonalData').html(data);
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

$("#StudentName").change(function () {
    var Data = $("#StudentName option:selected").val();
    GetAllStudent(Data);
});

function GetTotalCountList(studentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetTotalCountList", {
        "StudentID": studentID
    });
    postCall.done(function (data) {
        document.getElementById("ReportDiv").style.display = 'block';
        GetStudentAttendanceDetails(studentID);
        $('#ReportData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

function GetStudentAttendanceDetails(StudentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetStudentAttendanceDetails?StudentID=" + StudentID);
    postCall.done(function (response) {
        document.getElementById("ReportDiv").style.display = 'block';
        var chart = new CanvasJS.Chart("Reportcontainer", {
            animationEnabled: true,
            data: [{
                type: "pie",
                startAngle: 240,
                yValueFormatString: "##0.00\"%\"",
                indexLabel: "{label} {y}",
                dataPoints: response.dataPoints
            }]
        });
        chart.render();
        GetTestByStudent2(StudentID);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetHomeworkByStudent(StudentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetHomeworkByStudent?branchID=0&StudentID=" + StudentID);
    postCall.done(function (response) {
        HideLoader();
        document.getElementById("HomeworkDiv").style.display = 'block';
        var chart = new CanvasJS.Chart("homeworkContainer", {
            animationEnabled: true,
            theme: "light2",
            data: [{
                type: "column",
                showInLegend: true,
                dataPoints: response.dataPoints
            }]
        });
        chart.render();
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetHomeworkByStudent2(StudentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetHomeworkByStudent2?branchID=0&StudentID=" + StudentID);
    postCall.done(function (data) {
        GetHomeworkByStudent(StudentID);
        $('#HomeworkData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetTestByStudent(StudentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetTestByStudent?branchID=0&StudentID=" + StudentID);
    postCall.done(function (response) {
        document.getElementById("TestDiv").style.display = 'block';
        var chart = new CanvasJS.Chart("testContainer", {
            animationEnabled: true,
            theme: "light2",
            data: [{
                type: "pie",
                startAngle: 240,
                yValueFormatString: "##0.00\"%\"",
                indexLabel: "{label} {y}",
                dataPoints: response.testdataPoints
            }]
        });
        chart.render();
        GetHomeworkByStudent2(StudentID);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetTestByStudent2(StudentID) {
    var postCall = $.post(commonData.ProgressReportChart + "GetTestByStudent2?branchID=0&StudentID=" + StudentID);
    postCall.done(function (data) {
        GetTestByStudent(StudentID);
        $('#TestData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GoToAttendance(type) {
    window.location.href = "AttendanceByStudent?studentID=" + $("#StudentName option:selected").val() + "&type=" + type;
}