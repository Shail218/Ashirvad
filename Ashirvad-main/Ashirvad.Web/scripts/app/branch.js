/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    ShowLoader();
    var table = $('#branchtble').DataTable({
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
            url: "" + GetSiteURL() + "/Branch/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "BranchName" },
            {"data":"aliasName"},
            { "data": "BranchMaint.FilePath" },
            { "data": "BranchMaint.AppFilePath" },
            { "data": "RowStatus.RowStatusText" },
            { "data": "BranchID" },
        ],
        "columnDefs": [
            {
                targets: 2,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data =
                            '<img src = "' + data + '" style="height:40px;width:40px;"/>'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },{
                targets: 3,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data =
                            '<img src = "' + data + '" style="height:40px;width:40px;"/>'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 4,                
                orderable: false,                
            },
            {
                targets: 5,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a style="text-align:center !important;" href="BranchMaintenance?branchID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('textalign');
            $(tr.children[1]).addClass('image - cls');
            $(tr.children[2]).addClass('textalign');
            $(tr.children[3]).addClass('textalign');
        },        
    });

    if ($("#BranchID").val() > 0) {
        $("#fuBranchImage").addClass("editForm");
    }

    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
            $("#RowStatus_RowStatusId").val(1);
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
            $("#RowStatus_RowStatusId").val(2);
        }
    }
});

$('input[type=radio][name=Status]').change(function () {
    if (this.value == '1') {
        $("#RowStatus_RowStatusId").val(1);
    }
    else {
        $("#RowStatus_RowStatusId").val(2);
    }
});

function SaveBranch() {
    
    var isSuccess = ValidateData('dInformation');

    if (isSuccess) {
        ShowLoader();
        var frm = $('#fBranchDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
            formData.append('AppImageFile', $('input[type=file]')[1].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Branch + 'SaveBranch', formData, function (data) {
            HideLoader();
            if (data) {
                if (data.Status) {
                    ShowMessage(data.Message, 'Success');
                    window.location.href = "BranchMaintenance?branchID=0";
                }
                else {
                    ShowMessage(data.Message, 'Error');
                }
            } else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
          
        }, function (xhr) {
            HideLoader();
            ShowMessage('An unexpected error occcurred while processing request!', 'Error');
        });
    }
}

function RemoveBranch(branchID) {

    var postCall = $.post(commonData.Branch + "RemoveBranch", { "branchID": branchID });
    postCall.done(function (data) {
        if (data.Status) {
            ShowMessage(data.Message, 'Success');
            window.location.href = "BranchMaintenance?branchID=0";
        } else {
            ShowMessage(data.Message, 'Error');
        }
       
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}