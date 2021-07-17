/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
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

function GetAttendanceDetail(branchID) {
    
    var postCall = $.post(commonData.AttendanceRegister + "GetAllAttendanceByBranch", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#AttendanceData').html(data);
        //ShowMessage("Student added Successfully.", "Success");
        //window.location.href = "StudentMaintenance?studentID=0";
    }).fail(function () {
        
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetAttendanceByID(attendanceID) {
    
    var postCall = $.post(commonData.AttendanceRegister + "GetAttendanceByID", { "attendanceID": attendanceID });
    postCall.done(function (data) {
        
        //$('#AttendanceData').html(data);
        //ShowMessage("Attendance Removed Successfully.", "Success");
        //window.location.href = "AttendanceRegister/Index";
    }).fail(function () {
        
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function RemoveAttendance(attendanceID) {
    
    var postCall = $.post(commonData.AttendanceRegister + "RemoveAttendance", { "attendanceID": attendanceID });
    postCall.done(function (data) {
        
        ShowMessage("Attendance Removed Successfully.", "Success");
        window.location.href = "AttendanceRegister/Index";
    }).fail(function () {
        
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    GetAttendanceDetail($("#BranchName option:selected").val());
});
