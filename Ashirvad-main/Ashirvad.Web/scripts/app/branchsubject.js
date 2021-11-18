/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    //ShowLoader();

    LoadCourse();
 
    var IsEdit = $("#IsEdit").val();
    if (IsEdit == "True") {
        checkstatus();
    }

});

function LoadCourse() {
    var postCall = $.post(commonData.BranchCourse + "GetCourseDDL");
    postCall.done(function (data) {
        $('#CourseName').empty();
        $('#CourseName').select2();
        $("#CourseName").append("<option value=" + 0 + ">---Select Course---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
            }
        }
        if ($("#BranchCourse_course_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
        }
        var IsEdit = $("#IsEdit").val();
        if (IsEdit == "True") {
            LoadClass($("#BranchCourse_course_dtl_id").val());
        }
        
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadClass(CourseID) {
    var postCall = $.post(commonData.BranchClass + "GetClassDDL", { "CourseID": CourseID });
    postCall.done(function (data) {
        $('#ClassName').empty();
        $('#ClassName').select2();
        $("#ClassName").append("<option value=" + 0 + ">---Select Class---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#ClassName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
            }
        }
        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#ClassName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
        }

        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function OnSelectStatus(Data, SubjectData) {
    if (Data.checked == true) {
        $('#choiceList .' + SubjectData).each(function () {
            $(this)[0].checked = true;

        });
    }
    else {
        $('#choiceList .' + SubjectData).each(function () {
            $(this)[0].checked = false;

        });
    }


}


function SaveSubjectDetail() {
    var Array = [];
    var isSuccess = ValidateData('fBranchSubjectDetail');
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JsonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.BranchSubject + "SaveSubjectDetails", $('#fBranchSubjectDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "SubjectMaintenance?SubjectID=0&&CourseID=0"; }, 2000);

            }
            else {
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function GetData() {
    var SubjectStatus = [];
    var SubjectData = [];
    var SubjecNameData = [];
    var SubjectDetailData = [];
    var MainArray = [];

    $('#choiceList .isSubject').each(function () {
        var Subject = $(this)[0].checked;
        SubjectStatus.push(Subject);
    });

    $('#choiceList .SubjectName').each(function () {
        var SubjectName = $(this).val();
        SubjecNameData.push(SubjectName);
    });
    $('#choiceList .SubjectID').each(function () {
        var SubjectID = $(this).val();
        SubjectData.push(SubjectID);
    });
    $('#choiceList .Subjectdtlid').each(function () {
        var Subjectdtlid = $(this).val();
        SubjectDetailData.push(Subjectdtlid);
    });

    for (var i = 0; i < SubjectData.length; i++) {
        var IsSubject = SubjectStatus[i];
        var SubjectID = SubjectData[i];
        var SubjectDetailID = SubjectDetailData[i];
        var SubjectName = SubjecNameData[i];

        MainArray.push({
            "Subject": { "SubjectID": SubjectID, "SubjectName": SubjectName},
            "Subject_dtl_id": SubjectDetailID,
            "isSubject": IsSubject,


        })
    }
    return MainArray;
}

function checkstatus() {
    var Create = true;
    $('#choiceList .isSubject').each(function () {
        if ($(this)[0].checked == false) {
            Create = false;
        }
    });
    if (Create) {
        $("#allselect").prop('checked', true);
    }
    else {
        $("#allselect").prop('checked', false);
    }
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#ClassName").change(function () {
    var Data = $("#ClassName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
    
});
function RemoveSubject(CourseID,ClassID) {
    if (confirm('Are you sure want to delete this?')) {
        ShowLoader();
        var postCall = $.post(commonData.BranchSubject + "RemoveSubjectDetail", { "CourseID": CourseID, "ClassID": ClassID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "SubjectMaintenance?SubjectID=0&&CourseID=0"; }, 2000);

            }
            else {
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}