/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {


    ShowLoader();
    LoadBranch();
    LoadPackage();

    var Id = $("#BranchWiseRightsID").val();
    if (Id > 0) {
        update();

    }
});

function LoadBranch() {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {

        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
        var t = $("#branchinfo_BranchID").val();
        if ($("#branchinfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#branchinfo_BranchID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadPackage() {
    var postCall = $.post(commonData.Package + "PackageDataByBranch", { "branchID": 0 });
    postCall.done(function (data) {

        $('#PackageName').empty();
        $('#PackageName').select2();
        $("#PackageName").append("<option value=" + 0 + ">---Select Package Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#PackageName").append("<option value=" + data[i].PackageID + ">" + data[i].Package + "</option>");
        }
        var t = $("#Packageinfo_PackageID").val();
        if ($("#Packageinfo_PackageID").val() != "") {
            $('#PackageName option[value="' + $("#Packageinfo_PackageID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {

    var Data = $("#BranchName option:selected").val();
    $('#branchinfo_BranchID').val(Data);
    //LoadPackage(Data);
});

//$("#PackageName").change(function () {
//    var Data = $("#PackageName option:selected").val();
//    $('#Packageinfo_PackageID').val(Data);
//});

$("#PackageName").change(function () {
    var Data = $("#PackageName option:selected").val();
    $('#Packageinfo_PackageID').val(Data);

    var Data = $("#PackageName option:selected").val();
    if (Data > 0) {
        ShowLoader();
        var postCall = $.post(commonData.BranchWiseRight + "BranchRightUniqueData", { "PackageRightID": Data });
        postCall.done(function (data) {
            HideLoader();
            $("#rightsdiv").html(data);
            var test = $('#Packageinfo_PackageID').val();
            checkstatus();

        }).fail(function () {
            HideLoader();
        });
    }
    else {
        $("#UserDetails").html("");

    }
});

function update() {
    var Data = $("#PackageName option:selected").val();
    if (Data > 0) {
        ShowLoader();
        var postCall = $.post(commonData.BranchWiseRight + "BranchRightUniqueData", { "PackageRightID": Data });
        postCall.done(function (data) {
            HideLoader();
            $("#rightsdiv").html(data);
            var test = $('#Packageinfo_PackageID').val();
            checkstatus();

        }).fail(function () {
            HideLoader();
        });
    }
    else {
        $("#UserDetails").html("");

    }
}
function checkstatus() {
    var Create = true;
    var Delete = true;
    var View = true;

    $('#choiceList .createStatus').each(function () {
        if ($(this)[0].checked == false) {
            Create = false;
        }
    });

    $('#choiceList .deletestatus').each(function () {
        if ($(this)[0].checked == false) {
            Delete = false;
        }
    });
    $('#choiceList .viewstatus').each(function () {
        if ($(this)[0].checked == false) {
            View = false;
        }
    });

    $('#choiceList thead').each(function () {

        if (Create == true) {
            $(this).find("#allcreate")[0].checked = true;
        }

        if (Delete == true) {
            $(this).find("#alldelete")[0].checked = true;
        } if (View == true) {
            $(this).find("#allview")[0].checked = true;
        }
    });

}

function OnSelectStatus(Data, classData) {
    if (Data.checked == true) {
        $('#choiceList .' + classData).each(function () {
            $(this)[0].checked = true;

        });
    }
    else {
        $('#choiceList .' + classData).each(function () {
            $(this)[0].checked = false;

        });
    }
}

function GetData() {
    var CreateArray = [];
    var DeleteArray = [];
    var ViewArray = [];
    var PageArray = [];
    var PackageIDArray = [];
    var MainArray = [];

    $('#choiceList .createStatus').each(function () {
        var Create = $(this)[0].checked;
        CreateArray.push(Create);
    });

    $('#choiceList .deletestatus').each(function () {
        var Delete = $(this)[0].checked;
        DeleteArray.push(Delete);
    });
    $('#choiceList .viewstatus').each(function () {
        var View = $(this)[0].checked;
        ViewArray.push(View);
    });

    $('#choiceList .pagename').each(function () {
        var Page = $(this).val();
        PageArray.push(Page);
    });

    $('#choiceList .PackageRightsIdlist').each(function () {
        var Package = $(this).val();
        PackageIDArray.push(Package);
    });
    for (var i = 0; i < PageArray.length; i++) {
        var Page = PageArray[i];
        var Create = CreateArray[i];
        var Delete = DeleteArray[i];
        var View = ViewArray[i];
        var Package = PackageIDArray[i];
        MainArray.push({
            "PageInfo": { "PageID": Page },
            "Createstatus": Create,
            "Deletestatus": Delete,
            "Viewstatus": View,
            "PackageRightsId": Package

        })
    }
    return MainArray;
}

function SaveBranchWiseRight() {
    debugger;
    var Array = [];
    var isSuccess = ValidateData('drights');
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JasonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.BranchWiseRight + "SaveBranchRight", $('#fPackageRightDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data == true) {
                ShowMessage("Created Successfully!!", 'Success');
                setTimeout(function () { window.location.href = "BranchRightMaintenance?BranchRightID=0"; }, 2000);

            }
            else {
                ShowMessage("Failed to Create!!", 'Error');
            }

        }).fail(function () {
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function RemoveBranchRight(BranchRightID) {
    if (confirm('Are you sure want to delete this?')) {
        ShowLoader();
        var postCall = $.post(commonData.BranchWiseRight + "RemoveBranchRight", { "BranchRightID": BranchRightID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Branch Right Removed Successfully.", "Success");
            window.location.href = "BranchRightMaintenance?BranchRightID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

