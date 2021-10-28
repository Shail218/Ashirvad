/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function Savetodo(myModal) {
    ShowLoader();

    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        var postCall = $.post(commonData.ToDoRegister + 'SaveToDo', $('#fToDoRegisterDetail').serialize())
        postCall.done(function (data) {
          
            document.getElementById("" + myModal).style.display = "none";
            ShowMessage('ToDo details saved!', 'Success');
            window.location.href = "ToDoRegisterMaintenance?todoID=0";
            HideLoader();
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

function changestatus(row) {
    if (row.value == 'Active') {
        $("#Registerstatus").val(true);
    }
    else {
        $("#Registerstatus").val(false);
    }
}

function onclosemodel(myModal) {

    document.getElementById("" + myModal).style.display = "none";
}