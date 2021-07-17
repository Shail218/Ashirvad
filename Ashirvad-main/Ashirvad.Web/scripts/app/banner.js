/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    //$("#BranchDiv").hide();
    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            if ($("#BranchInfo_BranchID").val() != "0") {
                $("#rowStaBranch").attr('checked', 'checked');
                $("#BranchDiv").show();
                $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
            } else {
                $("#rowStaAll").attr('checked', 'checked');
                $("#BranchDiv").hide();
                $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
            }
        } else {
            $("#BranchDiv").hide();
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        if ($("#BranchInfo_BranchID").val() != "0") {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchDiv").show();
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        } else {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchDiv").hide();
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }
    } else {
        $("#BranchDiv").hide();
    }

    $('input[type=radio][name=Status]').change(function () {
        if (this.value == 'Active') {
            $("#RowStatus_RowStatusId").val(1);
        }
        else {
            $("#RowStatus_RowStatusId").val(2);
        }
    });

    $('input[type=radio][name=Type]').change(function () {
        if (this.value == 'Branch') {
            $('#BranchName option[value="0"]').attr("selected", "selected");
            $("#BranchDiv").show();
        }
        else {
            $("#BranchDiv").hide();
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

    $("#studenttbl tr").each(function () {
        var elemImg = $(this).find("#bannerImg");
        var BannerID = $(this).find("#item_BannerID").val();
        if (elemImg.length > 0 && BannerID.length > 0) {
            var postCall = $.post(commonData.Banner + "GetBannerImage", { "bannerID": BannerID });
            postCall.done(function (data) {
                $(elemImg).attr('src', data);
            }).fail(function () {
                $(elemImg).attr('src', "../ThemeData/images/Default.png");
            });
        }
    });

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

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

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

        //$("#BannerType").val(NotificationTypeList);

        var frm = $('#fBannerDetail');
        var formData = new FormData(frm[0]);
        //formData.append('ImageFile', $('input[type=file]')[0].files[0]);

        //formData.append('banner', bannerData);
        //formData.append('banner', bannerData);
        formData.append('BannerType', JSON.stringify(bannerData.BannerType));
        //formData.append('BranchInfo', bannerData.BranchInfo);
        formData.append('ImageFile', bannerData.ImageFile);
        var a = formData.getAll('BannerType');
        AjaxCallWithFileUpload(commonData.Banner + 'SaveBanner', formData, function (data) {
            
            if (data) {
                ShowMessage('Banner details saved!', 'Success');
                window.location.href = "BannerMaintenance?bannerID=0";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {

        });
    }
}

function RemoveBanner(branchID) {
    var postCall = $.post(commonData.Banner + "RemoveBanner", { "bannerID": branchID });
    postCall.done(function (data) {
        ShowMessage("Banner Removed Successfully.", "Success");
        window.location.href = "BannerMaintenance?bannerID=0";
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});
