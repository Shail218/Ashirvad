$(document).ready(function () {

    HideLoader();

    $(".date").blur(function () {
        if ($(this).val() != '' && !isDate($(this).val())) {
            alert('Invalid date entered. Please enter valid date(dd/mm/yyyy).');
            $(this).val('');
            $(this).focus();
        }
    });
    ApplyNumeric();
    //$(".select2").select2();
    CheckRightMaster();
});

function ShowLoader() {
    document.getElementById("loader").style.display = "Block";

}
function HideLoader() {
    document.getElementById("loader").style.display = "None";

}
function CheckRightMaster() {
    var type = $("#userType").val();
    if (type == "Admin") {
        CheckRights();
    } else {
        CheckRight();
    }

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
function setCurrentDate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = (day) + "/" + (month) + "/" + now.getFullYear();
    return today;
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
            ShowMessage('Please Enter ' + $(this).attr('alt'), "Error");
            //alert();
            $(this).focus();
            isSuccess = false;
            return false;
        }
    });

    if (isSuccess) {
        $('#' + divName + ' .requiredDDL').each(function () {
            if ($(this).hasClass('editForm')) {
                return true;
            }
            var ddlVal = $("#" + $(this).attr('id') + " :selected").val();
            if (ddlVal == '' || ddlVal == '0') {
                ShowMessage('Please select ' + $(this).attr('alt'), "Error");
                //alert();
                $(this).focus();
                isSuccess = false;
                return false;
            }
        });
    }

    if (isSuccess) {
        $('#' + divName + ' .timeformat').each(function () {
            var test = $(this).val();
            if ($(this).val() == '') {
                ShowMessage('Please Enter ' + $(this).attr('alt'), "Error");
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
        $('#' + divName + ' .mobile').each(function () {
            if ($(this).val() != '') {
                var mobileReg = /^[0-9]+$/;
                if (!mobileReg.test($(this).val())) {
                    ShowMessage('Enter Only Number.', "Error");
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
                ShowMessage('Please select file in ' + $(this).attr('alt'), "Error");
                //alert();
                $(this).focus();
                isSuccess = false;
                return false;
            }
        });
    }

    if (isSuccess) {
        $('#' + divName + ' .checkboxRequired').each(function () {
            if ($(this)[0].checked == false) {
                ShowMessage('Please select ' + $(this).attr('alt'), "Error");
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
function addloader() {
    document.getElementById("mainbody").style.opacity = "0.8";
    var loadingGIF = ` <div id="loader1" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #ffffff; opacity: 0.8;">
                            <img id="imgUpdateProgress" src="../ThemeData/images/preview.gif"
                                 AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 50%; left: 50%; Width: 200px; Height: 160px;" />
                            <span style="border-width: 0px; position: fixed; padding: 30px; background-color: #FFFFFF; color: #0472C4; font-size: 25px; left: 50%; top: 35%;">Please Wait ...</span>
                        </div>`;
    $("#loader").after(loadingGIF);
}

function DeleteLoader() {
    document.getElementById("mainbody").style.opacity = "1";
    $("#loader1").remove();
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

function ConvertData(FromDate, Type) {
    var Data = FromDate.split("/");
    if (Data.length == 1 && FromDate != "") {
        Data = FromDate.split("-");
        var Date = Data[0];
        var Month = Data[1];
        var Year = Data[2];
        if (Type == 'Local') {
            var FromDateupdate = Year + "-" + Month + "-" + Date;
            return FromDateupdate
        }
        else {
            var FromDateupdate = Year + "-" + Month + "-" + Date;
            return FromDateupdate
        }
    } else {
        var Date = Data[0];
        var Month = Data[1];
        var Year = Data[2];
        if (Type == 'Local') {
            var FromDateupdate = Year + "/" + Month + "/" + Date;
            return FromDateupdate
        }
        else {
            var FromDateupdate = Year + "-" + Month + "-" + Date;
            return FromDateupdate
        }
    }

}


function arrayBufferToBase64(buffer) {
    let binary = '';
    let bytes = new Uint8Array(buffer);
    let len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}


function ConvertDateFrom(date) {
    if (date != null) {


        var sd = date.split("/Date(");
        var sd2 = sd[1].split(")/");
        var date1 = new Date(parseInt(sd2[0]));

        return date1;
    }
    return "";;
}

function convertddmmyyyy(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/')
}


function CheckRights() {
    var rightslist = $("#rightslist").val();
    if (rightslist != null && rightslist != "") {
        var data = $.parseJSON(rightslist);
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {

                var Page = data[i].PageInfo.Page;
                var Createstatus = data[i].PackageRightinfo.Createstatus;
                var Deletestatus = data[i].PackageRightinfo.Deletestatus;
                var Viewstatus = data[i].PackageRightinfo.Viewstatus;
                var ClassName = '.' + Page.replace(/\s/g, '');
                var CreateClass = '.' + Page.replace(/\s/g, '') + 'Create';
                var DeleteClass = '.' + Page.replace(/\s/g, '') + 'Delete';
                var Count = 0;
                var classname = "";
                var Ts = $('#main-menu ' + ClassName);
                $('#main-menu ' + ClassName).each(function () {
                    if (!Viewstatus) {
                        //$(this).addClass("displayNone");
                        $(this).remove();
                    }
                    else {
                        var test = $(this);
                    }
                });
                $('#main-menu ' + CreateClass).each(function () {
                    if (!Createstatus) {
                        //$(this).addClass("displayNone");
                        $(this).html("");
                    }
                    else {
                        var test = $(this);
                    }
                });
                $('#main-menu ' + DeleteClass).each(function () {
                    if (!Deletestatus) {
                        //$(this).addClass("displayNone");
                        $(this).html("");
                    }
                    else {
                        var test = $(this);
                    }
                });
            }
            var classarray = [];
            classarray.push("MasterMenu");
            classarray.push("StudentMenu");
            classarray.push("AttendanceMenu");
            classarray.push("TestMenu");
            classarray.push("GalleryMenu");
            classarray.push("LibraryMenu");
            classarray.push("UserPermissionMenu");
            classarray.push("ChartReport");
            classarray.push("MoreMenu");
            for (var i = 0; i < classarray.length; i++) {
                var Status = false;
                var Count = 0;
                var T = $('#' + classarray[i] + ' ul li');
                for (var j = 0; j < T.length; j++) {
                    var OP = T[j];
                    var Check = OP.classList.contains('displayNone');
                    if (!Check) {
                        Count++;
                        Status = true
                        break;

                    }
                }

                if (!Status) {
                    $('#' + classarray[i]).addClass('displayNone');
                }
            }
        }
    }
   

}

function CheckRight() {
    var rightslist = $("#rightlist").val();
    if (rightslist != null && rightslist != "") {
        var data = $.parseJSON(rightslist);
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {

                var Page = data[i].PageInfo.Page;
                var Createstatus = data[i].RoleRightinfo.Createstatus;
                var Deletestatus = data[i].RoleRightinfo.Deletestatus;
                var Viewstatus = data[i].RoleRightinfo.Viewstatus;
                var ClassName = '.' + Page.replace(/\s/g, '');
                var CreateClass = '.' + Page.replace(/\s/g, '') + 'Create';
                var DeleteClass = '.' + Page.replace(/\s/g, '') + 'Delete';
                var Count = 0;
                var classname = "";
                var Ts = $('#main-menu ' + ClassName);
                $('#main-menu ' + ClassName).each(function () {
                    if (!Viewstatus) {
                        //$(this).addClass("displayNone");
                        $(this).remove();
                    }
                    else {
                        var test = $(this);
                    }
                });
                $('#main-menu ' + CreateClass).each(function () {
                    if (!Createstatus) {
                        //$(this).addClass("displayNone");
                        $(this).html("");
                    }
                    else {
                        var test = $(this);
                    }
                });
                $('#main-menu ' + DeleteClass).each(function () {
                    if (!Deletestatus) {
                        //$(this).addClass("displayNone");
                        $(this).html("");
                    }
                    else {
                        var test = $(this);
                    }
                });
            }
            var classarray = [];
            classarray.push("MasterMenu");
            classarray.push("StudentMenu");
            classarray.push("AttendanceMenu");
            classarray.push("TestMenu");
            classarray.push("GalleryMenu");
            classarray.push("LibraryMenu");
            classarray.push("UserPermissionMenu");
            classarray.push("MoreMenu");
            classarray.push("ChartReport");
            for (var i = 0; i < classarray.length; i++) {
                var Status = false;
                var Count = 0;
                var T = $('#' + classarray[i] + ' ul li');
                for (var j = 0; j < T.length; j++) {
                    var OP = T[j];
                    var Check = OP.classList.contains('displayNone');
                    if (!Check) {
                        Count++;
                        Status = true
                        break;

                    }
                }

                if (!Status) {
                    $('#' + classarray[i]).addClass('displayNone');
                }
            }
        }
    }
   

}


function DateConvert(FromDate, Text) {
    var Type = "Live";
    var str1 = GetSiteURL();
    var str2 = "localhost";
    var FromDateupdate;
    if (str1.indexOf(str2) != -1) {
        Type = "Local"
    }
    var Data = FromDate.value.split("/");
    if (Data.length == 1 && FromDate.value != "") {
        Data = FromDate.value.split("-");
        var Date = Data[0];
        var Month = Data[1];
        var Year = Data[2];
        if (Type == 'Local') {
            FromDateupdate = Date + "-" + Month + "-" + Year;

        }
        else {
            FromDateupdate = Year + "-" + Month + "-" + Date;

        }
    } else {
        var Date = Data[0];
        var Month = Data[1];
        var Year = Data[2];
        if (Type == 'Local') {
            FromDateupdate = Date + "/" + Month + "/" + Year;

        }
        else {
            FromDateupdate = Year + "/" + Month + "/" + Date;

        }
    }

    $("#" + Text).val(FromDateupdate);
}
function GetSiteURL() {

    var str = window.location.href;
    var res = str.split("/");
    var URL = '';
    if (res[2].toLowerCase() === 'localhost'.toLowerCase() || res[2].toLowerCase() === '127.0.0.1'.toLowerCase()) {
        URL = window.location.protocol + "//" + res[2].toLowerCase() + "/" + res[3].toLowerCase();
    }
    else {
        URL = window.location.protocol + "//" + res[2].toLowerCase() + "/";
    }
    SiteURL = URL;
    return URL;
}

function CheckMobileno(mobile) {

    if (mobile.value.length != 10) {

        var Alt = mobile.alt;
        ShowMessage('Please Enter Valid ' + Alt + '!', 'Error');
        mobile.value = "";
    }

}

function GetUserRights(PageName) {
    var rightslist = $("#rightslist").val();
    var permission = [];
    var data = $.parseJSON(rightslist);
    if (data.length > 0) {
      
        var Index = data.findIndex(function (entry, i) {
            if (entry.PageInfo.Page.replace(/\s/g, '') == PageName) {
                index = i;
                return i;
            }
        });

      var Delete=  data[Index].PackageRightinfo.Deletestatus;
        var Create =  data[Index].PackageRightinfo.Createstatus;
        permission.push({
            "Create": Create,
            "Delete": Delete
        })
    }
    return permission;
}

function GetUserRight(PageName) {
    var rightslist = $("#rightlist").val();
    var permission = [];
    var data = $.parseJSON(rightslist);
    if (data.length > 0) {
      
        var Index = data.findIndex(function (entry, i) {
            if (entry.PageInfo.Page.replace(/\s/g, '') == PageName) {
                index = i;
                return i;
            }
        });

        var Delete = data[Index].RoleRightinfo.Deletestatus;
        var Create = data[Index].RoleRightinfo.Createstatus;
        permission.push({
            "Create": Create,
            "Delete": Delete
        })
    }
    return permission;
}


function convertTo24Hour(time) {
    var hours = parseInt(time.substr(0, 2));
    if (time.indexOf('am') != -1 && hours == 12) {
        time = time.replace('12', '0');
    }
    if (time.indexOf('pm') != -1 && hours < 12) {
        time = time.replace(hours, (hours + 12));
    }
    return time.replace(/(am|pm)/, '');
}

