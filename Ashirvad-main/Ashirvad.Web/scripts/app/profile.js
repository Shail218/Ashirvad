/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function SaveUser() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Profile + "SaveUser", $('#fUserDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                if (data.IsEdit == false)
                    setTimeout(function () { window.location.href = "Profile"; }, 2000);
                else
                    setTimeout(function () { window.location.href = "Login"; }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

