/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    
    LoadBranch();

    ShowLoader();
    var table = $('#Studenttable').DataTable({
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
            url: "" + GetSiteURL() + "/ManageStudent/CustomServerSideSearchAction",
            type: 'POST',

        },
        "columns": [
            { "data": "FilePath" },
            { "data": "Name" },
            { "data": "AdmissionDate" },
            { "data": "StandardInfo.Standard" },
            { "data": "BatchInfo.BatchText" },
            { "data": "ContactNo" },
            { "data": "StudentID"}
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<img src = "' + data + '" style="height:60px;width:60px;margin-left:20px;"/>'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 1,
                orderable: true,
                searchable: true
            },
            {
                targets: 2,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = ConvertMiliDateFrom(data)
                    }
                    return data;
                },
                orderable: true,
                searchable: true
            },
            {
                targets: 5,
                orderable: true,
                searchable: true
            },
            {
                targets: 6,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data =
                            '<a onclick = "EditStudent(' + data + ')"><img src = "../ThemeData/images/viewIcon.png" /></a >';                           
                    }
                    HideLoader();
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });
});

function EditStudent(ID) {
    window.location = GetSiteURL() + "Student/StudentMaintenance?studentID=" + ID
}

function LoadBranch() {
   
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadActiveStudent() {
    ShowLoader();
    var Data = commonData.BranchID;
    var postCall = $.post(commonData.ManageStudent + "GetAllActiveStudent", { "branchID": Data });
    postCall.done(function (data) {
        HideLoader();
        $('#studentData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadInActiveStudent() {
    ShowLoader();
    var Data = commonData.BranchID;
    var postCall = $.post(commonData.ManageStudent + "GetAllInActiveStudent", { "branchID": Data });
    postCall.done(function (data) {
        HideLoader();
        $('#studentData').html(data);
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function RemoveStudent(studentID) {
    if (confirm('Are you sure want to delete this Student?')) {
        ShowLoader();
        var postCall = $.post(commonData.ManageStudent + "Removestudent", { "studentID": studentID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Student Removed Successfully.", "Success");
            window.location.href = "ManageStudentMaintenance?branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchID').val(Data);
});

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
        //return date1.toString("dd/MM/yyyy HH:mm:SS");
        return Final;
    }
    return "";;
}