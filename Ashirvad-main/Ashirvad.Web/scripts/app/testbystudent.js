/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    var url = new URL(window.location.href);
    var search_params = url.searchParams;
    var StudentID = search_params.get('studentID');
    var subjectID = search_params.get('subjectID');
    GetAllStudent(StudentID);
    GetTestByStudent(StudentID, subjectID);
});

function GetAllStudent(studentID) {
    var postCall = $.post(commonData.AttendanceByStudent + "GetStudentContentByID", {
        "StudentID": studentID
    });
    postCall.done(function (data) {
        $('#PersonalData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

function GetTestByStudent(studentid, subjectid) {
    var table = $('#categorytable').DataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": "Processing...",
        "serverSide": true,
        "language": {
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "TestByStudent/GetTestByStudent?studentid=" + studentid + "&subjectid=" + subjectid,
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "testEntityInfo.TestDate" },
            { "data": "SubjectInfo.Subject" },
            { "data": "testEntityInfo.Marks" },
            { "data": "AchieveMarks" },
            { "data": "Percentage" }
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
                orderable: false
            }
        ],
    });
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
