/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    $("#datepickermarks").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });
   
    var BrandID = $("#Branch_Name").val();
    LoadStandard(BrandID);
    
});

function LoadBranch(onLoaded) {
    ShowLoader();
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

function LoadStandard(branchID) {

    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID });
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

function LoadSubject(branchID) {
    var postCall = $.post(commonData.Subject + "SubjectDataByTestDate", { "TestDate": branchID });
    postCall.done(function (data) {

        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SubjectName").append("<option value=" + data[i].SubjectID + ">" + data[i].Subject + "</option>");
        }

        if ($("#SubjectInfo_SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#SubjectInfo_SubjectID").val() + '"]').attr("selected", "selected");
        }
      
    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadTestDetails(TestID, Subject) {
    var postCall = $.post(commonData.TestPaper + "GetTestDetails", { "TestID": TestID, "SubjectID": Subject, /*"Standard": Standard, "Batch": Batch2*/ });
    postCall.done(function (data) {
        $("#TotalMarks").val(data.Marks);
        $("#Remarks").val(data.Remarks);
        if (data.marksentered) {

            ShowMessage("Marks Already inserted for this Test !", "Error");
            $("#StudentDetail").html('');
        } else {
            var Std = $('#StandardInfo_StandardID').val();
            var Batch = $('#BatchType').val();
            LoadStudentDetails(Std, Batch);
        }
    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

//function LoadTestDetails(TestID,Subject) {
//    var postCall = $.post(commonData.TestPaper + "GetTestDetails", { "TestID": TestID, "SubjectID": Subject});
//    postCall.done(function (data) {
//        $("#TotalMarks").val(data.Marks);
//        $("#Remarks").val(data.Remarks);

//        var Std = $('#StandardInfo_StandardID').val();
//        var Batch = $('#batchEntityInfo_BatchID').val();
//        LoadStudentDetails(Std, Batch);
      

//    }).fail(function () {
//        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
//    });
//}


function SaveMarks() {
    var status = true;
    var MarksData = [];
    Map = {};
    $("#studenttbl tbody tr").each(function () {
        var StudentID = $(this).find("#item_StudentID").val();
        var marks = $(this).find("#Marks").val();
        if (marks == "" || marks == null) {
            status = false;
            return false;
        }
        if (StudentID != null) {
            Map = {
                AchieveMarks: marks,
                student: {
                    StudentID: StudentID
                }
            }
            MarksData.push(Map);
        }
    });
    $('#JsonData').val(JSON.stringify(MarksData));
    var isSuccess = ValidateData('dInformation');
    if (status && isSuccess) {
        ShowLoader();
        var frm = $('#fMarks');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('ImageFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.ResultEntry + 'SaveMarks', formData, function (data) {
            HideLoader();
            if (data.MarksID >= 0) {
                ShowMessage("Marks added Successfully.", "Success");
                window.location.href = "/ResultRegister/Index";
            }
            else {
                ShowMessage('Achieve Marks Already added!!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    } else if(!status){
        ShowMessage('Please Enter Achieve Marks!!', 'Error');
    }
}

function LoadTestDates(BatchType) {

    var BranchID = $("#Branch_Name").val();
    var STD = $('#StandardInfo_StandardID').val();
    var BatchType = BatchType;

    if (BranchID > 0 && BatchType > 0 && STD>0) {
        var postCall = $.post(commonData.TestPaper + "GetTestDatesByBatch", { "BranchID": BranchID, "BatchType": BatchType, "stdID": STD });
        postCall.done(function (data) {
            $('#testddl').empty();
            $('#testddl').select2();
            $("#testddl").append("<option value=" + 0 + ">---Select Test Date---</option>");
            for (i = 0; i < data.length; i++)
            {
                var test = ConvertDateFrom(data[i].TestDate);
                var TestDate = convertddmmyyyy(test);
                $("#testddl").append("<option value='" + data[i].TestID + "'>" + TestDate+ "</option>");
            }
           
            HideLoader();
        }).fail(function () {
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
    else {
        $('#testddl').empty();
        $('#testddl').select2();
       
    }
}

function LoadStudentDetails(Std, Batch)
{
    var postCall = $.post(commonData.ResultEntry + "GetStudentByStd", { "Std": Std, "BatchTime": Batch });
    postCall.done(function (data) {
        $("#StudentDetail").html(data);


    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {

    var Data = $("#BranchName option:selected").val();
    $('#BranchInfo_BranchID').val(Data);
    LoadSubject(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#StandardInfo_StandardID').val(Data);
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    var Data1= $("#testddl option:selected").val();
    $('#SubjectInfo_SubjectID').val(Data);
    LoadTestDetails(Data1, Data);
    
});

$("#Batchtime").change(function () {
    var Data = $("#Batchtime option:selected").val();
    $('#BatchType').val(Data);
    LoadTestDates(Data);
});

$("#testddl").change(function () {                                                                               
    var Data = $("#testddl option:selected").val();
    var Text = $("#testddl option:selected").text();
    $('#testEntityInfo_TestID').val(Data);
    LoadSubject(Text);
});
