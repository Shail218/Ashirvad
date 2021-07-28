/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }
});

function Savetodo() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fToDoDetail');
        var formData = new FormData(frm[0]);
        formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        AjaxCallWithFileUpload(commonData.ToDo + 'SaveToDo', formData, function (data) {

            if (data) {
                HideLoader();
                ShowMessage('ToDo details saved!', 'Success');
                window.location.href = "ToDoMaintenance?todoID=0";
            }
            else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function EditToDo(todoID) {
    var postCall = $.post(commonData.ToDoRegister + "ToDoEdit", { "todoID": todoID });
    postCall.done(function (data) {
        $('#viewpreventive').html(data);
        $('#EmpTitle').val("View To-Do Register");
        $('#myModal').removeClass("displayNone");
        document.getElementById("myModal").style.display = "block"
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$('input[type=radio][name=Status]').change(function () {
    if (this.value == 'Active') {
        $("#RowStatus_RowStatusId").val(1);
    }
    else {
        $("#RowStatus_RowStatusId").val(2);
    }
});



function onclosemodel(myModal) {

    document.getElementById("" + myModal).style.display = "none";
}