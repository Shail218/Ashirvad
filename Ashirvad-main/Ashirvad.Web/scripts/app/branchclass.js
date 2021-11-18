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
        if (data != null)
        {
            for (i = 0; i < data.length; i++) {
                $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
            }
        }
        if ($("#BranchCourse_course_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
        }
        
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}




function OnSelectStatus(Data, classData) {
    if (Data.checked == true) {
        $('#choiceList .' + classData).each(function () {
            $(this)[0].checked = true;

        });
    }
    else {
        $('#choiceList .' + classData).each(function () {
            $(this)[0].checked = false;

        });
    }


}


function SaveClassDetail() {
    var Array = [];
    var isSuccess = ValidateData('fBranchClassDetail');
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JsonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.BranchClass + "SaveClassDetails", $('#fBranchClassDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "ClassMaintenance?ClassID=0"; }, 2000);

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
    var ClassStatus = [];
    var ClassData = [];
    var ClassNameData = [];
    var ClassDetailData = [];
    var MainArray = [];

    $('#choiceList .isClass').each(function () {
        var Class = $(this)[0].checked;
        ClassStatus.push(Class);
    });


    $('#choiceList .ClassID').each(function () {
        var ClassID = $(this).val();
        ClassData.push(ClassID);
    });
    $('#choiceList .ClassName').each(function () {
        var ClassName = $(this).val();
        ClassNameData.push(ClassName);
    });
    $('#choiceList .Classdtlid').each(function () {
        var Classdtlid = $(this).val();
        ClassDetailData.push(Classdtlid);
    });

    for (var i = 0; i < ClassData.length; i++) {
        var IsClass = ClassStatus[i];
        var ClassID = ClassData[i];
        var ClassDetailID = ClassDetailData[i];
        var ClassName = ClassNameData[i];

        MainArray.push({
            "Class": { "ClassID": ClassID, "ClassName": ClassName},
            "Class_dtl_id": ClassDetailID,
            "isClass": IsClass,


        })
    }
    return MainArray;
}

function checkstatus() {
    var Create = true;
    $('#choiceList .isClass').each(function () {
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
});
function RemoveClass(CourseID) {
    if (confirm('Are you sure want to delete this?')) {
        ShowLoader();
        var postCall = $.post(commonData.BranchClass + "RemoveClassDetail", { "PackageRightID": CourseID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "ClassMaintenance?ClassID=0"; }, 2000);

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