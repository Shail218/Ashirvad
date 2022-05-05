/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function UpdateCompetitionRank(CompetitionId, CompetitionRankId,StudentId) {
    ShowLoader();
    var rank = $("#Rank_" + CompetitionRankId).val();
    if (rank != "" && rank != null) {
        var postCall = $.post(commonData.CompetitionRankRegister + "UpdateCompetitionRankDetail", { "CompetitionId": CompetitionId, "CompetitionRankId": CompetitionRankId, "StudentId": StudentId, "Rank": rank });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
      
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