/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();

    var table = $('#notificationtable').DataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "language": {
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Notification/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "data": HideLoader(),
        "columns": [
            { "data": "Branch.BranchName" },
            { "data": "Notification_Date" },
            { "data": "NotificationMessage" },
            { "data": "NotificationTypeText" },
            { "data": "NotificationID" },
            { "data": "NotificationID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = ConvertMiliDateFrom(data)
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a href="NotificationMaintenance?notificationID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 5,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a onclick = "RemoveNotification(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },                
                orderable: false,
                searchable: false                
            }            
        ]
    });

    $("#notification").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
    });

    if ($("#Branch_BranchID").val() != "") {
        if ($("#Branch_BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchType").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchType").val(1);
        }
    } else {
        $("#Branch_BranchID").val(0);
    }

    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }

});

function SaveNotification() {    
    var isSuccess = ValidateData('dInformation');
    var NotificationTypeList = [];
    if ($('input[type=checkbox][id=rowStaAdmin]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Admin",
            TypeID: 1
        });
    }
    if ($('input[type=checkbox][id=rowStaTeacher]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Teacher",
            TypeID: 2
        });
    }
    if ($('input[type=checkbox][id=rowStaStudent]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Student",
            TypeID: 3
        });
    }
    $('#JSONData').val(JSON.stringify(NotificationTypeList));
    var date2 = $("#Notification_Date").val();
    $("#Notification_Date").val(ConvertData(date2));
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Notification + "SaveNotification", $('#fNotificationDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage('Notification details saved!', 'Success');
            window.location.href = "NotificationMaintenance?notificationID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function RemoveNotification(branchID) {
    if (confirm('Are you sure want to delete this Notification?')) {
        ShowLoader();
        var postCall = $.post(commonData.Notification + "RemoveNotification", { "notificationID": branchID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Notification Removed Successfully.", "Success");
            window.location.href = "NotificationMaintenance?notificationID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$('input[type=radio][name=Type]').change(function () {
    if (this.value == 'All') {
        $("#BranchType").val(0);
    }
    else {
        $("#BranchType").val(1);
    }
});

function ConvertMiliDateFrom(date) {
    if (date != null) {
        var sd = date.split("/Date(");
        var sd2 = sd[1].split(")/");
        var date1 = new Date(parseInt(sd2[0]));
        var d = date1.getDate();
        var m = date1.getMonth() + 1;
        var y = date1.getFullYear();
        var hr = date1.getHours();
        var min = date1.getMinutes();
        var sec = date1.getSeconds();

        if (parseInt(d) < 10) {
            d = "0" + d;
        }
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        var Final = d + "-" + m + "-" + y + " ";
        var d = date1.toString("dd/MM/yyyy HH:mm:SS");
        return Final;
    }
    return "";;
}
