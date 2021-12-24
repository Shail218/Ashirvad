/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }
        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            GetAttendanceDetail(commonData.BranchID);
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
        HideLoader();
        $('#AttendanceData').html(data);
    }).fail(function () {
        HideLoader();
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

function EditAttendance() {
    var AttendanceData = [];
    Map = {};
    $("#attendancetable tbody tr").each(function () {
        var IsAbsent, IsPresent;
        var Remarks = $(this).find("#item_Remarks").val();
        var StudentID = $(this).find("#item_Student_StudentID").val();
        var detailID = $(this).find("#item_DetailID").val();
        var headerID = $(this).find("#item_HeaderID").val();
        var checked = $(this).find("#cb").prop("checked");
        if (checked) {
            IsAbsent = true;
            IsPresent = false;
        } else {
            IsAbsent = false;
            IsPresent = true;
        }
        if (StudentID != null) {
            Map = {
                IsAbsent: IsAbsent,
                IsPresent: IsPresent,
                Remarks: Remarks,
                Student: {
                    StudentID: StudentID
                },
                DetailID: detailID,
                HeaderID: headerID
            }
            AttendanceData.push(Map);
        }
    });
    $('#JsonData').val(JSON.stringify(AttendanceData));
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.AttendanceEntry + "AttendanceMaintenance", $('#attendence').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Attendance added Successfully.", "Success");
            window.location.href = "/AttendanceRegister/Index";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveAttendance(attendanceID) {
    if (confirm('Are you sure want to delete this Attendance?')) {
        ShowLoader();
        var postCall = $.post(commonData.AttendanceRegister + "RemoveAttendance", { "attendanceID": attendanceID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Attendance Removed Successfully.", "Success");
            location.reload();
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    GetAttendanceDetail($("#BranchName option:selected").val());
});
