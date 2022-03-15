/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    table = $("#studenttbl").DataTable({
        "bLengthChange": false,
    });
    $("#datepickerfromdate").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    $("#datepickertodate").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
    }

    if ($("#BatchTypeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTypeID").val() + '"]').attr("selected", "selected");
        LoadStudent($("#BatchTypeID").val());
    }

    LoadCourse();
});

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value='" + data[i].BranchID + "'>" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
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
                if (data.length == 1) {
                    $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
                    $('#CourseName option[value="' + data[i].course_dtl_id + '"]').attr("selected", "selected");
                    $('#BranchCourse_course_dtl_id').val(data[i].course_dtl_id);
                } else {
                    $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
                }
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
                if (data.length == 1) {
                    $("#StandardName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
                    $('#StandardName option[value="' + data[i].Class_dtl_id + '"]').attr("selected", "selected");
                    $('#BranchClass_Class_dtl_id').val(data[i].Class_dtl_id);
                } else {
                    $("#StandardName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
                }
            }
        }

        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#StandardName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
    });
}

function LoadStudent(batchtime) {
    ShowLoader();
    var std_id = $("#BranchClass_Class_dtl_id").val();
    var course_id = $("#BranchCourse_course_dtl_id").val();
    var postCall = $.post(commonData.AttendanceReport + "StudentData", { "std": std_id, "courseid": course_id, "BatchTime": batchtime });
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

function SaveReport() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#From_Date").val();
        var fromdate = ConvertData(date1);
        var date2 = $("#To_Date").val();
        var todate = ConvertData(date2);
        var standard = $("#BranchClass_Class_dtl_id").val();
        var Course = $("#BranchCourse_course_dtl_id").val();
        var batchtime = $("#BatchTypeID").val();
        var student_id = $("#studentEntity_Name").val();

        table.destroy();
        table = $('#studenttbl').DataTable({
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
                url: "" + GetSiteURL() + "/AttendanceReport/CustomServerSideSearchAction",
                type: 'POST',
                "data": function (d) {

                    HideLoader();
                    d.FromDate = fromdate;
                    d.ToDate = todate;
                    d.StandardId = standard;
                    d.BatchTime = batchtime;
                    d.studentid = student_id;
                    d.courseid = Course;
                }
            },
            "columns": [
                { "data": "AttendanceDate" },
                { "data": "Branch.BranchName" },
                { "data": "BranchCourse.course.CourseName" },
                { "data": "BranchClass.Class.ClassName" },
                { "data": "BatchTypeText" },
                { "data": "AttendanceDetail" },
                { "data": "AttendanceDetail" }

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
                    targets: 5,
                    render: function (data, type, full, meta) {
                        if (type === 'display') {
                            data = full.AttendanceDetail[0].IsAbsent == true ? "<p style='font-color=red;'>Absent</p>" : "<p style='font-color=green;'>Present</p>"
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                },
                {
                    targets: 6,
                    render: function (data, type, full, meta) {

                        if (type === 'display') {
                            data = full.AttendanceDetail[0].Remarks
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                }

            ],

        });
    }
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadStandard(Data);
});

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
});

$("#StudentName").change(function () {
    var Data = $("#StudentName option:selected").val();
    $('#studentEntity_Name').val(Data);
});

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $('#BatchTypeID').val(Data);
    LoadStudent(Data);
});


function format(d) {
    // `d` is the original data object for the row
    var tabledata = tabletd(d);
    return `<div style = "display:none">
                            <div style="max-height: 200px; overflow-y: scroll !important"><table style="width: 100%;" id="subcategorytbl2" class="table table-bordered dataTable no-footer">
                                <thead>
                                    <tr style="background-color:#005cbf;font-style:inherit;color:aliceblue">

                                       <th>
                                            Student Name
                                        </th>
                                        <th>
                                            Absent / Present
                                        </th>
                                        <th>
                                            Remark
                                        </th>
                                        

                                    </tr>
                                </thead>

                                <tbody>`+
        tabledata +
        `</tbody>
                            </table>
                            </div>

                </div> `;
}

function tabletd(d) {

    var data = ``;
    for (var i = 0; i < d.length; i++) {
        var Name = d[i].Student.FirstName + " " + d[i].Student.LastName;
        var Status = d[i].IsAbsent == true ? "Absent" : "Present";
        var remark = d[i].Remarks;
        data = data +
            `<tr>
             <td>
             `+ Name + `
             </td>
              <td>
             `+ Status + `
             </td>
            <td>
             `+ remark + `
             </td>
            </tr>`;
    }
    return data;
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