/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />



function DownloadAnsdetail(CompetitionId, StudentID, CompetitionDate, StudentName, ClassName) {
    ShowLoader();
    var postCall = $.post(commonData.CompetitionAnswerSheet + "SaveZipFile", { "competitionId": CompetitionId, "StudentID": StudentID, "Competition": CompetitionDate, "Student": StudentName, "Class": ClassName });
    postCall.done(function (data) {
        HideLoader();
        if (data != null && data != "") {
            var a = document.createElement("a"); //Create <a>
            a.href = data //Image Base64 Goes here         
            a.click(); //Downloaded file
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function UpdateCompetition(competitionId, StudentID) {
    ShowLoader();

    var Postfix = "" + competitionId + StudentID + "";
    var Remark = $("#Remark_" + Postfix).val();

    var postCall = $.post(commonData.CompetitionAnswerSheet + "UpdateCompetitionAnswerSheetRemarks", { "competitionId": competitionId, "StudentID": StudentID, "Remarks": Remark });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "CompetitionAnsSheetMaintenance?competitonID=" + CompetitionId + "" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
}

function GetData() {
    $('#votetble tbody tr').each(function () {
        var row = $(this);
        var Choice = $(this).find('#Choice').val();
        var ChoiceID = $(this).find('#ChoiceID').val();
        var ID = 0;

        if (Choice !== null && Choice !== "") {
            if (ChoiceID !== null && ChoiceID !== "") {
                ID = ChoiceID;
            }

            var alldata = {
                'ID': ID,
                'Choice': Choice,


            };
            data.push(alldata);
        }

    });
}
