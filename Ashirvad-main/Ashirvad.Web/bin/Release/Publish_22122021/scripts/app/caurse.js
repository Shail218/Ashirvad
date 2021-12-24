/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    if ($("#CourseID").val() > 0) {
        $("#fuCaurseImage").addClass("editForm");
    }
});

function SaveCourse() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fCaurseDetail');
        var formData = new FormData(frm[0]);
        formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        AjaxCallWithFileUpload(commonData.Course + 'SaveCourse', formData, function (data) {
            HideLoader();
            if (data.CourseID >= 0) {
                ShowMessage('Course details saved!', 'Success');
                window.location.href = "CourseMaintenance?courseID=0";
            }
            else {
                ShowMessage('Course Already Exists!!!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemoveCourse(courseId) {
    if (confirm('Are you sure want to delete this Course?')) {
        ShowLoader();
        var postCall = $.post(commonData.Course + "RemoveCourse", { "courseID": courseId });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Course Removed Successfully.", "Success");
            window.location.href = "CourseMaintenance?courseID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}