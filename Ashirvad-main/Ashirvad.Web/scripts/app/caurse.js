/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    if ($("#CourseID").val() > 0) {
        $("#fuCaurseImage").addClass("editForm");
    }

    ShowLoader();
    var table = $('#studenttbl').DataTable({
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
            url: "" + GetSiteURL() + "/Caurse/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "CourseName" },
            { "data": "filepath" },
            { "data": "CourseID" },
            { "data": "CourseID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data = (data == null || data == "https://mastermind.org.in") ? '<img src="../ThemeData/images/Default.png" id="branchImg" style="height:60px;width:60px;margin-left:20px;" />' : '<img src = "' + data + '" style="height:60px;width:60px;margin-left:20px;"/>'
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
                            '<a style="text-align:center !important;" href="CourseMaintenance?courseID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 3,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<a onclick = "removecourse(' + data + ')"><img src = "../themedata/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('image - cls');
        },
    });

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
            if (data) {
                ShowMessage("Course Removed Successfully.", "Success");
                window.location.href = "CourseMaintenance?courseID=0";
            }
            else {
                ShowMessage("Course is Already in used!!!.", "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}