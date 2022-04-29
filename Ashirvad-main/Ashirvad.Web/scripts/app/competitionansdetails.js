/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />



function DownloadAnsdetail(CompetitionId, StudentID) {
    //ShowLoader();
    //var postCall = $.post(commonData.TestPaper + "SaveZipFile", { "testid": HomeworkID, "StudentID": StudentID, "Test": Homework, "Student": Student, "Class": Class });
    //postCall.done(function (data) {
    //    HideLoader();
    //    if (data != null && data != "") {
    //        var a = document.createElement("a"); //Create <a>
    //        a.href = data //Image Base64 Goes here         
    //        a.click(); //Downloaded file
    //    }
    //}).fail(function () {
    //    HideLoader();
    //    ShowMessage("An unexpected error occcurred while processing request!", "Error");
    //});
}
function UpdateCompetition(HomeWorkID, StudentID) {
    //ShowLoader();

    //var Postfix = "" + HomeWorkID + StudentID + "";
    //var Remark = $("#Remark_" + Postfix).val();
    //var Done = $("#Done_" + Postfix);
    //var Pending = $("#Pending_" + Postfix);
    //var StatusValue;
    //var Status = Done.is(":checked");
    //StatusValue = parseInt(Done.val());

    //if (!Status) {
    //    Status = Pending.is(":checked");
    //    StatusValue = parseInt(Pending.val());
    //}
    //if (Status) {
    //    var postCall = $.post(commonData.TestPaper + "UpdateAnsdetails", { "TestID": HomeWorkID, "StudentID": StudentID, "Remark": Remark, "Status": StatusValue });
    //    postCall.done(function (data) {
    //        HideLoader();
    //        if (data.Status == true) {
    //            ShowMessage(data.Message, "Success");
    //            setTimeout(function () { window.location.href = "StudentAnswerSheetMaintenance?testID=" + HomeWorkID + "" }, 2000);
    //        } else {
    //            ShowMessage(data.Message, "Error");
    //        }
    //    }).fail(function () {
    //        HideLoader();
    //        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    //    });
    //}
    //else {
    //    HideLoader();
    //    ShowMessage("Please Select Status Of Homework!", "Error");
    //}
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
