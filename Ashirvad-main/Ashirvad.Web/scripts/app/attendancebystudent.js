/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    var url = new URL(window.location.href);
    var search_params = url.searchParams;
    var StudentID = search_params.get('studentID');
    var type = search_params.get('type');
    GetAllStudent(StudentID);
    GetStudentAttendanceDetails(StudentID, type);
});

function GetAllStudent(studentID, type) {
    var postCall = $.post(commonData.AttendanceByStudent + "GetStudentContentByID", {
        "StudentID": studentID
    });
    postCall.done(function (data) {
        $('#PersonalData').html(data);
    }).fail(function () {
        HideLoader();
    });
}

function GetStudentAttendanceDetails(StudentID, type) {
    var postCall = $.post(commonData.AttendanceByStudent + "GetStudentAttendanceDetails?StudentID=" + StudentID + "&type=" + type);
    postCall.done(function (data) {
        HideLoader();
        $('#FilteredData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}