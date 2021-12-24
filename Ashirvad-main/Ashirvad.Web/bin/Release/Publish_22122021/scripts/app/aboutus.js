/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    if ($("#AboutUsID").val() > 0 ) {
        $("#fuHeaderImage").addClass("editForm");
        
    }
    if ($("#DetailID").val() > 0) {
        $("#fuHeaderImageDetail").addClass("editForm");
    }
    HideLoader();
 
    //$("#studenttbl tr").each(function () {
       
    //    var elemImg = $(this).find("#headerImg");
    //    var aboutID = $(this).find("#item_AboutUsID").val();
     
    //    if (elemImg.length > 0 && aboutID.length > 0) {
    //        var postCall = $.post(commonData.AboutUs + "GetHeaderImage", { "aboutID": aboutID });
           
    //        postCall.done(function (data) {
             
    //            $(elemImg).attr('src', data);
    //        }).fail(function () {
    //            $(elemImg).attr('src', "../ThemeData/images/Default.png");
    //        });
    //    }
    //});
});

function SaveAboutUs() {
    
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fAboutUsDetails');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }       
        AjaxCallWithFileUpload(commonData.AboutUs + 'SaveAboutus', formData, function (data) {
            HideLoader();

            if (data.AboutUsID > 0) {

                ShowMessage('About us added Successfully.', 'Success');
                window.location.href = "AboutUsMaintenance?aboutID=0&detailid=0";

            }
            else if (data.AboutUsID < 0) {

                ShowMessage('Already Exist.', 'Error');

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

function SaveDetail(aboutID) {
   
    var isSuccess = ValidateData('dDetailsInformation');
    if (isSuccess) {
        var frm = $('#fDetails');
        AboutUsID: $("#AboutUsInfo_AboutUsID").val(aboutID);
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.AboutUs + 'SaveDetails', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage("About Us Deatils added Successfully.", "Success");
                window.location.href = "AboutUsMaintenance?aboutID=0";
            } else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    } else {
        HideLoader();
    }
}

function RemoveaboutUs(aboutID) {
    
    if (confirm('Are you sure want to delete this About Us details?')) {
        ShowLoader();
        var postCall = $.post(commonData.AboutUs + "RemoveAboutUs", { "aboutID": aboutID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("About Us Removed Successfully.", "Success");
            window.location.href = "AboutUsMaintenance?aboutID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveDetails(detailID) {
    
    if (confirm('Are you sure want to delete this About Us details?')) {
        ShowLoader();
        var postCall = $.post(commonData.AboutUs + "RemoveDetails", { "detailID": detailID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("About Us Removed Successfully.", "Success");
            window.location.href = "AboutUsMaintenance?aboutID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#fuHeaderImage").change(function () {
    readURL(this);
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.readAsDataURL(input.files[0]);
        reader.onload = function (e) {

            $('#imgStud').attr('src', e.target.result);
            var bas = reader.result;
            var PANtUploadval = bas;
            var ssmdPAN = PANtUploadval.replace("data:image/*;base64,", "")
            var code = ssmdPAN.split(",");
            $('#HeaderImageText').val(code[1]);
        }
    }
}




