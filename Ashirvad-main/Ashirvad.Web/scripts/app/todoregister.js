/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function Savetodo(myModal) {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.ToDoRegister + 'SaveToDo', $('#fToDoRegisterDetail').serialize())
        postCall.done(function (data) {
            HideLoader();
            document.getElementById("" + myModal).style.display = "none";
            ShowMessage('ToDo details saved!', 'Success');
            window.location.href = "ToDoRegisterMaintenance?todoID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
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

function changestatus() {
    if (this.value == 'Active') {
        $("#RowStatus_RowStatusId").val(1);
    }
    else {
        $("#RowStatus_RowStatusId").val(2);
    }
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