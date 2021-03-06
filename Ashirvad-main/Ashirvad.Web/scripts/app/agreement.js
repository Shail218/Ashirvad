/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();

    if ($("#AgreementID").val() == 0) {
        $("#RowStatusData_RowStatusId").val(1);
    }

    var table = $('#agreementtable').DataTable({
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
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Agreement/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "BranchData.BranchName" },
            { "data": "SerialKey" },
            { "data": "AgreementFromDate" },
            { "data": "AgreementToDate" },
            { "data": "Amount" },
            { "data": "RowStatusData.RowStatusText" },
            { "data": "AgreementID" },
        ],
        "columnDefs": [
            {
                targets: 2,
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
                targets: 3,
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
                targets: 6,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a style="text-align:center !important;" href="AgreementMaintenance?agreeID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });

    $("#datepickerfromdate").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        defaultDate: new Date(),
    });
  
    $("#datepickertodate").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        defaultDate:new Date(),
    });

    LoadBranch(function () {
        if ($("#BranchData_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchData_BranchID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#BranchData_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchData_BranchID").val() + '"]').attr("selected", "selected");
    } else {
        $("#AgreementFromDate").val(setCurrentDate());
        $("#AgreementToDate").val(setCurrentDate());
    }

    if ($("#RowStatusData_RowStatusId").val() != "") {

        var rowStatus = $("#RowStatusData_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
            $("#RowStatusData_RowStatusId").val(1);
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
            $("#RowStatusData_RowStatusId").val(2);
        }
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
    return "";
}

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveAgreement() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#AgreementFromDate").val();
        $("#AgreementFromDate").val(ConvertData(date1));
        var date2 = $("#AgreementToDate").val();
        $("#AgreementToDate").val(ConvertData(date2));
        var postCall = $.post(commonData.Agreement + "SaveAgreement", $('#fAgreementDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "AgreementMaintenance?agreeID=0"; }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchData_BranchID').val(Data);
});

$('input[type=radio][name=Status]').change(function () {
    if (this.value == 'Active') {
        $("#RowStatusData_RowStatusId").val(1);
    }
    else {
        $("#RowStatusData_RowStatusId").val(2);
    }
});