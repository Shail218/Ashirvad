/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function SaveToDo() {
    debugger;
    var isSuccess = ValidateData('dInformation');

    if (isSuccess) {
        debugger;
        var frm = $('#fToDoDetail');
        var formData = new FormData(frm[0]);
        formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        AjaxCallWithFileUpload(commonData.ToDo + 'SaveToDo', formData, function (data) {
            debugger;
            if (data) {
                ShowMessage('ToDo details saved!', 'Success');
                window.location.href = "ToDoMaintenance?todoID=0";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {

        });
    }
}