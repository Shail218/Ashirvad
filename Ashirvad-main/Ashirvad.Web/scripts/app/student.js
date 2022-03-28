/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadCount();
    $("#datepickeradmission").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        defaultDate: new Date(),

    });

    $("#datepickerbirth").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        defaultDate: new Date(),

    });

    if ($("#RowStatus_RowStatusId").val() != "") {

        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }

    if ($("#LastYearResult").val() != "") {

        var rowStatus = $("#LastYearResult").val();
        if (rowStatus == "1") {
            $("#rowStaPass").attr('checked', 'checked');
        }
        else {
            $("#rowStaFail").attr('checked', 'checked');
        }
    } else {
        $("#DOB").val(setCurrentDate());
        $("#AdmissionDate").val(setCurrentDate());

    }

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadSchoolName(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadSchoolName($("#BranchInfo_BranchID").val());
    }

    if ($("#SchoolTime").val() != "") {
        $('#SchoolTimeDDL option[value="' + $("#SchoolTime").val() + '"]').attr("selected", "selected");
    }

    if ($("#BatchInfo_BatchTime").val() != "") {
        $('#BatchTime option[value="' + $("#BatchInfo_BatchTime").val() + '"]').attr("selected", "selected");
    }

    if ($("#StudentID").val() > 0) {
        $("#fuStudentImage").addClass("editForm");
    }

    LoadCourse();
});

function LoadCount() {
    debugger;
    var postCall = $.post(commonData.Student + "getcount");
    postCall.done(function (response) {
        HideLoader();
        if (!response.Status) {
            ShowMessage(response.Message, "Error");
            $("#savebtn").html("");
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

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

function LoadSchoolName(branchID) {
    var postCall = $.post(commonData.School + "SchoolData", { "branchID": branchID });
    postCall.done(function (data) {
        $('#SchoolName').empty();
        $('#SchoolName').select2();
        $("#SchoolName").append("<option value=" + 0 + ">---Select School Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SchoolName").append("<option value=" + data[i].SchoolID + ">" + data[i].SchoolName + "</option>");
        }
        if ($("#SchoolInfo_SchoolID").val() != "") {
            $('#SchoolName option[value="' + $("#SchoolInfo_SchoolID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function (e) {
        console.log(e);
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadCourse() {
    var postCall = $.post(commonData.BranchCourse + "GetCourseDDL");
    postCall.done(function (data) {
        $('#CourseName').empty();
        $('#CourseName').select2();
        $("#CourseName").append("<option value=" + 0 + ">---Select Course---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                if (data.length == 1) {
                    $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
                    $('#CourseName option[value="' + data[i].course_dtl_id + '"]').attr("selected", "selected");
                    $('#BranchCourse_course_dtl_id').val(data[i].course_dtl_id);
                } else {
                    $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
                }
            }
        }

        if ($("#BranchCourse_course_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
            LoadClass($("#BranchCourse_course_dtl_id").val());
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadClass(CourseID) {
    ShowLoader();
    var postCall = $.post(commonData.BranchClass + "GetClassDDL", { "CourseID": CourseID });
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                if (data.length == 1) {
                    $("#StandardName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
                    $('#StandardName option[value="' + data[i].Class_dtl_id + '"]').attr("selected", "selected");
                    $('#BranchClass_Class_dtl_id').val(data[i].Class_dtl_id);
                } else {
                    $("#StandardName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
                }
            }
        }

        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#StandardName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        HideLoader();
    });
}

function SaveStudent() {
    var id = $("#StudentID").val();
    if (id > 0) {
        $("#StudentPassword1").removeClass('required');

        $("#ParentPassword1").removeClass('required');

    }
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        //var a = $("#StudentPassword1").val();
        //$("#StudentPassword").val(a);
        //var b = $("#ParentPassword1").val();
        //$("#StudentMaint_ParentPassword").val(b);
        var date1 = $("#DOB").val();
        $("#DOB").val(ConvertData(date1));
        var date2 = $("#AdmissionDate").val();
        $("#AdmissionDate").val(ConvertData(date2));
        $("#Final_Year").val(getCurrentFinancialYear());
        var frm = $('#fStudentDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Student + 'SaveStudent', formData, function (data) {
            if (data) {
                HideLoader();
                if (data.Success) {
                    ShowMessage(data.Message, 'Success');
                    window.location.href = "StudentMaintenance?studentID=0";
                } else {
                    ShowMessage(data.Message, 'Error');
                }
            }
            else {
                HideLoader();
                ShowMessage("An unexpected error occcurred while processing request!", "Error");
            }
        }).fail(function () {
            HideLoader();
            //ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function getCurrentFinancialYear() {
    var fiscalyear = "";
    var today = new Date();
    if ((today.getMonth() + 1) <= 3) {
        fiscalyear = (today.getFullYear() - 1) + "-" + today.getFullYear()
    } else {
        fiscalyear = today.getFullYear() + "-" + (today.getFullYear() + 1)
    }
    return fiscalyear
}

function RemoveStudent(studentID) {
    if (confirm('Are you sure want to delete this Student?')) {
        ShowLoader();
        var postCall = $.post(commonData.Student + "RemoveStudent", { "studentID": studentID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Success) {
                ShowMessage(data.Message, "Success");
                window.location.href = "StudentMaintenance?studentID=0";
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
    LoadSchoolName(Data);
    LoadStandard(Data);
});

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
});

$("#SchoolName").change(function () {
    var Data = $("#SchoolName option:selected").val();
    $('#SchoolInfo_SchoolID').val(Data);
});

$("#SchoolTimeDDL").change(function () {
    var Data = $("#SchoolTimeDDL option:selected").val();
    $('#SchoolTime').val(Data);
});

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $('#BatchInfo_BatchType').val(Data);
});

$("#fuStudentImage").change(function () {
    readURL(this);
});

$('input[type=radio][name=Status]').change(function () {
    if (this.value == '1') {
        $("#RowStatus_RowStatusId").val(1);
    }
    else {
        $("#RowStatus_RowStatusId").val(2);
    }
});

$('input[type=radio][name=rdbResultofLastYear]').change(function () {
    if (this.value == '1') {
        $("#LastYearResult").val(1);
    }
    else {
        $("#LastYearResult").val(2);
    }
});

//function lastyear(row) {
//    $("#LastYearResult").val(row.val());
//}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.readAsDataURL(input.files[0]);
        reader.onload = function (e) {

            $('#imgStud').attr('src', e.target.result);
            var bas = reader.result;
            var PANtUploadval = bas;
            var ssmdPAN = PANtUploadval.replace("data:image/*;base64,", "")
            var code = ssmdPAN.split(",");
            $('#StudImage').val(code[1]);
        }
    }
}