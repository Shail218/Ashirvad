/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    if ($("#ToDoID").val() > 0) {
        $("#fuDocument").addClass("editForm");
    }
    var check = GetUserRights('To-DoEntry');
    var table = $('#todotable').DataTable({
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
            url: "" + GetSiteURL() + "/ToDo/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "columns": [
            { "data": "ToDoDate" },
            { "data": "UserInfo.Username" },
            { "data": "ToDoDescription" },
            { "data": "FilePath" },
            { "data": "ToDoID" },
            { "data": "ToDoID" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = ConvertMiliDateFrom(data)
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 3,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<a href= "' + full.FilePath.replace("https://mastermind.org.in", "") + '" id="paperdownload" download="' + full.ToDoFileName + '"> <img src="../ThemeData/images/icons8-desktop-download-24 (1).png" /></a>'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        if (check[0].Create) {
                            data = '<a href="ToDoMaintenance?todoID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
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
                                '<a onclick = "RemoveToDo(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
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

    $("#datepickertodo").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        defaultDate: new Date(),

    });

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");    
        }
        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadUser(commonData.BranchID);
            HideLoader();
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadUser($("#BranchInfo_BranchID").val());
    } else {
        $("#ToDoDate").val(setCurrentDate());

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

function LoadUser(branchID) {
    
    var postCall = $.post(commonData.UserPermission + "GetAllUsers", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#UserName').empty();
        $('#UserName').select2();
        $("#UserName").append("<option value=" + 0 + ">---Select User---</option>");
        for (i = 0; i < data.length; i++) {
            $("#UserName").append("<option value=" + data[i].UserID + ">" + data[i].Username + "</option>");
        }
        if ($("#UserInfo_UserID").val() != "") {
            $('#UserName option[value="' + $("#UserInfo_UserID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function SaveToDo() {   
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#ToDoDate").val();
        $("#ToDoDate").val(ConvertData(date1));
        var frm = $('#fToDoDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.ToDo + 'SaveToDo', formData, function (data) {           
            if (data) {
                HideLoader();
                ShowMessage('ToDo details saved!', 'Success');
                window.location.href = "ToDoMaintenance?todoID=0";
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

function RemoveToDo(todoID) {
    if (confirm('Are you sure want to delete this ToDo Task?')) {
        ShowLoader();
        var postCall = $.post(commonData.ToDo + "RemoveToDo", { "todoID": todoID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("ToDo Removed Successfully.", "Success");
            window.location.href = "ToDoMaintenance?todoID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function DownloadToDo(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.ToDo + "Downloadtodo", { "todoid": branchID });
    postCall.done(function (data) {
        HideLoader();
        if (data != null) {
            var a = document.createElement("a"); //Create <a>
            a.href = "data:" + data[3] + ";base64," + data[1]; //Image Base64 Goes here
            a.download = data[2];//File name Here
            a.click(); //Downloaded file
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function ConvertMiliDateFrom(date) {
    if (date != null) {
        var sd = date.split("/Date(");
        var sd2 = sd[1].split(")/");
        var date1 = new Date(parseInt(sd2[0]));
        var d = date1.getDate();
        var m = date1.getMonth() + 1;
        var y = date1.getFullYear();
        var hr = date1.getHours();
        var min = date1.getMinutes();
        var sec = date1.getSeconds();

        if (parseInt(d) < 10) {
            d = "0" + d;
        }
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        var Final = d + "-" + m + "-" + y + " ";
        var d = date1.toString("dd/MM/yyyy HH:mm:SS");
        return Final;
    }
    return "";
}

$("#BranchName").change(function () {   
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadUser(Data);
});

$("#UserName").change(function () {    
    var Data = $("#UserName option:selected").val();
    $('#UserInfo_UserID').val(Data);
});