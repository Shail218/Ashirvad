/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    $("#datepicker").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    $("#datepickerappointment").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    $("#datepickerjoining").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    $("#datepickerleaving").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    var table = $('#usertable').DataTable({
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
            url: "" + GetSiteURL() + "/User/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "Name" },
            { "data": "MobileNo" },
            { "data": "EmailID" },
            { "data": "GenderText" },
            { "data": "StaffID"},
            { "data": "StaffID"}
        ],
        "columnDefs": [
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a href="UserMaintenance?branchID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
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
                            '<a onclick = "RemoveUser(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
    }

    if ($("#Gender").val() != "") {
        $('#GenderName').find('option:contains("' + $("#Gender").val() + '")').attr('selected', 'selected');
    }
    if ($("#Userrole").val() != "") {
        $('#Role').find('option:contains("' + $("#Userrole").val() + '")').attr('selected', 'selected');
    }
});

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

function SaveUser() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#DOB").val();
        $("#DOB").val(ConvertData(date1));
        var date2 = $("#ApptDT").val();
        $("#ApptDT").val(ConvertData(date2));
        var date3 = $("#JoinDT").val();
        $("#JoinDT").val(ConvertData(date3));
        var date4 = $("#LeavingDT").val();
        $("#LeavingDT").val(ConvertData(date4));
        var postCall = $.post(commonData.User + "SaveUser", $('#fUserDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "UserMaintenance?branchID=0"; }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }   
        }).fail(function () {
            HideLoader();
            ShowMessage(data.Message , "Error");
        });
    }
}

function RemoveUser(userID) {
    if (confirm('Are you sure want to delete this User?')) {
        ShowLoader();
        var postCall = $.post(commonData.User + "RemoveUser", { "userID": userID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("User Removed Successfully.", "Success");
            window.location.href = "UserMaintenance?branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#GenderName").change(function () {
    var Data = $("#GenderName option:selected").val();
    $('#Gender').val(Data);
});

$("#Role").change(function () {
    var Data = $("#Role option:selected").val();
    $('#Userrole').val(Data);
});

$("#BranchName").change(function () {    
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});