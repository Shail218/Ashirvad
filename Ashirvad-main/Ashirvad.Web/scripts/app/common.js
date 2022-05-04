var commonData =
{
    siteURL: $("#hdnFldSiteURL").val(),
    VDName: $("#hdnFldVDName").val(),
    BranchID: $("#hdnBranchID").val(),
    Branch: $("#hdnFldVDName").val() + "Branch/",
    Login: $("#hdnFldVDName").val() + "Login/",
    School: $("#hdnFldVDName").val() + "School/",
    Batch: $("#hdnFldVDName").val() + "Batch/",
    Standard: $("#hdnFldVDName").val() + "Standard/",
    Subject: $("#hdnFldVDName").val() + "Subject/",
    User: $("#hdnFldVDName").val() + "User/",
    Student: $("#hdnFldVDName").val() + "Student/",
    ManageStudent: $("#hdnFldVDName").val() + "ManageStudent/",
    Notification: $("#hdnFldVDName").val() + "Notification/",
    Photos: $("#hdnFldVDName").val() + "Photos/",
    Videos: $("#hdnFldVDName").val() + "Videos/",
    LiveVideo: $("#hdnFldVDName").val() + "LiveVideo/",
    Youtube: $("#hdnFldVDName").val() + "Youtube/",
    Banner: $("#hdnFldVDName").val() + "Banner/",
    Paper: $("#hdnFldVDName").val() + "Papers/",
    Library: $("#hdnFldVDName").val() + "Library/",
    UserPermission: $("#hdnFldVDName").val() + "UserPermission/",
    AttendanceEntry: $("#hdnFldVDName").val() + "AttendanceEntry/",
    AttendanceRegister: $("#hdnFldVDName").val() + "AttendanceRegister/",
    TestPaper: $("#hdnFldVDName").val() + "TestPaper/",
    Homework: $("#hdnFldVDName").val() + "Homework/",
    Reminder: $("#hdnFldVDName").val() + "Reminder/",
    ToDo: $("#hdnFldVDName").val() + "ToDo/",
    ToDoRegister: $("#hdnFldVDName").val() + "ToDoRegister/",
    ChangePassword: $("#hdnFldVDName").val() + "ChangePassword/",
    AboutUs: $("#hdnFldVDName").val() + "AboutUs/",
    AdminReport: $("#hdnFldVDName").val() + "AdminReport/",
    Announcement: $("#hdnFldVDName").val() + "Anouncement/",
    UPI: $("#hdnFldVDName").val() + "Upi/",
    Agreement: $("#hdnFldVDName").val() + "Agreement/",
    AttendanceReport: $("#hdnFldVDName").val() + "AttendanceReport/",
    FeesStructure: $("#hdnFldVDName").val() + "FeesStructure/",
    ResultEntry: $("#hdnFldVDName").val() + "ResultEntry/",
    Category: $("#hdnFldVDName").val() + "Category/",
    NewLibrary: $("#hdnFldVDName").val() + "NewLibrary/",
    Page: $("#hdnFldVDName").val() + "Page/",
    Package: $("#hdnFldVDName").val() + "Package/",
    PackageRight: $("#hdnFldVDName").val() + "PackageRight/",
    BranchWiseRight: $("#hdnFldVDName").val() + "BranchWiseRight/",
    Faculty: $("#hdnFldVDName").val() + "Faculty/",
    Course: $("#hdnFldVDName").val() + "Caurse/",
    Class: $("#hdnFldVDName").val() + "Class/",
    SuperAdminSubject: $("#hdnFldVDName").val() + "SuperAdminSubject/",
    BranchCourse: $("#hdnFldVDName").val() + "BranchCourse/",
    MarksRegister: $("#hdnFldVDName").val() + "ResultRegister/",
    BranchClass: $("#hdnFldVDName").val() + "BranchClass/",
    BranchSubject: $("#hdnFldVDName").val() + "BranchSubject/",
    ManageLibrary: $("#hdnFldVDName").val() + "ManageLibrary/",
    Profile: $("#hdnFldVDName").val() + "Profile/",
    Circular: $("#hdnFldVDName").val() + "Circular/",
    Home: $("#hdnFldVDName").val() + "Home/",
    StandardWiseChart: $("#hdnFldVDName").val() + "StandardWiseChart/",
    BatchWiseChart: $("#hdnFldVDName").val() + "BatchWiseChart/",
    ListOfStudent: $("#hdnFldVDName").val() + "ListOfStudent/",
    ProgressReportChart: $("#hdnFldVDName").val() + "ProgressReportChart/",    
    AttendanceByStudent: $("#hdnFldVDName").val() + "AttendanceByStudent/" ,
    OnlinePaymentList: $("#hdnFldVDName").val() + "OnlinePaymentList/",
    Role: $("#hdnFldVDName").val() + "Role/",
    RoleRights: $("#hdnFldVDName").val() + "RoleRights/",
    UserRights: $("#hdnFldVDName").val() + "UserRights/",
    Competition: $("#hdnFldVDName").val() + "Competition/",
    CompetitionAnswerSheet: $("#hdnFldVDName").val() + "CompetitionAnsDetails/",
    CompetitionRank: $("#hdnFldVDName").val() + "CompetitionRank/",
};

function GetExtention(image){
    var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9\+\/\=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/\r\n/g, "\n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }

    // Define the string, also meaning that you need to know the file extension
    var encoded = image;

    // Decode the string
    var decoded = Base64.decode(encoded);
    console.log(decoded);

    // if the file extension is unknown
    var extension = undefined;
    // do something like this
    var lowerCase = decoded.toLowerCase();
    if (lowerCase.indexOf("png") !== -1) extension = "png"
    else if (lowerCase.indexOf("jpg") !== -1 || lowerCase.indexOf("jpeg") !== -1)
        extension = "jpg"
    else extension = "tiff";

    // and then to display the image
    var img = document.createElement("img");
    img.src = decoded;

    // alternatively, you can do this
    img.src = "data:image/" + extension + ";base64," + encoded;
}


var tableToExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
        , base64 = function (s) { debugger; return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { debugger; return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name) {
        debugger;

        var FileName = $("#" + name).text();
        var ctx = { worksheet: FileName || 'Worksheet', table: table }
        var a = document.createElement("a");
        a.href = uri + base64(format(template, ctx));
        a.download = "" + FileName + ".xls";
        document.body.appendChild(a);
        a.click();
        //window.location.href = uri + base64(format(template, ctx));
    }
})()

function Exportdata(Controller) {
    ShowLoader();
    var postCall = $.post($("#hdnFldVDName").val() + Controller+"/" + "GetExportData");
    postCall.done(function (data) {
        HideLoader();
        tableToExcel(data, 'exceltitle');
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}