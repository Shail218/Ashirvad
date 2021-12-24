/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    $("#datepickeradmission").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    $("#datepickerbirth").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

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
    }

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadSchoolName(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadSchoolName($("#BranchInfo_BranchID").val());
        LoadStandard($("#BranchInfo_BranchID").val());
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
    //if ($("#StudImage").val() != "") {
    //    $('#imgStud').attr('src', "data:image/jpg;base64," + $("#StudImage").val());
    //}

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

function LoadSchoolName(branchID) {
    var postCall = $.post(commonData.School + "SchoolData", { "branchID": branchID});
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

function LoadStandard(branchID) {
    
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID});
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if ($("#StandardInfo_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#StandardInfo_StandardID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
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
        var a = $("#stupass").val();
        $("#StudentPassword").val(a);
        var b = $("#parentpass").val();
        $("#StudentMaint_ParentPassword").val(b);
        var date1 = $("#DOB").val();
        $("#DOB").val(ConvertData(date1));
        var date2 = $("#AdmissionDate").val();
        $("#AdmissionDate").val(ConvertData(date2));
        var frm = $('#fStudentDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Student + 'SaveStudent', formData, function (data) {
            HideLoader();
            ShowMessage("Student added Successfully.", "Success");
            window.location.href = "StudentMaintenance?studentID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveStudent(studentID) {
    ShowLoader();
    var postCall = $.post(commonData.Student + "RemoveStudent", { "studentID": studentID });
    postCall.done(function (data) {
        HideLoader();
        ShowMessage("Student Removed Successfully.", "Success");
        window.location.href = "StudentMaintenance?studentID=0";
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadSchoolName(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#StandardInfo_StandardID').val(Data);
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