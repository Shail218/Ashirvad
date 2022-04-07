/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    
    LoadBranch();

    ShowLoader();
    table = $('#Studenttable').DataTable({
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
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)

        },
        "columns": [
            { "data": "FilePath" },
            { "data": "Name" },
            { "data": "AdmissionDate" },
            { "data": "BranchCourse.course.CourseName" },
            { "data": "BranchClass.Class.ClassName" },
            { "data": "BatchInfo.BatchText" },
            { "data": "ContactNo" },
            { "data": "StudentID"}
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = (data == null || data == "https://mastermind.org.in") ? '<img src="../ThemeData/images/Default.png" id="branchImg" style="height:60px;width:60px;margin-left:20px;" />' : '<img src = "' + data + '" style="height:60px;width:60px;margin-left:20px;"/>'
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
                targets: 6,
                orderable: true,
                searchable: true
            },
            {
                targets: 7,
                render: function (data, type, full, meta) {
                   
                    if (type === 'display') {
                        data = full.RowStatus.RowStatusText;                           
                    }
                    HideLoader();
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 8,
                render: function (data, type, full, meta) {
                    
                    if (type === 'display') {
                        data =
                            '<a class="ManageStudentMasterCreate" onclick = "EditStudent(' + full.StudentID + ')"><img src = "../ThemeData/images/viewIcon.png" /></a >';
                    }
                    HideLoader();
                    CheckRights();
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

function ChangeStatusStudent(Status) {
    ShowLoader();
    table.destroy();
     table = $('#Studenttable').DataTable({
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
            "data": function (d) {
                
                HideLoader();
                d.status = Status;
               
            }

        },
        "columns": [
            { "data": "FilePath" },
            { "data": "Name" },
            { "data": "AdmissionDate" },
            { "data": "BranchCourse.course.CourseName" },
            { "data": "BranchClass.Class.ClassName" },
            { "data": "BatchInfo.BatchText" },
            { "data": "ContactNo" },
            { "data": "StudentID"}
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {
                    debugger;
                    if (type === 'display') {
                        data = (data == null || data == "https://mastermind.org.in") ? '<img src="../ThemeData/images/Default.png" id="branchImg" style="height:60px;width:60px;margin-left:20px;" />' : '<img src = "' + data + '" style="height:60px;width:60px;margin-left:20px;"/>'
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
                targets: 6,
                orderable: true,
                searchable: true
            },
            {
                targets: 7,
                render: function (data, type, full, meta) {
                   
                    if (type === 'display') {
                        data = full.RowStatus.RowStatusText;
                            
                    }
                   
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 8,
                render: function (data, type, full, meta) {
                    
                    if (type === 'display') {
                        data =
                            '<a onclick = "EditStudent(' + full.StudentID + ')"><img src = "../ThemeData/images/viewIcon.png" /></a >';
                    }
                    
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });
}

function RemoveStudent(studentID) {
    if (confirm('Are you sure want to delete this Student?')) {
        ShowLoader();
        var postCall = $.post(commonData.ManageStudent + "Removestudent", { "studentID": studentID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                window.location.href = "ManageStudentMaintenance?branchID=0";
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