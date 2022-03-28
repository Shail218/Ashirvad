/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

function Savetodo(myModal) {


    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.ToDoRegister + 'SaveToDo', $('#fToDoRegisterDetail').serialize())
        postCall.done(function (data) {
            if (data.Status) {
                HideLoader();
                document.getElementById("" + myModal).style.display = "none";
                ShowMessage(data.Message, 'Success');
                window.location.href = "ToDoRegisterMaintenance?todoID=0";
            } else {
                ShowMessage(data.Message, "Error");
            }                    
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