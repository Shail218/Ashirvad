/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    $("#datepickermarks").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        defaultDate: new Date(),
    });

    table = $('#marksregistertable').DataTable({
        "bLengthChange": false
    });

    LoadCourse();

});

function LoadBranch(onLoaded) {
    ShowLoader();
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

function LoadSubject(branchID) {
    var postCall = $.post(commonData.Subject + "SubjectDataByTestDate", { "TestDate": branchID });
    postCall.done(function (data) {

        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SubjectName").append("<option value=" + data[i].SubjectID + "_" + data[i].testID + ">" + data[i].Subject + "</option>");
        }

        if ($("#BranchSubject_Subject_dtl_id").val() != "") {
            $('#SubjectName option[value="' + $("#BranchSubject_Subject_dtl_id").val() + '"]').attr("selected", "selected");
        }

    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadTestDates(BatchType) {
    var BranchID = $("#Branch_Name").val();
    var STD = $('#BranchClass_Class_dtl_id').val();
    var Courseid = $('#BranchCourse_course_dtl_id').val();
    var BatchType = BatchType;
    if (BranchID > 0 && BatchType > 0 && STD > 0) {
        var postCall = $.post(commonData.TestPaper + "GetTestDatesByBatch", { "BranchID": BranchID, "BatchType": BatchType, "stdID": STD, "courseid": Courseid });
        postCall.done(function (data) {
            $('#testddl').empty();
            $('#testddl').select2();
            $("#testddl").append("<option value=" + 0 + ">---Select Test Date---</option>");
            for (i = 0; i < data.length; i++) {
                var test = ConvertDateFrom(data[i].TestDate);
                var TestDate = convertddmmyyyy(test);
                $("#testddl").append("<option value='" + data[i].TestID + "'>" + TestDate + "</option>");
            }
            HideLoader();
        }).fail(function () {
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
    else {
        $('#testddl').empty();
        $('#testddl').select2();
    }
}

function LoadStudentDetails() {
    ShowLoader();
    var STD = $('#testEntityInfo_TestID').val();
    var Subject = $('#BranchSubject_Subject_dtl_id').val();
    var BatchType = $('#BatchType').val();
    var courseid = $("#BranchCourse_course_dtl_id").val();
    table.destroy();
    table = $('#marksregistertable').DataTable({
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
            url: "" + GetSiteURL() + "/ResultRegister/CustomServerSideSearchAction?Std=" + STD + "&Batch=" + BatchType + "&courseid=" + courseid + "&MarksID=" + Subject + "",
            type: 'POST',
            "data": function (d) {
                HideLoader();
                d.Std = STD;
                d.Batch = BatchType;
                d.MarksID = Subject;
                d.courseid = courseid;
            }
            },
            "columns": [
                { "data": "student.Name" },
                { "data": "BranchSubject.Subject.SubjectName" },
                { "data": "testEntityInfo.Marks" },
                { "data": "AchieveMarks" },
                { "data": "MarksID" }
            ],
            "columnDefs": [
                {
                    targets: 3,
                    render: function (data, type, full, meta) {
                        if (type === 'display') {
                            data = '<input value="' + data + '" name="' + data + '" class="form-control customwidth required" alt="Achieve Mark" autocomplete="off" Id="Marks_' + full.MarksID + '" />'
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                },
                {
                    targets: 4,
                    render: function (data, type, full, meta) {
                        if (type === 'display') {
                            data =
                                `<a onclick = "UpdateMarks(` + data + `,` + full.student.StudentID + `)" class="ladda-button mb-2 mr-2 btn btn-primary" data-style="expand-left"> <span class="ladda-label">
                        Save
                    </span >
        <span class="ladda-spinner"></span></a >`
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                }
            ]
        });
}

function UpdateMarks(MarksID, StudentID) {
    ShowLoader();
    var marks = $("#Marks_" + MarksID).val();
    if (marks != "" && marks != null) {
        var postCall = $.post(commonData.MarksRegister + "UpdateMarksDetails", { "MarksID": MarksID, "StudentID": StudentID, "AchieveMarks": marks });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                LoadStudentDetails();
                //setTimeout(function () { window.location.href = "GetAllAchieveMarks" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    } else {
        HideLoader();
        ShowMessage("Please Enter Achieve Marks!!", "Error");
    }
}

function clearsubject() {
    $('#SubjectName').empty();
    $('#SubjectName').select2();
    $("#SubjectName").append("<option value=" + 0 + ">---Select Subject---</option>");
}

function clearclass() {
    $('#StandardName').empty();
    $('#StandardName').select2();
    $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadSubject(Data);
    LoadStandard(Data);
});

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    clearclass();
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    var SPData = Data.split('_');
    var sub = SPData[0];
    var test = SPData[1];
    $('#testEntityInfo_TestID').val(parseInt(test));
    $('#BranchSubject_Subject_dtl_id').val(sub);
    LoadStudentDetails();
});

$("#Batchtime").change(function () {
    var Data = $("#Batchtime option:selected").val();
    $('#BatchType').val(Data);
    LoadTestDates(Data);
});

$("#testddl").change(function () {
    var Data = $("#testddl option:selected").val();
    var Text = $("#testddl option:selected").text();
    var Date1 = ConvertData(Text);
    LoadSubject(Date1);
});