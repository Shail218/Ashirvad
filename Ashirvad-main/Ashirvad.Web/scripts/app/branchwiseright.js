/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    
    //if ($("#PackageID").val() > 0) {
    //}

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            LoadPackage($("#Branch_BranchID").val());
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadPackage(commonData.BranchID);
        }
    });

   
    
    //if ($("#Branch_BranchID").val() != "") {
    //    $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
    //    LoadPackage($("#Branch_BranchID").val());
    //}

    if ($("#BatchTypeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTypeID").val() + '"]').attr("selected", "selected");
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

        //$.each(data, function (i) {
        //    $("#BranchName").append($("<option></option>").val(data[i].BranchID).html(data[i].BranchName));
        //});

        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadPackage(branchID) {
    var postCall = $.post(commonData.Package + "PackageDataByBranch", { "branchID": branchID });
    postCall.done(function (data) {

        $('#PackageName').empty();
        $('#PackageName').select2();
        $("#PackageName").append("<option value=" + 0 + ">---Select Package Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#PackageName").append("<option value=" + data[i].PackageID + ">" + data[i].Package + "</option>");
        }
        if ($("#Package_PackageID").val() != "") {
            $('#PackageName option[value="' + $("#Package_PackageID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {

    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadPackage(Data);
});

//$("#PackageName").change(function () {
//    var Data = $("#PackageName option:selected").val();
//    $('#Package_PackageID').val(Data);
//});

$("#PackageName").change(function () {
    debugger;
    var Data = $("#PackageName option:selected").val();
    $('#Package_PackageID').val(Data);

    var Data = $("#PackageName option:selected").val();
    if (Data > 0) {
        ShowLoader();
        var postCall = $.post(commonData.BranchWiseRight + "BranchRightUniqueData", { "PackageRightID": Data });
        postCall.done(function (data) {
            $("#rightsdiv").html(data);
            checkstatus();
            HideLoader();
        }).fail(function () {
            HideLoader();
        });
    }
    else {
        $("#UserDetails").html("");

    }
}); 

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