/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />
$(document).ready(function () {
    ShowLoader();
    LoadCourse();
});

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

        if ($("#BranchCourse_course_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
            LoadClass($("#BranchCourse_course_dtl_id").val());
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
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
        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
            LoadStudentByStandard($("#BranchClass_Class_dtl_id").val());
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
    });
}
function UpdatePaymentStatus(paymentID, StudentID,status) {
    ShowLoader();
    var marks = $("#remarks_" + paymentID).val();
    if (marks != "" && marks != null) {
        var postCall = $.post(commonData.OnlinePaymentList + "UpdatePaymentDetails", { "paymentID": paymentID, "StudentID": StudentID, "Remarks":marks,"status": status });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                showStudentPaymentDetails(0, 0, StudentID);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    } else {
        HideLoader();
        ShowMessage("Please Enter Remarks!!", "Error");
    }
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    clearClass();
    clearStudent();
    LoadClass(Data);
    //showStudentPaymentDetails(Data, 0, 0);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    clearStudent();
    LoadStudentByStandard(Data);
    var Data1 = $("#CourseName option:selected").val();
    //showStudentPaymentDetails(Data1, Data, 0);
});
$("#StudentName").change(function () {
    var Data = $("#StudentName option:selected").val();
    var Data1 = $("#CourseName option:selected").val();
    var Data2 = $("#StandardName option:selected").val();
    showStudentPaymentDetails(Data1, Data2,Data);
});

function showStudentPaymentDetails(CourseId,ClassID,studentId) {
    ShowLoader();
    var postCall = $.post(commonData.OnlinePaymentList + "GetStudentPaymentData?CourseID=" + CourseId + "&ClassId=" + ClassID +"&StudentId=" + studentId);
    postCall.done(function (data) {
        HideLoader();
        $("#PaymentData").html('');
        $("#PaymentData").html(data);
    }).fail(function () {
        HideLoader();
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStudentByStandard(stdid) {
    ShowLoader();
    var courseid = $("#CourseName option:selected").val()
    var postCall = $.post(commonData.StandardWiseChart + "StudentDataByStandard", { "StdID": stdid, "courseid": courseid });
    postCall.done(function (data) {
        $('#StudentName').empty();
        $('#StudentName').select2();
        $("#StudentName").append("<option value=" + 0 + ">---Select Student---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StudentName").append("<option value='" + data[i].StudentID + "'>" + data[i].Name + "</option>");
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}
function clearStudent() {
    $('#StudentName').empty();
    $('#StudentName').select2();
    $("#StudentName").append("<option value=" + 0 + ">---Select Student---</option>");
}
function clearClass() {
    $('#StandardName').empty();
    $('#StandardName').select2();
    $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
}
function clearcourse() {
    $('#CourseName').empty();
    $('#CourseName').select2();
    $("#CourseName").append("<option value=" + 0 + ">---Select Standard---</option>");
}