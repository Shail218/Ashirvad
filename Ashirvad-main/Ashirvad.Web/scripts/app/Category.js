function SaveCategory() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Category + "SaveCategory", $('#fCategory').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "CategoryMaintenance?CategoryID=0" }, 2000);
            } else {
                ShowMessage(data.Message, "Error");
            }        
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

