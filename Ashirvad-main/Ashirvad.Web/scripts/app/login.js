/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function login() {
    var hasError = false;
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        $.post(commonData.Login + 'ValidateUser', $('#FormItem').serialize(), function (data) {
            HideLoader();
            if (data == true) {
                window.location.href = commonData.VDName + 'Home/Index';
            }
            else {
                ShowMessage('Invalid Username or Password!', 'Error', 'error');
            }
        }).fail(function (xhr) {
            HideLoader();
            alert(xhr);
        });
    }

    else {
    }
}