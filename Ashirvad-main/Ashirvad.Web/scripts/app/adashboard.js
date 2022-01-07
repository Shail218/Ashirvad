/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    ShowLoader();
    var studenttbl = $("#circulartable").DataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false,
        "bAutoWidth": false,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "ordering": false,
        "language": {
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Circular/GetAllCircular",
            type: 'POST'
        },
        columns: [
            { "data": "CircularTitle" },
            { "data": "CircularId" },
            { "data": "CircularId" }
        ],
        "columnDefs": [
            {
                targets: 0,
                orderable: false,
                searchable: false
            },
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<a href= "' + full.FilePath.replace("https://mastermind.org.in", "") + '" id="paperdownload" target="_blank"> <img src="../ThemeData/images/icons8-preview-pane-24.png" /></a>'
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
                        data = '<a href= "' + full.FilePath.replace("https://mastermind.org.in", "") + '" id="paperdownload" download="' + full.FileName + '"> <img src="../ThemeData/images/icons8-desktop-download-24 (1).png" /></a>'
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

    var studenttbltask = $("#todotable").DataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false,
        "bAutoWidth": false,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "ordering": false,
        "language": {
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "/ToDo/GetAllTask",
            type: 'POST'
        },
        columns: [
            { "data": "UserInfo.Username" },
            { "data": "ToDoDescription" },
            { "data": "FilePath" },
            { "data": "FilePath" }
        ],
        "columnDefs": [
            {
                targets: 2,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<a href= "' + full.FilePath.replace("https://mastermind.org.in", "") + '" target="_blank"> <img src="../ThemeData/images/icons8-desktop-download-24 (1).png" /></a>'
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
                        data = '<a href= "' + full.FilePath.replace("https://mastermind.org.in", "") + '" download="' + full.ToDoFileName + '"> <img src="../ThemeData/images/icons8-desktop-download-24 (1).png" /></a>'
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

    var studenttblreminder = $("#remindertable").DataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false,
        "bAutoWidth": false,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "ordering": false,
        "language": {
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Reminder/GetAllReminder",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        columns: [
            { "data": "ReminderTime" },
            { "data": "ReminderDesc" }
        ],
        "columnDefs": [
            {
                targets: 0,
                orderable: false,
                searchable: false
            },
            {
                targets: 1,
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('textalign');
        }
    });
});
