/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function UpdateCompetitionRank(CompetitionId, CompetitionRankId) {
    ShowLoader();
    var rank = $("#Rank_" + CompetitionRankId).val();
    if (rank != "" && rank != null) {
        var postCall = $.post(commonData.CompetitionRankRegister + "UpdateCompetitionRankDetail", { "CompetitionId": CompetitionId, "CompetitionRankId": CompetitionRankId, "Rank": rank });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                GetStudentDetail(CompetitionId);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    } else {
        HideLoader();
        ShowMessage("Please Enter Rank!!", "Error");
    }
}

function GetStudentDetail(CompetitionId) {
    var postCall = $.post(commonData.CompetitionRankRegister + "GetCompetitionRankStudentListbyCompetitionId", { "CompetitionId": CompetitionId });
    postCall.done(function (data) {
        $('#CompetitionRankData').html(data);
    }).fail(function (xhr) {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}