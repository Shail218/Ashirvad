/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadCourse();
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
            url: "" + GetSiteURL() + "/Class/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "ClassName" },
            { "data": "courseEntity.CourseName" },
            { "data": "ClassID" },
            { "data": "ClassID" }
        ],
        "columnDefs": [
            {
                targets: 2,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a style="text-align:center !important;" href="ClassMaintenance?classID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
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
                        data = '<a onclick = "RemoveClass(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[2]).addClass('textalign');
            $(tr.children[3]).addClass('textalign');
        },
    });

});


function SaveClass() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Class + "SaveClass", $('#fClassDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.ClassID >= 0) {
                ShowMessage("Class details saved!", "Success");
                setTimeout(function () { window.location.href = "ClassMaintenance?classID=0" }, 2000);
            } else {
                ShowMessage("Class Already Exists!!", "Error");
            }
        }).fail(function () {
            HideLoader();
        });
    }
}

function RemoveClass(classId) {
    if (confirm('Are you sure want to delete this Class?')) {
        ShowLoader();
        var postCall = $.post(commonData.Class + "RemoveClass", { "classID": classId });
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Class Removed Successfully.", "Success");
                window.location.href = "ClassMaintenance?classID=0";
            }
            else {
                ShowMessage("Class is Already in used!!.", "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function LoadCourse() {
    var postCall = $.post(commonData.Class + "CourseData");
    postCall.done(function (data) {
        $('#CourseName').empty();
        $('#CourseName').select2();
        $("#CourseName").append("<option value=" + 0 + ">---Select Course---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#CourseName").append("<option value='" + data[i].CourseID + "'>" + data[i].CourseName + "</option>");
            }
        }
        if ($("#courseEntity_CourseID").val() != "") {
            $('#CourseName option[value="' + $("#courseEntity_CourseID").val() + '"]').attr("selected", "selected");
        }

        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#courseEntity_CourseID').val(Data);
});
