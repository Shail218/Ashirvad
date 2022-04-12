/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();

    var table = $('#attendanceregistertable').DataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            url: "" + GetSiteURL() + "/AttendanceRegister/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "AttendanceDate" },
            { "data": "BranchCourse.course.CourseName" },
            { "data": "BranchClass.Class.ClassName" },
            { "data": "BatchTypeText" },
            { "data": "PresentCount" },
            { "data": "AbsentCount" },
            { "data": "TotalCount" },
            { "data": "AttendanceRemarks" },
            { "data": "AttendanceID" },
            { "data": "AttendanceID" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = ConvertMiliDateFrom(data)
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 8,
                render: function (data, type, full, meta) {
                    var check = GetUserRights('AttendanceRegister');
                    if (type === 'display') {
                        if (check[0].Create) {
                            data =
                                '<a onclick="RedirectAttendance(' + data + ')"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 9,
                render: function (data, type, full, meta) {
                  
                    var check = GetUserRights('AttendanceRegister');
                   
                    if (type === 'display') {
                        if (check[0].Delete) {
                            data =
                                '<a onclick = "RemoveAttendance(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                    }
                    
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });
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
    });
}

function GetAttendanceByID(attendanceID) {
    var postCall = $.post(commonData.AttendanceRegister + "GetAttendanceByID", { "attendanceID": attendanceID });
    postCall.done(function (data) {
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
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                window.location.href = "/AttendanceRegister/Index";
            } else {
                ShowMessage(data.Message, "Error");
            }
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

function ConvertMiliDateFrom(date) {
    if (date != null) {
        var sd = date.split("/Date(");
        var sd2 = sd[1].split(")/");
        var date1 = new Date(parseInt(sd2[0]));
        var d = date1.getDate();
        var m = date1.getMonth() + 1;
        var y = date1.getFullYear();
        var hr = date1.getHours();
        var min = date1.getMinutes();
        var sec = date1.getSeconds();

        if (parseInt(d) < 10) {
            d = "0" + d;
        }
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        var Final = d + "-" + m + "-" + y + " ";
        var d = date1.toString("dd/MM/yyyy HH:mm:SS");
        return Final;
    }
    return "";
}

function RedirectAttendance(data) {
    window.location.href = GetSiteURL() + "AttendanceRegister/GetAttendanceByID?attendanceID=" + data+"";
}
