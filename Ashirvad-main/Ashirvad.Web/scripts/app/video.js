﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
    }

    $("#studenttbl tr").each(function () {
        
        var elemImg = $(this).find("#Img");
        var photoID = $(this).find("#item_UniqueID").val();
        if (elemImg.length > 0 && photoID.length > 0) {
            var postCall = $.post(commonData.Videos + "GetPhoto", { "videoID": photoID });
            postCall.done(function (data) {
                $(elemImg).attr('src', data);
            }).fail(function () {
                $(elemImg).attr('src', "../ThemeData/images/Default.png");
            });
        }
    });

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

function SaveVideos() {
    
    var isSuccess = ValidateData('dInformation');

    if (isSuccess) {
        var frm = $('#fVideosDetail');
        var formData = new FormData(frm[0]);
        formData.append('ImageFile', $('input[type=file]')[0].files[0]);

        AjaxCallWithFileUpload(commonData.Videos + 'SaveVideos', formData, function (data) {
            
            if (data) {
                ShowMessage('Videos details saved!', 'Success');
                window.location.href = "VideosMaintenance?videoID=0";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {

        });
    }
}

function RemoveVideos(branchID) {
    var postCall = $.post(commonData.Videos + "RemoveVideos", { "videoID": branchID });
    postCall.done(function (data) {
        ShowMessage("Videos Removed Successfully.", "Success");
        window.location.href = "VideosMaintenance?videoID=0";
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
});