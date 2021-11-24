/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    if ($("#BannerID").val() > 0) {
        $("#fuBannerImage").addClass("editForm");
    }

    if ($("#BranchInfo_BranchID").val() != "") {
        if ($("#BranchInfo_BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchInfo_BranchID").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchInfo_BranchID").val(1);
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
                ID: 0,
                TypeText: "Admin",
                TypeID: 1
            });
        }
        if ($('input[type=checkbox][id=rowStaTeacher]').is(":checked")) {
            NotificationTypeList.push({
                ID: 0,
                TypeText: "Teacher",
                TypeID: 2
            });
        }
        if ($('input[type=checkbox][id=rowStaStudent]').is(":checked")) {
            NotificationTypeList.push({
                ID: 0,
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
        $("#BranchInfo_BranchID").val(0);
    }
    else {
        $("#BranchInfo_BranchID").val(1);
    }
});
