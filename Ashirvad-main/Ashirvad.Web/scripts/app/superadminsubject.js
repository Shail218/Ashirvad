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
            url: "" + GetSiteURL() + "/SuperAdminSubject/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "SubjectName" },
            { "data": "SubjectID" },
            { "data": "SubjectID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a style="text-align:center !important;" href="SubjectMaintenance?subjectID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
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
                        data = '<a onclick = "RemoveSubject(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('textalign');
            $(tr.children[2]).addClass('textalign');
        },
    });

});

function SaveSubject() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.SuperAdminSubject + "SaveSubject", $('#fSubjectDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.SubjectID >= 0) {
                ShowMessage("Subject details saved!", "Success");
                setTimeout(function () { window.location.href = "SubjectMaintenance?subjectID=0" }, 2000);
            } else {
                ShowMessage("Subject Already Exists!!", "Error");
            }
        }).fail(function () {
            HideLoader();
        });
    }
}

function RemoveSubject(subjectId) {
    if (confirm('Are you sure want to delete this Subject?')) {
        ShowLoader();
        var postCall = $.post(commonData.SuperAdminSubject + "RemoveSubject", { "subjectID": subjectId });
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Subject Removed Successfully.", "Success");
                window.location.href = "SubjectMaintenance?subjectID=0";
            } else {
                ShowMessage("Subject is Already in used!!.", "Error");
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
        var IsEdit = $("#IsEdit").val();
        if (IsEdit == "True") {
            LoadClass($("#courseEntity_CourseID").val());
        }

        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadClass(CourseID) {
    var postCall = $.post(commonData.Class + "GetClassDDL", { "CourseID": CourseID });
    postCall.done(function (data) {
        $('#ClassName').empty();
        $('#ClassName').select2();
        $("#ClassName").append("<option value=" + 0 + ">---Select Class---</option>");
        if (data != 0) {
            for (i = 0; i < data.length; i++) {
                $("#ClassName").append("<option value='" + data[i].ClassID + "'>" + data[i].ClassName + "</option>");
            }
        }
        if ($("#classEntity_ClassID").val() != "") {
            $('#ClassName option[value="' + $("#classEntity_ClassID").val() + '"]').attr("selected", "selected");
        }

        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#courseEntity_CourseID').val(Data);
    LoadClass(Data);
});

$("#ClassName").change(function () {
    var Data = $("#ClassName option:selected").val();
    $('#classEntity_ClassID').val(Data);

});