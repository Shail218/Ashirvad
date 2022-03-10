/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#BannerID").val() > 0) {
        $("#fuBannerImage").addClass("editForm");
    }
    ShowLoader();
    var check = GetUserRights('BannerMaster');
    var studenttbl = $("#studenttbl").DataTable({
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
            url: "" + GetSiteURL() + "/Banner/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        columns: [
            { "data": "BranchInfo.BranchName" },
            { "data": "FilePath" },
            { "data": "BannerTypeText" },
            { "data": "BannerID" },
            { "data": "BannerID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = (data == null || data == "https://mastermind.org.in") ? '<img src="../ThemeData/images/Default.png" id="branchImg" style="height:60px;width:60px;margin-left:20px;" />' : '<img src = "' + data + '" style="height:60px;width:60px;margin-left:20px;"/>'
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
                        if (check[0].Create) {
                            data =
                                '<a href="BannerMaintenance?bannerID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                    }
                    else {
                        data = "";
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (check[0].Delete) {
                        if (type === 'display') {
                            data =
                                '<a onclick = "RemoveBanner(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                        }
                    }
                    else {
                        data = "";
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('image - cls');
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        if ($("#BranchInfo_BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchType").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchType").val(1);
        }
    } else {
        $("#BranchInfo_BranchID").val(0);
    }

    $('input[type=radio][name=Status]').change(function () {
        if (this.value == 'Active') {
            $("#RowStatus_RowStatusId").val(1);
        }
        else {
            $("#RowStatus_RowStatusId").val(2);
        }
    });

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

function chkOnChange(elem, hdnID, selText) {
    if ($(this).attr('checked') == 'checked') {
        alert('yes,' + hdnID + ',' + selText);
    }
}

function SaveBanner() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        var NotificationTypeList = [];
        if ($('input[type=checkbox][id=rowStaAdmin]').is(":checked")) {
            NotificationTypeList.push({
                // ID: 0,
                TypeText: "Admin",
                TypeID: 1
            });
        }
        if ($('input[type=checkbox][id=rowStaTeacher]').is(":checked")) {
            NotificationTypeList.push({
                //  ID: 0,
                TypeText: "Teacher",
                TypeID: 2
            });
        }
        if ($('input[type=checkbox][id=rowStaStudent]').is(":checked")) {
            NotificationTypeList.push({
                //  ID: 0,
                TypeText: "Student",
                TypeID: 3
            });
        }
        var bannerData =
        {
            BannerID: $("#BannerID").val(),
            BannerType: NotificationTypeList,
            BranchInfo: {
                BranchID: $("#BranchInfo_BranchID").val()
            },
            ImageFile: $('input[type=file]')[0].files[0]
        };
        $('#JSONData').val(JSON.stringify(NotificationTypeList));
        ShowLoader();
        var frm = $('#fBannerDetail');
        var formData = new FormData(frm[0]);
        formData.append('ImageFile', bannerData.ImageFile);
        AjaxCallWithFileUpload(commonData.Banner + 'SaveBanner', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage('Banner details saved!', 'Success');
                window.location.href = "BannerMaintenance?bannerID=0";
            }
            else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemoveBanner(branchID) {
    if (confirm('Are you sure want to delete this Banner?')) {
        ShowLoader();
        var postCall = $.post(commonData.Banner + "RemoveBanner", { "bannerID": branchID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Banner Removed Successfully.", "Success");
            window.location.href = "BannerMaintenance?bannerID=0";
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
