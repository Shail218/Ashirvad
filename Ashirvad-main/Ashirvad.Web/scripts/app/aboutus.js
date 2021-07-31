/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

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
        formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        AjaxCallWithFileUpload(commonData.AboutUs + 'SaveAboutus', formData, function (data) {
            HideLoader();
            if (data) {
               // ShowMessage("About Us Details saved!!!", "Success");
                //window.location.href = "AboutUsMaintenance?aboutID=0";
                var postCall = $.post(commonData.AboutUs + "DetailMaintenance", { "aboutID": 0 });
                postCall.done(function (data) {
                    $("#deatilInfo").html(data);
                    ShowMessage("About Us Details saved!!!", "Success");
                    var postCall1 = $.post(commonData.AboutUs + "ManageMaintenance", { "aboutID": 0 });
                    postCall1.done(function (data) {
                        $("#about_table").html(data);
                    }).fail(function () {
                        HideLoader();
                        ShowMessage("An unexpected error occcurred while processing request!", "Error");
                    });
                }).fail(function () {
                    HideLoader();
                    ShowMessage("An unexpected error occcurred while processing request!", "Error");
                });
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}


