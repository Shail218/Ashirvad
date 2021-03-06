/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {

    LoadFinancialYear();
});
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
                setTimeout(function () { window.location.href = commonData.VDName + 'Login/Index'; }, 5000);
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
    var isValidate = false;
    var confirmPassword = $("#confirm_password").val();
    var newPassword = $("#new_password").val();
    var oldpass = $("#old_password").val();
    if (confirmPassword === newPassword) {
       
        if (confirmPassword === oldpass) {
            ShowMessage("Old Password and New Password cannot be same.", "Error");
        } else {
            isValidate = true;
        }
    } else {
        ShowMessage("Password and Confirm Password must be same.", "Error");
    }
    if (isSuccess && isValidate) {
        ShowLoader();
        var password = $("#new_password").val();
        var oldPassword = $("#old_password").val();
        var postCall = $.post(commonData.ChangePassword + "changepassword", { "password": password, "oldPassword": oldPassword });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                window.location.href;
            } else {
                ShowMessage(data.Message, "Error");
            }
           
            }).fail(function () {
                HideLoader();
                ShowMessage("An unexpected error occcurred while processing request!", "Error");
            });
    }
}

function LoadFinancialYear() {
        $('#financeyr').empty();
        $('#financeyr').select2();
    $("#financeyr").append("<option value=" + 0 + ">---Select Financial Year---</option>");
   
    var today = new Date();
    
    for (i = 2020; i <= today.getFullYear(); i++) {
        //if (data.length == 1) {
        //    $("#financeyr").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
        //    $('#financeyr option[value="' + data[i].course_dtl_id + '"]').attr("selected", "selected");

        //} else {
        //    $("#financeyr").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
        //}
        var fiscalyear = "";
        if (i == today.getFullYear()) {
            if ((today.getMonth() + 1) <= 3) {
                fiscalyear = (today.getFullYear() - 1) + "-" + today.getFullYear()
                //$("#financeyr").append("<option value='" + fiscalyear + "'>" + fiscalyear + "</option>");
                $("#FinancialYear").val(fiscalyear);
                $('#financeyr option[value="' + $("#FinancialYear").val() + '"]').attr("selected", "selected");
            } else {
                fiscalyear = today.getFullYear() + "-" + (today.getFullYear() + 1)
                $("#financeyr").append("<option value='" + fiscalyear + "'>" + fiscalyear + "</option>");
                $("#FinancialYear").val(fiscalyear);
                $('#financeyr option[value="' + $("#FinancialYear").val() + '"]').attr("selected", "selected");
            }
        } else {
            fiscalyear = i + "-" + (i + 1)
            $("#financeyr").append("<option value='" + fiscalyear + "'>" + fiscalyear + "</option>");
        }
    }
}

$("#financeyr").change(function () {
    var Data = $("#financeyr option:selected").val();
    $('#FinancialYear').val(Data);
});
function getCurrentFinancialYear() {
    var fiscalyear = "";
    var today = new Date();
    if ((today.getMonth() + 1) <= 3) {
        fiscalyear = (today.getFullYear() - 1) + "-" + today.getFullYear()
    } else {
        fiscalyear = today.getFullYear() + "-" + (today.getFullYear() + 1)
    }
    return fiscalyear
}