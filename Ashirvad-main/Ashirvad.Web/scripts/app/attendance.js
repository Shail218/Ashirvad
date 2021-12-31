/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    $("#datepickerattendance").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    table = $('#attendancetable').DataTable({
        "bLengthChange": false
    });

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#Branch_BranchID").val());
    }

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

        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
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
        if ($("#Standard_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#Standard_StandardID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function ValidateAttendanceData() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#AttendanceDate").val();
        $("#AttendanceDate").val(ConvertData(date1));
        var postCall = $.post(commonData.AttendanceEntry + "VerifyAttendanceRegister", $('#fAttendanceReportDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                GetStudentDetail();
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            //ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function GetStudentDetail() {    
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var STD = $('#Standard_StandardID').val();
        var BatchTime = $('#BatchTypeID').val();
        table.destroy();
        table = $('#attendancetable').DataTable({
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
                url: "" + GetSiteURL() + "/AttendanceEntry/CustomServerSideSearchAction?STD='" + STD + "'&BatchTime='" + BatchTime + "'",
                type: 'POST',
                "data": function (d) {
                    HideLoader();
                    d.STD = STD;
                    d.BatchTime = BatchTime;
                }
            },
            "columns": [
                { "data": "Name" },
                { "data": "StudentID" }
            ],
            "columnDefs": [
                {
                    targets: 1,
                    render: function (data, type, full, meta) {
                        if (type === 'display') {
                            data = `<input type="checkbox" value="` + data + `" name="cb" id = "cb"/> <span style="margin-left:20px;">Absent</span>
                                <input hidden value = `+ full.StudentID +` Id = "StudentID" />
                                <input hidden value = `+ full.GrNo +` Id = "GrNo" />`
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
                                '<input name="Remarks" class = "remark" alt="Remarks" autocomplete="off" id="Remarks" />'
                        }
                        return data;
                    },
                    orderable: false,
                    searchable: false
                }
            ]
        });
    }
}

function SaveAttendance() {
    
    var AttendanceData = [];
    Map = {};
    $("#attendancetable tbody tr").each(function () {       
        var IsAbsent, IsPresent;
        var Remarks = $(this).find("#Remarks").val();
        var StudentID = $(this).find("#StudentID").val();
        var checked = $(this).find("#cb").prop("checked");
        if (checked) {
            IsAbsent = true;
            IsPresent = false;
        } else {
            IsAbsent = false;
            IsPresent = true;
        }
        if (StudentID != null) {
            Map = {
                IsAbsent: IsAbsent,
                IsPresent: IsPresent,
                Remarks: Remarks,
                Student: {
                    StudentID: StudentID
                }
            }
            AttendanceData.push(Map);
        }
    });
    //var date1 = $("#AttendanceDate").val();
    //$("#AttendanceDate").val(ConvertData(date1));
    $('#JsonData').val(JSON.stringify(AttendanceData));
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader(); 
        var postCall = $.post(commonData.AttendanceEntry + "AttendanceMaintenance", $('#fAttendanceReportDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Attendance added Successfully.", "Success");
            window.location.href = "/AttendanceRegister/Index";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveStudent(studentID) {
    
    var postCall = $.post(commonData.Student + "RemoveStudent", { "studentID": studentID });
    postCall.done(function (data) {
        
        ShowMessage("Student Removed Successfully.", "Success");
        window.location.href = "StudentMaintenance?studentID=0";
    }).fail(function () {
        
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#Standard_StandardID').val(Data);
});

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $('#BatchTypeID').val(Data);
});
