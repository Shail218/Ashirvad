/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    if ($("#AboutUsID").val() > 0 && $("#DetailID").val() > 0) {
        $("#fuHeaderImage").addClass("editForm");
        $("#fuHeaderImageDetail").addClass("editForm");
    }

    $("#studenttbl tr").each(function () {
        var elemImg = $(this).find("#headerImg");
        var aboutID = $(this).find("#item_AboutUsID").val();
        if (elemImg.length > 0 && aboutID.length > 0) {
            var postCall = $.post(commonData.AboutUs + "GetHeaderImage", { "aboutID": aboutID });
            postCall.done(function (data) {
                $(elemImg).attr('src', data);
            }).fail(function () {
                $(elemImg).attr('src', "../ThemeData/images/Default.png");
            });
        }
    });
});

function SaveAboutUs() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fAboutUsDetails');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }       
        AjaxCallWithFileUpload(commonData.AboutUs + 'SaveAboutus', formData, function (data) {
            if (data.AboutUsID > 0) {
                SaveDetail(data.AboutUsID);
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

function SaveDetail(aboutID) {
    var isSuccess = ValidateData('dDetailsInformation');
    if (isSuccess) {
        var frm = $('#fDetails');
        AboutUsID: $("#AboutUsInfo_AboutUsID").val(aboutID);
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.AboutUs + 'SaveDetails', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage("About Us Deatils added Successfully.", "Success");
                window.location.href = "AboutUsMaintenance?aboutID=0";
            } else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });

    }
}

function RemoveaboutUs(aboutID) {
    ShowLoader();
    var postCall = $.post(commonData.AboutUs + "RemoveAboutUs", { "aboutID": aboutID });
    postCall.done(function (data) {
        HideLoader();
        ShowMessage("About Us Removed Successfully.", "Success");
        window.location.href = "BannerMaintenance?bannerID=0";
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}



