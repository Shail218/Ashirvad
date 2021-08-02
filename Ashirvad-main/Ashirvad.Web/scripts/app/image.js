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

    $("#studenttbl tr").each(function () {       
        var elemImg = $(this).find("#Img");
        var photoID = $(this).find("#item_UniqueID").val();
        if (elemImg.length > 0 && photoID.length > 0) {
            var postCall = $.post(commonData.Photos + "GetPhoto", { "photoID": photoID });
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

function SavePhotos() {   
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fPhotosDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }  
        AjaxCallWithFileUpload(commonData.Photos + 'SavePhotos', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage('Photos details saved!', 'Success');
                window.location.href = "PhotosMaintenance?photoID=0";
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

function RemovePhotos(branchID) {
    if (confirm('Are you sure want to delete this Image?')) {
        ShowLoader();
        var postCall = $.post(commonData.Photos + "RemovePhotos", { "photoID": branchID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Photos Removed Successfully.", "Success");
            window.location.href = "PhotosMaintenance?photoID=0";
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