/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function login() {
    var hasError = false;
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        $.post(commonData.Login + 'ValidateUser', $('#FormItem').serialize(), function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = commonData.VDName + data.URL;}, 500);
                
            }
            else {
                ShowMessage(data.Message,'Error');
            }
        }).fail(function (xhr) {
            HideLoader();
            alert(xhr);
        });
    }

    else {
    }
}

function checkusername() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        $.post(commonData.Login + 'CheckUserName', $('#FormItem').serialize(), function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                window.location.href = commonData.VDName + 'Login/Index';
            }
            else {
                ShowMessage(data.Message, 'Error');
            }
        }).fail(function (xhr) {
            HideLoader();
            alert(xhr);
        });
    }

    else {
    }
}

function ChangePassword() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var password = $("#Password").val();
        var oldPassword = $("#old_password").val();
        var postCall = $.post(commonData.ChangePassword + "changepassword", { "password": password, "oldPassword": oldPassword });
        postCall.done(function (data) {
                HideLoader();
                ShowMessage("password Change Successfully.", "Success");
            window.location.href = commonData.VDName + 'Home/Index';
            }).fail(function () {
                HideLoader();
                ShowMessage("An unexpected error occcurred while processing request!", "Error");
            });
    }
}