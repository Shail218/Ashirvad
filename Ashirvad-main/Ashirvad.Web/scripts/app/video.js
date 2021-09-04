/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {

    if ($("#UniqueID").val() > 0) {
        $("#fuImage").addClass("editForm");
    }

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
    }

});

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

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

$("#BranchName").change(function () {    
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
});

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
