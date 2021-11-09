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
                setTimeout(function () { window.location.href = "PackageRightMaintenance?PackageRightID=0"; }, 2000);

            }
            else {
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function GetData() {
    var CourseStatus = [];
    var CourseData = [];
    var CourseDetailData = [];

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
            "course": { "CourseID": IsCourse },
            "course_dtl_id": CourseDetailID,
            "iscourse": IsCourse,
            

        })
    }
    return MainArray;
}