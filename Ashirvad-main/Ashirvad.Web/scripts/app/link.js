﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    var check = GetUserRights('OnlineClass');
    var table = $('#livevideotable').DataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            url: "" + GetSiteURL() + "/LiveVideo/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "Title" },
            { "data": "LinkDesc" },
            { "data": "StandardName" },
            { "data": "LinkURL" },
            { "data": "UniqueID" },
            { "data": "UniqueID" }
        ],
        "columnDefs": [
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        if (check[0].Create) {
                            data =
                                '<a href="LiveVideoMaintenance?linkID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                       
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 5,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        if (check[0].Delete) {
                            data =
                                '<a onclick = "RemoveLink(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                      
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });
    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            LoadStandard($("#Branch_BranchID").val());
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadStandard(commonData.BranchID);
            HideLoader();
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#Branch_BranchID").val());
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
        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStandard(branchID) {
    
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if ($("#StandardID").val() != "") {
            $('#StandardName option[value="' + $("#StandardID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveLink() {  
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.LiveVideo + "SaveLink", $('#flinkDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("School added Successfully.", "Success");
            window.location.href = "LiveVideoMaintenance?linkID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveLink(schoolID) {
    if (confirm('Are you sure want to delete this Live Video?')) {
        ShowLoader();
        var postCall = $.post(commonData.LiveVideo + "RemoveLink", { "linkID": schoolID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Live Video Removed Successfully.", "Success");
            window.location.href = "LiveVideoMaintenance?linkID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    
    var Data = $("#StandardName option:selected").val();
    $('#StandardID').val(Data);
});