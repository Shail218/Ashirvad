/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadCourse();
    var test = LoadFinalYear();
    var test1 = LoadYear();
    CheckData();
});

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

function LoadFinalYear() {
    var financial_year = "";
    var today = new Date();
    if ((today.getMonth() + 1) <= 3) {
        financial_year = (today.getFullYear() - 1) + "-" + today.getFullYear()
    } else {
        financial_year = today.getFullYear() + "-" + (today.getFullYear() + 1)
    }

    $('#YearName').empty();
    $('#YearName').select2();
    $("#YearName").append("<option value=" + 0 + ">---Select Year---</option>");
    if (financial_year != "") {
        $("#YearName").append("<option value='" + financial_year + "'>" + financial_year + "</option>");
        $('#YearName option[value="' + financial_year + '"]').attr("selected", "selected");
        $('#Final_Year').val(financial_year);
    } else {
        $("#YearName").append("<option value='" + financial_year + "'>" + financial_year + "</option>");
    }
   
    return financial_year;
}

function LoadYear() {
    var financial_year = "";
    var today = new Date();
    if ((today.getMonth() + 1) <= 3) {
        financial_year = (today.getFullYear() - 1) + "-" + today.getFullYear()
    } else {
        financial_year = today.getFullYear() + "-" + (today.getFullYear() + 1)
    }

    $('#Year').empty();
    $('#Year').select2();
    $("#Year").append("<option value=" + 0 + ">---Select Year---</option>");
    if (financial_year != "") {
        $("#Year").append("<option value='" + financial_year + "'>" + financial_year + "</option>");
        $('#Year option[value="' + financial_year + '"]').attr("selected", "selected");
        $('#Final_Year').val(financial_year);
    } else {
        $("#Year").append("<option value='" + financial_year + "'>" + financial_year + "</option>");
    }

    return financial_year;
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
});

$("#YearName").change(function () {
    var Data = $("#YearName option:selected").val();
    $('#Final_Year').val(Data);
});
$("#Year").change(function () {
    var Data = $("#Year option:selected").val();
    $('#Final_Year').val(Data);
});
function Filterstudent() {
    ShowLoader();
    var course = $('#BranchCourse_course_dtl_id').val();
    var classname = $('#BranchClass_Class_dtl_id').val();
    var finalyear = $('#Final_Year').val();
    var postCall = $.post(commonData.Student + "GetFilterStudent", { "course": course, "classname": classname, "finalyear": finalyear });
    postCall.done(function (data) {
        $("#studentdetails").html(data);
        LoadYear();
        HideLoader();

    });
}

function OnSelectStatus(Data, classData) {
    if (Data.checked == true) {
        $('#Studenttable .' + classData).each(function () {
            $(this)[0].checked = true;

        });
    }
    else {
        $('#Studenttable .' + classData).each(function () {
            $(this)[0].checked = false;

        });
    }
}

function checkstatus(status) {
    var Create = true;
    $('#Studenttable .createStatus').each(function () {
        if ($(this)[0].checked == false) {
            Create = false;
        }       
    });

    if (Create) {
        $("#allcreate").prop('checked', true);
    }
    else {
        $("#allcreate").prop('checked', false);
    }
    
}

function CheckData() {
    var count = $("#datacount").val();
    if (count == "0") {
        $("#studentdiv").css("height", "0px");
        $("#studentdiv").css("overflow-y", "none");
    }

}
