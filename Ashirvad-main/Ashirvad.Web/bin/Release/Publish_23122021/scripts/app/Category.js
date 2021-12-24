/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
   
   
});

function LoadBranch(onLoaded) {
    ShowLoader();
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value='" + data[i].BranchID + "'>" + data[i].BranchName + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function Savecategory() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();

        var postCall = $.post(commonData.Category + "SaveCategory", $("#fCategory").serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "CategoryMaintenance?CategoryID=0" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }        
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveCategory(categoryID) {
    if (confirm('Are you sure want to delete this Category?')) {
        ShowLoader();
        var postCall = $.post(commonData.Category + "RemoveCategory", { "categoryID": categoryID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Category Removed Successfully.", "Success");
            window.location.href = "CategoryMaintenance?categoryID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}



$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
});

