/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    if ($("#UniqueID").val() > 0) {
        $("#fuImage").addClass("editForm");
    }
    var check = GetUserRights('Video');
    var table = $('#videotable').DataTable({
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
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Videos/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "FilePath"},
            { "data": "Remarks" },
            { "data": "UniqueID" },
            { "data": "UniqueID" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<a href= "' + full.FilePath + '" id="videoDownload" download="' + full.FileName + '"> <img src="../ThemeData/images/cloud-download-alt-solid-svg.png" /></a>'
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
                        if (check[0].Create) {
                            data =
                                '<a href="VideosMaintenance?videoID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                       
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
                        if (check[0].Delete) {
                            data =
                                '<a onclick = "RemoveVideos(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                       
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });

});

function SaveVideo() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fVideosDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Videos + 'SaveVideos', formData, function (data) {

            if (data) {
                HideLoader();
                ShowMessage('Videos details saved!', 'Success');
                window.location.href = "VideosMaintenance?videoID=0";
            }
            else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
            ShowMessage('An unexpected error occcurred while processing request!', 'Error');
        });
    }
}

function RemoveVideos(branchID) {
    if (confirm('Are you sure want to delete this Video?')) {
        ShowLoader();
        var postCall = $.post(commonData.Videos + "RemoveVideos", { "videoID": branchID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Videos Removed Successfully.", "Success");
            window.location.href = "VideosMaintenance?videoID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function DownloadVideo(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.Videos + "DownloadVideo", { "videoID": branchID });
    postCall.done(function (data) {
        HideLoader();
        if (data != null) {
            var a = document.createElement("a"); //Create <a>
            a.href = "data:video/mp4;base64," + data[1]; //Image Base64 Goes here
            a.download = data[2]; //File name Here
            a.click(); //Downloaded file
        }

    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}
