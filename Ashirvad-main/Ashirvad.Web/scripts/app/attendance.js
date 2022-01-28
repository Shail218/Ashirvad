/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    $("#datepickerattendance").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    table = $('#attendancetable').DataTable({
        "bLengthChange": false
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
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }

        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
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

function ValidateAttendanceData() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#AttendanceDate").val();
        $("#AttendanceDate").val(ConvertData(date1));
        var postCall = $.post(commonData.AttendanceEntry + "VerifyAttendanceRegister", $('#fAttendanceReportDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                GetStudentDetail();
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            //ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function GetStudentDetail() {    
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var Course = $('#BranchCourse_course_dtl_id').val();
        var STD = $('#BranchClass_Class_dtl_id').val();
        var BatchTime = $('#BatchTypeID').val();
        table.destroy();
        table = $('#attendancetable').DataTable({
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
                url: "" + GetSiteURL() + "/AttendanceEntry/CustomServerSideSearchAction?STD=" + STD + "&courseid=" + Course + "&BatchTime=" + BatchTime + "",
                type: 'POST',
                "data": function (d) {
                    HideLoader();
                    d.STD = STD;
                    d.BatchTime = BatchTime;
                }
            },
            "columns": [
                { "data": "Name" },
                { "data": "StudentID" }
            ],
            "columnDefs": [
                {
                    targets: 1,
                    render: function (data, type, full, meta) {
                        if (type === 'display') {
                            data = `<input type="checkbox" value="` + data + `" name="cb" id = "cb"/> <span style="margin-left:20px;">Absent</span>
                                <input hidden value = `+ full.StudentID +` Id = "StudentID" />
                                <input hidden value = `+ full.GrNo +` Id = "GrNo" />`
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                },
                {
                    targets: 2,
                    render: function (data, type, full, meta) {
                        if (type === 'display') {
                            data =
                                '<input name="Remarks" class = "remark" alt="Remarks" autocomplete="off" id="Remarks" />'
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                }
            ]
        });
    }
}

function SaveAttendance() {
    
    var AttendanceData = [];
    Map = {};
    $("#attendancetable tbody tr").each(function () {       
        var IsAbsent, IsPresent;
        var Remarks = $(this).find("#Remarks").val();
        var StudentID = $(this).find("#StudentID").val();
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
                }
            }
            AttendanceData.push(Map);
        }
    });
    //var date1 = $("#AttendanceDate").val();
    //$("#AttendanceDate").val(ConvertData(date1));
    $('#JsonData').val(JSON.stringify(AttendanceData));
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader(); 
        var postCall = $.post(commonData.AttendanceEntry + "AttendanceMaintenance", $('#fAttendanceReportDetail').serialize());
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

function RemoveStudent(studentID) {
    
    var postCall = $.post(commonData.Student + "RemoveStudent", { "studentID": studentID });
    postCall.done(function (data) {
        
        ShowMessage("Student Removed Successfully.", "Success");
        window.location.href = "StudentMaintenance?studentID=0";
    }).fail(function () {
        
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
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

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $('#BatchTypeID').val(Data);
});
