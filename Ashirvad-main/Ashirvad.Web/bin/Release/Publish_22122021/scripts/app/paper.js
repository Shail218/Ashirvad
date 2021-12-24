/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    if ($("#PaperID").val() > 0) {
        $("#fuPaperImage").addClass("editForm");
    }

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            LoadSubject($("#Branch_BranchID").val());
            LoadStandard($("#Branch_BranchID").val());
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadSubject(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadSubject($("#Branch_BranchID").val());
        LoadStandard($("#Branch_BranchID").val());
    }

    if ($("#BatchTypeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTypeID").val() + '"]').attr("selected", "selected");
    }

});

function LoadBranch(onLoaded) {
    var postCall = $.post(commonData.Branch + "BranchData");
    postCall.done(function (data) {
        
        $('#BranchName').empty();
        $('#BranchName').select2();
        $("#BranchName").append("<option value=" + 0 + ">---Select Branch---</option>");
        for (i = 0; i < data.length; i++) {
            $("#BranchName").append("<option value=" + data[i].BranchID + ">" + data[i].BranchName + "</option>");
        }

        //$.each(data, function (i) {
        //    $("#BranchName").append($("<option></option>").val(data[i].BranchID).html(data[i].BranchName));
        //});

        if (onLoaded != undefined) {
            onLoaded();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadSubject(branchID) {
    var postCall = $.post(commonData.Subject + "SubjectDataByBranch", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SubjectName").append("<option value=" + data[i].SubjectID + ">" + data[i].Subject + "</option>");
        }
        if ($("#Subject_SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#Subject_SubjectID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStandard(branchID) {
    
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if ($("#Standard_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#Standard_StandardID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function SavePaper() {
    ShowLoader();
    var isSuccess = ValidateData('dInformation');
   

    if (isSuccess) {
        var frm = $('#fPaperDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('PaperData.PaperFile', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Paper + 'SavePaper', formData, function (data) {
           
            if (data) {
                HideLoader();
                ShowMessage("Paper added Successfully.", "Success");
                window.location.href = "PaperMaintenance?paperID=0";
            }
            else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
           
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemovePaper(paperID) {
    if (confirm('Are you sure want to delete this Practice Paper?')) {
        ShowLoader();
        var postCall = $.post(commonData.Paper + "RemovePaper", { "paperID": paperID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Paper Removed Successfully.", "Success");
            window.location.href = "PaperMaintenance?paperID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function DownloadPaper(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.Paper + "Downloadpaper", { "paperid": branchID });
    postCall.done(function (data) {
        HideLoader();
        if (data != null) {
            var a = document.createElement("a"); //Create <a>
            a.href = "data:" + data[3] + ";base64," + data[1]; //Image Base64 Goes here
            a.download = data[2];//File name Here
            a.click(); //Downloaded file
        }
    }).fail(function () {
        HideLoader();
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {

    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadSubject(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#Standard_StandardID').val(Data);
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    $('#Subject_SubjectID').val(Data);
});

$("#BatchTime").change(function () {
    var Data = $("#BatchTime option:selected").val();
    $('#BatchTypeID').val(Data);
});