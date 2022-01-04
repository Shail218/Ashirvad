/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#CircularId").val() > 0) {
        $("#fuCircularFile").addClass("editForm");
    }
    ShowLoader();
    var studenttbl = $("#circulartble").DataTable({
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
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Circular/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        columns: [
            { "data": "CircularTitle" },
            { "data": "FilePath" },
            { "data": "CircularId" },
            { "data": "CircularId" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<a href= "' + full.FilePath.replace("https://mastermind.org.in", "") + '" id="paperdownload" download="' + full.FileName + '"> <img src="../ThemeData/images/icons8-desktop-download-24 (1).png" /></a>'
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
                            '<a href="CircularMaintenance?circularID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
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
                        data =
                            '<a onclick = "RemoveCircular(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
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
            $(tr.children[3]).addClass('textalign');
        }
    });
});

function SaveCircular() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fCircularDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Circular + 'SaveCircular', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage('Circular details saved!', 'Success');
                window.location.href = "CircularMaintenance?circularID=0";
            }
            else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemoveCircular(circularID) {
    if (confirm('Are you sure want to delete this Circular?')) {
        ShowLoader();
        var postCall = $.post(commonData.Circular + "RemoveCircular", { "circularID": circularID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Circular Removed Successfully.", "Success");
            window.location.href = "CircularMaintenance?circularID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}
