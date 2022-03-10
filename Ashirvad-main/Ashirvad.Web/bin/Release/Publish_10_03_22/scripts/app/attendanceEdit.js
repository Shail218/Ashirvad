/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    //ShowLoader();

    //var table = $('#attendanceregistertable').DataTable({
    //    "bPaginate": true,
    //    "bLengthChange": false,
    //    "bFilter": true,
    //    "bInfo": true,
    //    "bAutoWidth": true,
    //    "proccessing": true,
    //    "sLoadingRecords": "Loading...",
    //    "sProcessing": true,
    //    "serverSide": true,
    //    "language": {
    //        processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
    //    },
    //    "ajax": {
    //        url: "" + GetSiteURL() + "/AttendanceRegister/CustomServerSideSearchAction",
    //        type: 'POST',
    //        dataFilter: function (data) {
    //            HideLoader();
    //            return data;
    //        }.bind(this)
    //    },
    //    "columns": [
    //        { "data": "AttendanceDate" },
    //        { "data": "Standard.Standard" },
    //        { "data": "BatchTypeText" },
    //        { "data": "AttendanceID" },
    //        { "data": "AttendanceID" }
    //    ],
    //    "columnDefs": [
    //        {
    //            targets: 0,
    //            render: function (data, type, full, meta) {
    //                if (type === 'display') {
    //                    data = ConvertMiliDateFrom(data)
    //                }
    //                return data;
    //            },
    //            orderable: false,
    //            searchable: false
    //        },
    //        {
    //            targets: 3,
    //            render: function (data, type, full, meta) {
    //                debugger;
    //                if (type === 'display') {
    //                    data =
    //                        '<a href="AttendanceRegister/GetAttendanceByID?attendanceID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
    //                }
    //                return data;
    //            },
    //            orderable: false,
    //            searchable: false
    //        },
    //        {
    //            targets: 4,
    //            render: function (data, type, full, meta) {
    //                if (type === 'display') {
    //                    data =
    //                        '<a onclick = "RemoveAttendance(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
    //                }
    //                return data;
    //            },
    //            orderable: false,
    //            searchable: false
    //        }
    //    ]
    //});
});



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


