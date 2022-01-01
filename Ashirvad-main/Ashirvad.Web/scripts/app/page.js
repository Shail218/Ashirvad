/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    var test = "" + GetSiteURL() + "/Page/CustomServerSideSearchAction";
    var table = $('#pagetble').DataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": "Processing...",
        "serverSide": true,
        "language": {
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: "" + GetSiteURL() + "/Page/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "Page" }
            //{ "data": "PageID" },
            //{ "data": "PageID" }
        ],
        //"columnDefs": [
        //    {
        //        targets: 1,
        //        render: function (data, type, full, meta) {

        //            if (type === 'display') {
        //                data =
        //                    '<a style="text-align:center !important;" href="PageMaintenance?branchID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
        //            }

        //            return data;
        //        },
        //        orderable: false,
        //        searchable: false
        //    },
        //    {
        //        targets: 2,
        //        render: function (data, type, full, meta) {

        //            if (type === 'display') {
        //                data =
        //                    '<a href="#" onclick="RemovePage(' + data + ');"><img src= "../ThemeData/images/delete.png"/></a>'


        //            }
        //            return data;
        //        },
        //        orderable: false,
        //        searchable: false
        //    }
        //],
        //createdRow: function (tr) {
        //    $(tr.children[1]).addClass('textalign');
        //    $(tr.children[2]).addClass('textalign');
        //},
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
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SavePage() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Page + "SavePage", $('#fPageDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "PageMaintenance?branchID=0" }, 20);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemovePage(pageID) {
    if (confirm('Are you sure want to delete this Page?')) {
        ShowLoader();
        var postCall = $.post(commonData.Page + "RemovePage", { "pageID": pageID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Page Removed Successfully.", "Success");
            window.location.href = "PageMaintenance?branchID=0";
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