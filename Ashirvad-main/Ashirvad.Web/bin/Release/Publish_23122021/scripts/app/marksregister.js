﻿/// <reference path="common.js" />
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
            $("#SubjectName").append("<option value=" + data[i].SubjectID + "_" + data[i].testID + ">" + data[i].Subject + "</option>");
        }

        if ($("#SubjectInfo_SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#SubjectInfo_SubjectID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadTestDates(BatchType) {

    var BranchID = $("#Branch_Name").val();
    var STD = $('#StandardInfo_StandardID').val();
    var BatchType = BatchType;

    if (BranchID > 0 && BatchType > 0 && STD > 0) {
        var postCall = $.post(commonData.TestPaper + "GetTestDatesByBatch", { "BranchID": BranchID, "BatchType": BatchType, "stdID": STD });
        postCall.done(function (data) {
            $('#testddl').empty();
            $('#testddl').select2();
            $("#testddl").append("<option value=" + 0 + ">---Select Test Date---</option>");
            for (i = 0; i < data.length; i++) {
                var test = ConvertDateFrom(data[i].TestDate);
                var TestDate = convertddmmyyyy(test);
                $("#testddl").append("<option value='" + data[i].TestID + "'>" + TestDate + "</option>");
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

function LoadStudentDetails() {
    var STD = $('#testEntityInfo_TestID').val();
    var Subject = $('#SubjectInfo_SubjectID').val();
    var BatchType = $('#BatchType').val();
    var postCall = $.post(commonData.MarksRegister + "GetAllAchieveMarks", { "Std": STD, "Batch": BatchType, "MarksID": Subject});
    postCall.done(function (data) {
        $("#StudentDetail").html(data);
    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function UpdateMarks(MarksID, StudentID) {
    ShowLoader();
    var marks = $("#Marks_" + MarksID).val();
    if (marks != "" && marks != null) {
        var postCall = $.post(commonData.MarksRegister + "UpdateMarksDetails", { "MarksID": MarksID, "StudentID": StudentID, "AchieveMarks": marks });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                LoadStudentDetails();
                //setTimeout(function () { window.location.href = "GetAllAchieveMarks" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    } else {
        HideLoader();
        ShowMessage("Please Enter Achieve Marks!!", "Error");
    }
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
    var SPData = Data.split('_');
    var sub = SPData[0];
    var test = SPData[1];
    $('#SubjectInfo_SubjectID').val(sub);
    $('#testEntityInfo_TestID').val(test);
    LoadStudentDetails();
});

$("#Batchtime").change(function () {
    var Data = $("#Batchtime option:selected").val();
    $('#BatchType').val(Data);
    LoadTestDates(Data);
});

$("#testddl").change(function () {
    var Data = $("#testddl option:selected").val();
    var Text = $("#testddl option:selected").text();
    var Date1 = ConvertData(Text);
    LoadSubject(Date1);
});