/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />
$(document).ready(function () {
    LoadBranch(function () {
        if ($("#competitionRankInfo_branchInfo_BranchID").val() != "") {
            $('#Branch option[value="' + $("#competitionRankInfo_branchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }
    });
    if ($("#competitionRankInfo_branchInfo_BranchID").val() != "") {
        $('#Branch option[value="' + $("#competitionRankInfo_branchInfo_BranchID").val() + '"]').attr("selected", "selected");
    }
    LoadCompetition(function () {
        if ($("#competitionInfo_CompetitionID").val() != "") {
            $('#Comp option[value="' + $("#competitionInfo_CompetitionID").val() + '"]').attr("selected", "selected");
        }
        LoadCompetitionStudent();
    });
    if ($("#competitionInfo_CompetitionID").val() != "") {
        $('#Comp option[value="' + $("#competitionInfo_CompetitionID").val() + '"]').attr("selected", "selected");
    }

});
function LoadBranch() {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        $('#Branch').empty();
        $('#Branch').select2();
        $("#Branch").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#Branch").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }
        if ($("#competitionRankInfo_branchInfo_BranchID").val() != "") {
            $('#Branch option[value="' + $("#competitionRankInfo_branchInfo_BranchID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadCompetition() {
    var postCall = $.post(commonData.CompetitionRank + "GetCompetitionData");
    postCall.done(function (data) {
        $('#Comp').empty();
        $('#Comp').select2();
        $("#Comp").append("<option value=" + 0 + ">---Select Competition---</option>");
        for (i = 0; i < data.Result.length; i++) {
            $("#Comp").append("<option value=" + data.Result[i].CompetitionID + ">" + data.Result[i].CompetitionName + "</option>");
        }
        if ($("#competitionInfo_CompetitionID").val() != "") {
            $('#Comp option[value="' + $("#competitionInfo_CompetitionID").val() + '"]').attr("selected", "selected");
            LoadCompetitionStudent();
        }
    }).fail(function () {
    });
}
function LoadCompetitionStudent() {
    var competitonID = $("#Comp option:selected").val();
    var BranchId = $("#Branch option:selected").val();
    if (competitonID != 0 && BranchId != 0) {

        var postCall = $.post(commonData.WinnerEntry + "GetStudentListbyCompetitionIdandBranchId", { "CompetitionId": competitonID, "BranchId": BranchId });
        postCall.done(function (data) {
            $('#Student').empty();
            $('#Student').select2();
            $("#Student").append("<option value=" + 0 + ">---Select Student---</option>");
            for (i = 0; i < data.length; i++) {
                $("#Student").append("<option value=" + data[i].CompetitionRankId + ">" + data[i].studentInfo.Name + " - " + data[i].studentInfo.BranchClass.BranchCourse.course.CourseName + " - " + data[i].studentInfo.BranchClass.Class.ClassName+ "</option>");
            }
            if ($("#competitionRankInfo_CompetitionRankId").val() != "") {
                $('#Student option[value="' + $("#competitionRankInfo_CompetitionRankId").val() + '"]').attr("selected", "selected");
            }
        }).fail(function () {
        });
    }
}
$("#Comp").change(function () {
    var Data = $("#Comp option:selected").val();
    $('#competitionInfo_CompetitionID').val(Data);
    LoadCompetitionStudent();
});

$("#Branch").change(function () {
    var Data = $("#Branch option:selected").val();
    $('#competitionRankInfo_branchInfo_BranchID').val(Data);
    LoadCompetitionStudent();
});

$("#Student").change(function () {
    var Data = $("#Student option:selected").val();
    $('#competitionRankInfo_CompetitionRankId').val(Data);
});

function SaveCompetitionWinner() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.WinnerEntry + "SaveCompetitionWinner", $('#fCompetitionWinner').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "CompetitionWinnerMaintenance?CompetitionWinnerId=0" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}
function RemoveCompetitionWinner(competitionWinnerID) {
    if (confirm('Are you sure want to delete this Competition Winner?')) {
        ShowLoader();
        var postCall = $.post(commonData.WinnerEntry + "RemoveCompetitionWinner", { "CompetitionWinnerId": competitionWinnerID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "CompetitionWinnerMaintenance?CompetitionWinnerId=0" }, 2000);
            }
            else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}
