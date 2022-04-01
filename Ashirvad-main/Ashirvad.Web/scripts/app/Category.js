/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();

    var table = $('#categorytable').DataTable({
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
            url: "" + GetSiteURL() + "/Category/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "Category" },
            { "data": "CategoryID" },
            { "data": "CategoryID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data = '<a style="text-align:center !important;" href="CategoryMaintenance?categoryID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 2,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a style="text-align:center !important;" onclick = "RemoveCategory(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('image - cls');
        },
    });
   
});

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
            if (data.Success) {
                ShowMessage(data.Message, "Success");
                window.location.href = "CategoryMaintenance?categoryID=0";
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}
