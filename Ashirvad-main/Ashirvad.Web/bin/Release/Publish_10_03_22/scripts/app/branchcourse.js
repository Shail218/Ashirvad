/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    var IsEdit = $("#IsEdit").val();
    if (IsEdit == "True") {
        checkstatus('old');
    }   
});

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


function SaveCourseDetail() {
    var Array = [];
    var isSuccess = true;
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JsonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.BranchCourse + "SaveCourseDetails", $('#fBranchCourseDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "CourseMaintenance?courseID=0"; }, 2000);

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
    var CourseStatus = [];
    var CourseData = [];
    var CourseDetailData = [];
    var MainArray = [];

    $('#choiceList .iscourse').each(function () {
        var Course = $(this)[0].checked;
        CourseStatus.push(Course);
    });

    
    $('#choiceList .CourseID').each(function () {
        var CourseID = $(this).val();
        CourseData.push(CourseID);
    });
    $('#choiceList .coursedtlid').each(function () {
        var coursedtlid = $(this).val();
        CourseDetailData.push(coursedtlid);
    });
    
    for (var i = 0; i < CourseData.length; i++) {
        var IsCourse = CourseStatus[i];
        var CourseID = CourseData[i];
        var CourseDetailID = CourseDetailData[i];
        
        
        MainArray.push({
            "course": { "CourseID": CourseID },
            "course_dtl_id": CourseDetailID,
            "iscourse": IsCourse,
            

        })
    }
    return MainArray;
}

function checkstatus(status) {
    var Create = true;  
    $('#choiceList .iscourse').each(function ()
    {
        if ($(this)[0].checked == false)
        {
            Create = false;
        }
        //if ($(this)[0].checked == true) {
        //    var IsEdit = $("#IsEdit").val();
        //    if (IsEdit == "True" && status == "old") {
        //        $(this).prop("disabled", true);
        //    }

        //}
    });
    if (Create) {
        $("#allselect").prop('checked', true);
    }
    else {
        $("#allselect").prop('checked', false);
    }
    var id = $(status).parents('tr').find("#course_detailid").val();
    var postCall = $.post(commonData.BranchCourse + "Check_CourseDetail", { "coursedetailid": id });
    postCall.done(function (data) {       
        if (data.Status == true) {
                    
        }
        else {
            ShowMessage(data.Message, 'Error');
            $(status).prop('checked', true);
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function RemoveCourse(CourseID) {
    if (confirm('Are you sure want to delete this?')) {
        ShowLoader();
        var postCall = $.post(commonData.BranchCourse + "RemoveCourseDetail", { "PackageRightID": CourseID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "CourseMaintenance?courseID=0"; }, 2000);

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