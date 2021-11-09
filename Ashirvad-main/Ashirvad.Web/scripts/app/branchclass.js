/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    //ShowLoader();

    LoadCourse(function () {
        if ($("#CourseID").val() != "") {
            $('#CourseName option[value="' + $("#CourseID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#CourseID").val() != "") {
        $('#CourseName option[value="' + $("#CourseID").val() + '"]').attr("selected", "selected");
    }
});

function LoadCourse(onLoaded) {
    var postCall = $.post(commonData.Course + "CourseData");
    postCall.done(function (data) {
        $('#CourseName').empty();
        $('#CourseName').select2();
        $("#CourseName").append("<option value=" + 0 + ">---Select Course---</option>");
        for (i = 0; i < data.length; i++) {
            $("#CourseName").append("<option value='" + data[i].CourseID + "'>" + data[i].CourseName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}