/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />



$(document).ready(function () {
    ShowLoader();
    var table = $('#studenttbl').DataTable({
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
            url: "" + GetSiteURL() + "/Role/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "RoleName" },
            { "data": "RoleID" },
            { "data": "RoleID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data =
                            '<a style="text-align:center !important;" href="RoleMaintenance?branchID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
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
                            '<a href="#" style="text-align:center !important;" onclick="RemoveRole(' + data + ');"><img src= "../ThemeData/images/delete.png"/></a>'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        createdRow: function (tr) {
            $(tr.children[1]).addClass('textalign');
            $(tr.children[2]).addClass('textalign');
        },
    });

    //if ($("#BranchInfo_BranchID").val() != "") {
    //    $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
    //}
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

        //        //$.each(data, function (i) {
        //        //    $("#BranchName").append($("<option></option>").val(data[i].BranchID).html(data[i].BranchName));
        //        //});

        if (onLoaded != undefined) {
            onLoaded();
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveRole() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Role + "SaveRole", $('#fRoleDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "RoleMaintenance?branchID=0" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function (xhr) {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveRole(RoleID) {
    if (confirm('Are you sure want to delete this Role?')) {
        ShowLoader();
        var postCall = $.post(commonData.Role + "RemoveRole", { "RoleID": RoleID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                window.location.href = "RoleMaintenance?branchID=0";
            } else {
                ShowMessage(data.Message, "Error");
            }
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