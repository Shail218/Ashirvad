/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    if ($("#FeesID").val() > 0) {
        $("#fuFeeImage").addClass("editForm");
    }
    ShowLoader();
    var check = GetUserRights('StudentFeesStructure');
    var table = $('#feetable').DataTable({
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
            url: "" + GetSiteURL() + "/FeesStructure/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "standardInfo.Standard" },
            { "data": "FilePath" },
            { "data": "Remark" },
            { "data": "FeesID" },
            { "className": 'StudentFeesStructureDelete', "data": "FeesID" }
        ],
        "columnDefs": [
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<img src = "' + data + '" style="height:60px;width:60px;margin-left:20px;"/>'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 3,
                render: function (data, type, full, meta) {
                    if (check[0].Create) {
                        if (type === 'display') {
                            data =
                                '<a  href="FeesMaintenance?FeesID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                    }
                    else {
                        data = "";
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (check[0].Delete) {
                        if (type === 'display') {
                            data =
                                '<a  onclick = "RemoveFees(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                        }
                    }
                    else {
                        data = "";
                    }
                    
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ],
        
    });

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#BranchInfo_BranchID").val());
    }
});

function LoadBranch(onLoaded) {
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
        if ($("#standardInfo_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#standardInfo_StandardID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveFeeStructure() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fFeesDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.FeesStructure + 'SaveFees', formData, function (data) {
            HideLoader();
            if (data.FeesID>=0) {
                ShowMessage("Fee Structure added Successfully.", "Success");
                window.location.href = "FeesMaintenance?FeesID=0";
            }
            else {
                ShowMessage("Fee Structure Already Exist. ","Error");
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemoveFees(feeID) {
    if (confirm('Are you sure want to delete this Fee Structure?')) {
        ShowLoader();
        var postCall = $.post(commonData.FeesStructure + "RemoveFees", { "FeesID": feeID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Fee Structure Removed Successfully.", "Success");
            window.location.href = "FeesMaintenance?FeesID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}


$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#standardInfo_StandardID').val(Data);
});