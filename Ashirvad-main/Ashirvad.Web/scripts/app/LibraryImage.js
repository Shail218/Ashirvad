/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadCategory(commonData.BranchID);
        }

    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadCategory($("#BranchInfo_BranchID").val());
    }

    if ($("#BatchTime").val() != "") {
        $('#BatchName option[value="' + $("#BatchTime").val() + '"]').attr("selected", "selected");
    }

    if ($("#BatchID").val() > 0) {
        SpliteData();
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

function LoadCategory(branchID) {
    var postCall = $.post(commonData.Category + "CategoryData", { "branchID": branchID });
    postCall.done(function (data) {
        $('#CategoryName').empty();
        $('#CategoryName').select2();
        $("#CategoryName").append("<option value=" + 0 + ">---Select Category---</option>");
        for (i = 0; i < data.length; i++) {
            $("#CategoryName").append("<option value=" + data[i].CategoryID + ">" + data[i].Category + "</option>");
        }

        if ($("#CategoryInfo_CategoryID").val() != "") {

            $('#CategoryName option[value="' + $("#CategoryInfo_CategoryID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});

$("#CategoryName").change(function () {
    var Data = $("#CategoryName option:selected").val();
    $('#CategoryInfo_CategoryID').val(Data);
});

function SaveLibraryImage() {
    var Return;
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#flibimage');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item.length > 0) {
            if (item[0].files.length > 0) {
                formData.append('ImageFile', $('input[type=file]')[0].files[0]);
            }
        }
        
        AjaxCallWithFileUpload(commonData.NewLibrary + 'SaveLibrary', formData, function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Library Image added Successfully.", "Success");
                window.location.href = "LibraryMaintenance?LibraryID=0&Type=2";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function SaveLibraryvideo() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();      
        var postCall = $.post(commonData.NewLibrary + 'SaveLibrary', $('#flibvideo').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Library Video added Successfully.", "Success");
                window.location.href = "LibraryMaintenance?LibraryID=0&Type=1";
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveLibraryImage(LibraryID) {
    if (confirm('Are you sure want to delete this Image?')) {
        ShowLoader();
        var postCall = $.post(commonData.NewLibrary + "RemoveLibrary", { "LibraryID": LibraryID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Library Image Removed Successfully.", "Success");
            window.location.href = "LibraryMaintenance?LibraryID=0&Type=2";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveLibraryVideo(LibraryID) {
    if (confirm('Are you sure want to delete this Video?')) {
        ShowLoader();
        var postCall = $.post(commonData.NewLibrary + "RemoveLibrary", { "LibraryID": LibraryID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Library video Removed Successfully.", "Success");
            window.location.href = "LibraryMaintenance?LibraryID=0&Type=1";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}


