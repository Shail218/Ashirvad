/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {

    $("#datepickertest").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    LoadBranch(function () {
        if ($("#Branch_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
            LoadStandard($("#Branch_BranchID").val());
            LoadSubject($("#Branch_BranchID").val());
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#Branch_BranchID").val());
        LoadSubject($("#Branch_BranchID").val());
    }

    if ($("#BatchTimeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTimeID").val() + '"]').attr("selected", "selected");
    }

    if ($("#PaperTypeID").val() != "") {
        var Data = $("#PaperTypeID").val();
        if (Data == 2) {
            $('#link').show();
            $('#testpaper').hide();
        } else if (Data == 1) {
            $('#testpaper').show();
            $('#link').hide();
        } else {
            $('#testpaper').hide();
            $('#link').hide();
        }
        $('#Type option[value="' + $("#PaperTypeID").val() + '"]').attr("selected", "selected");
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

function LoadStandard(branchID) {
    
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID});
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

function SaveTest() {
    
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        var date1 = $("#TestDate").val();
        $("#TestDate").val(ConvertData(date1));
        var postCall = $.post(commonData.TestPaper + "SaveTest", $('#fTestDetail').serialize());
        postCall.done(function (data) {
            
            if (data > 0) {
                SaveTestPaper(data);
            } else {
                ShowMessage("Test not added Successfully.", "Success");
                window.location.href = "TestPaperMaintenance?testID=0";
            }
        }).fail(function () {
            
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function SaveTestPaper(testID) {
    
    var isSuccess = ValidateData('dpaperInformation');
    $('#TestID').val(testID);
    if (isSuccess) {
        
        var frm = $('#fTestPaperDetail');
        var formData = new FormData(frm[0]);
        formData.append('FileInfo', $('input[type=file]')[0].files[0]);

        AjaxCallWithFileUpload(commonData.TestPaper + 'SaveTestPaper', formData, function (data) {
            
            if (data) {
                ShowMessage("Test paper added Successfully.", "Success");
                window.location.href = "TestPaperMaintenance?testID=0";
            } else {
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {

        });

    }
}

function RemoveTest(testID) {
    
    var postCall = $.post(commonData.TestPaper + "RemoveTest", { "testID": testID });
    postCall.done(function (data) {
        
        ShowMessage("Test Removed Successfully.", "Success");
        window.location.href = "TestPaperMaintenance?testID=0";
    }).fail(function () {
        
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#Branch_BranchID').val(Data);
    LoadStandard(Data);
    LoadSubject(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#Standard_StandardID').val(Data);
});

$("#BatchTime").change(function () {
    var value = $("#BatchTime option:selected").val();
    var text = $("#BatchTime option:selected").text();
    $('#BatchTimeID').val(value);
    $('#BatchTimeText').val(text);
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    $('#Subject_SubjectID').val(Data);
});

$("#Type").change(function () {
    var Data = $("#Type option:selected").val();
    if (Data == 2) {
        $('#link').show();
        $('#testpaper').hide();
    } else if (Data == 1) {
        $('#testpaper').show();
        $('#link').hide();
    } else {
        $('#testpaper').hide();
        $('#link').hide();
    }
    $('#PaperTypeID').val(Data);
});

