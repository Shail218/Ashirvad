﻿$(document).ready(function () {

    HideLoader();

    $(".date").blur(function () {
        if ($(this).val() != '' && !isDate($(this).val())) {
            alert('Invalid date entered. Please enter valid date(dd/mm/yyyy).');
            $(this).val('');
            $(this).focus();
        }
    });
    ApplyNumeric();
    $(".select2").select2();
});

function ShowLoader() {
    document.getElementById("loader").style.display = "Block";

}
function HideLoader() {
    document.getElementById("loader").style.display = "None";

}

function ApplyNumeric() {
    $(".numeric").numeric({ decimal: ".", negative: false, scale: 4 }).keydown(function (e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            e.preventDefault();
        } else {
            var key = e.keyCode;
            if (!((key == 110) || (key == 8) || (key == 9) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                e.preventDefault();
            }
        }
    });
}

function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function ApplyNumeric() {
    $('.numeric').keydown(function (e) {
        if (e.shiftKey || e.ctrlKey || e.altKey) {
            e.preventDefault();
        } else {
            var key = e.keyCode;


            if (!((key == 8) || (key == 9) || (key == 46) || (key == 110) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                e.preventDefault();
            }

            if ($(this).val().indexOf('.') !== -1 && key == 110) {
                e.preventDefault();
            }

        }
    });

}

function ValidateData(divName) {
    var isSuccess = true;
    $('#' + divName + ' .required').each(function () {
        var test = $(this).val();
        if ($(this).val() == '') {
            ShowMessage('Please select ' + $(this).attr('alt'), "Error");
            //alert();
            $(this).focus();
            isSuccess = false;
            return false;
        }
    });

    if (isSuccess) {
        $('#' + divName + ' .requiredDDL').each(function () {
            var ddlVal = $("#" + $(this).attr('id') + " :selected").val();
            if (ddlVal == '' || ddlVal == '0') {
                ShowMessage('Please select ' + $(this).attr('alt'),"Error");
                //alert();
                $(this).focus();
                isSuccess = false;
                return false;
            }
        });
    }

    if (isSuccess) {
        $('#' + divName + ' .email').each(function () {
            if ($(this).val() != '') {
                var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
                if (!emailReg.test($(this).val())) {
                    ShowMessage('Invalid Email ID entered.', "Error");
                    $(this).focus();
                    isSuccess = false;
                    return false;
                }
            }
        });
    }

    if (isSuccess) {
        $('#' + divName + ' .fileRequired').each(function () {
            if ($(this).hasClass('editForm')) {
                return true;
            }

            if ($(this).get(0).files.length === 0) {
                ShowMessage('Please select file in ' + $(this).attr('alt'),"Error");
                //alert();
                $(this).focus();
                isSuccess = false;
                return false;
            }
        });
    }

    return isSuccess;
}

function ParseNull(itemObject) {
    return (itemObject == '' || itemObject == undefined || itemObject == null || itemObject == "") ? null : itemObject;
}

function AjaxCall(serviceUrl, dataParam, successCallBack, errorCallBack) {
    var baseURI = $("input[id$=hdnServiceURL]").val();
    $.ajax({
        url: serviceUrl,
        data: dataParam,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: successCallBack,
        error: errorCallBack,
        crossDomain: true,
        async: false
    });
}

function AjaxCallWithFileUpload(serviceUrl, dataParam, successCallBack, errorCallBack) {
    $.ajax({
      
        type: 'POST',
        url: serviceUrl,
        data: dataParam,
        cache: false,
        contentType: false,
        processData: false,
        crossDomain: true,
        success: successCallBack,
        error: errorCallBack,
        async: false
    });
}

function ConvertJsonDateString(jsonDate) {
    var shortDate = null;
    if (jsonDate) {
        var regex = /-?\d+/;
        var matches = regex.exec(jsonDate);
        var dt = new Date(parseInt(matches[0]));
        var month = dt.getMonth() + 1;
        var monthString = month > 9 ? month : '0' + month;
        var day = dt.getDate();
        var dayString = day > 9 ? day : '0' + day;
        var year = dt.getFullYear();
        shortDate = dayString + '/' + monthString + '/' + year;
    }
    return shortDate;
}

function getURLVar(key) {
    var value = [];

    var query = String(document.location).split('?');

    if (query[1]) {
        var part = query[1].split('&');

        for (i = 0; i < part.length; i++) {
            var data = part[i].split('=');

            if (data[0] && data[1]) {
                value[data[0]] = data[1];
            }
        }

        if (value[key]) {
            return value[key];
        } else {
            return '';
        }
    }
}
function readURL(input, img) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#' + img).attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
}

function ShowMessage(message, messagetype, onClose) {
    var cssclass;
    $("#alert_container").show();
    switch (messagetype) {
        case 'Success':
            cssclass = 'alert-success'
            break;
        case 'Error':
            cssclass = 'alert-danger'
            break;
        case 'Warning':
            cssclass = 'alert-warning'
            break;
        default:
            cssclass = 'alert-info'
    }
    $('#alert_container').html('');
    $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
    setTimeout(function () { $("#alert_container").hide(); }, 5000);
}

function ConvertData(FromDate) {
    var Data = FromDate.split("/");
    var Date = Data[0];
    var Month = Data[1];
    var Year = Data[2];
    var FromDateupdate = Month + "/" + Date + "/" + Year;
    return FromDateupdate
}
