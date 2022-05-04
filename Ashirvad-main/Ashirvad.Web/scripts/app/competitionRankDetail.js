/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />
$(document).ready(function () {
    LoadCompetition();
});
function LoadCompetition() {
    var postCall = $.post(commonData.CompetitionRank + "GetCompetitionData");
    postCall.done(function (data) {
        $('#CompetitionNamedrop').empty();
        $('#CompetitionNamedrop').select2();
        $("#CompetitionNamedrop").append("<option value=" + 0 + ">---Select Competition---</option>");
        for (i = 0; i < data.Result.length; i++) {
            $("#CompetitionNamedrop").append("<option value=" + data.Result[i].CompetitionID + ">" + data.Result[i].CompetitionName + "</option>");
        }
    }).fail(function () {
    });
}

$("#CompetitionNamedrop").change(function () {
    var Data = $("#CompetitionNamedrop option:selected").val();
    $('#CompetitionID').val(Data);
});

function GetStudentList() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        var competitonID = $('#CompetitionID').val();
        ShowLoader();
        var postCall = $.post(commonData.CompetitionRank + "GetStudentListforCompetitionRank", { "competitonID": competitonID });
        postCall.done(function (data) {
            HideLoader();
            $('#StudentRankList').html(data);
            var Sat = $("#status").val();
            var Mess = $("#message").val();
            if (Sat == "True") {

            } else {
                ShowMessage("" + Mess + "", "Error");
            }


        }).fail(function (xhr) {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function SaveCompetitionRank() {
    ShowLoader();
    var CompetitionRankData = [];
    Map = {};
    var CompetitionId = $('#CompetitionID').val();
    $("#studenttbl tbody tr").each(function () {
        var BranchId = $(this).find("#item_branchInfo_BranchID").val();
        var ClassId = $(this).find("#item_studentInfo_BranchClass_Class_ClassID").val();
        var StudentId = $(this).find("#item_studentInfo_StudentID").val();
        var CourseId = $(this).find("#item_studentInfo_BranchClass_BranchCourse_course_CourseID").val();
        var Remarks = $(this).find("#item_Remarks").val();
        if (StudentId != null) {
            Map = {
                branchInfo: {
                    BranchID: BranchId
                },
                studentInfo: {
                    BranchClass: {
                        Class: {
                            ClassID: ClassId
                        },
                        BranchCourse: {
                            course: {
                                CourseID: CourseId
                            }
                        }
                    },
                    StudentID: StudentId
                },
                competitionRank: Remarks,
                competitionInfo: {
                    CompetitionID: CompetitionId
                }
            }
            CompetitionRankData.push(Map);
        }
    });
    var jsonData = JSON.stringify(CompetitionRankData);
    var postCall = $.post(commonData.CompetitionRank + "CompetitionRankMaintenance", { "JsonData": jsonData });
    postCall.done(function (data) {
        HideLoader();
        if (data.Status) {
            ShowMessage(data.Message, "Success");
            window.location.href;
        } else {
            ShowMessage(data.Message, "Error");
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}