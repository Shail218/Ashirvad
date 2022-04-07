/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    LoadCourse();
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

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
});



$("#Status").change(function () {
    var Data = $("#Status option:selected").val();
    $('#RowStatus').val(Data);
});

$("#Statusddl").change(function () {
    var Data = $("#Statusddl option:selected").val();
    $('#Next_Class').val(Data);
});


function FilterstudentStatusWise() {
    ShowLoader();
    var course = $('#BranchCourse_course_dtl_id').val();
    var classname = $('#BranchClass_Class_dtl_id').val();
    var status = $('#RowStatus').val();

    var postCall = $.post(commonData.Student + "GetFilterStudentStatusWise", { "course": course, "classname": classname, "status": status });
    postCall.done(function (data) {
        $("#studentdetails").html(data);
        HideLoader();

    });
}

function ChangeStudentStatus() {
    ShowLoader();
    var Listdata = JSON.stringify(GetData());
    var postCall = $.post(commonData.Student + "ChangeStudentStatus", { "Studentdata": Listdata });
    postCall.done(function (data) {

        HideLoader();
        if (data.Status) {
            ShowMessage(data.Message, "Success");
            setTimeout(function () { window.location.href = "StudentStatusChange" }, 2000);
        } else {
            ShowMessage(data.Message, "Error");
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function GetData() {
    var MainArray = [];
    var course = $('#BranchCourse_course_dtl_id').val();
    var classname = $('#BranchClass_Class_dtl_id').val();
    var status = $("#Statusddl option:selected").val();
    $('#Studenttable tbody tr').each(function () {
        var IsCheck = $(this).find("#Createstatus")[0].checked

        if (IsCheck) {
            var StudentID = $(this).find("#item_StudentID").val();

            MainArray.push({
                "StudentID": StudentID,
                "RowStatus": {"RowStatus":status}
            })

        }
    });

    return MainArray;
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
    HideLoader();
}
