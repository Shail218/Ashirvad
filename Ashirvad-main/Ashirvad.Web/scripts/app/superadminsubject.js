/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
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
            { "data": "SubjectID" },
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
            ShowMessage("Subject Removed Successfully.", "Success");
            window.location.href = "SubjectMaintenance?subjectID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}