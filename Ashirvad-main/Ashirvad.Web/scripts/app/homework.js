/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();
    if ($("#HomeworkID").val() > 0) {
        $("#fuHomeworkDoc").addClass("editForm");
    }


    $("#datepickerhomework").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',

    });

    LoadBranch(function () {
        if ($("#BranchInfo_BranchID").val() != "") {
            $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
            LoadSubject($("#BranchInfo_BranchID").val());
            LoadStandard($("#BranchInfo_BranchID").val());
        }

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#BranchInfo_BranchID").val(commonData.BranchID);
            LoadSubject(commonData.BranchID);
            LoadStandard(commonData.BranchID);
        }
    });

    if ($("#BranchInfo_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#BranchInfo_BranchID").val() + '"]').attr("selected", "selected");
        LoadSubject($("#Branch_BranchID").val());
        LoadStandard($("#Branch_BranchID").val());
    }

    if ($("#BatchTimeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTimeID").val() + '"]').attr("selected", "selected");
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
    
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": branchID });
    postCall.done(function (data) {
        
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }

        if ($("#StandardInfo_StandardID").val() != "") {
            $('#StandardName option[value="' + $("#StandardInfo_StandardID").val() + '"]').attr("selected", "selected");
        }
    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
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

        if ($("#SubjectInfo_SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#SubjectInfo_SubjectID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        //ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function SaveHomework() {    
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#HomeworkDate").val();
        $("#HomeworkDate").val(ConvertData(date1));
        var frm = $('#fHomeworkDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Homework + 'SaveHomework', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage("Homework added Successfully.", "Success");
                window.location.href = "HomeworkMaintenance?homeworkID=0&branchID=0";
            } else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    }
}

function RemoveHomework(homeworkID) {
    if (confirm('Are you sure want to delete this Homework?')) {
        ShowLoader();
        var postCall = $.post(commonData.Homework + "RemoveHomework", { "homeworkID": homeworkID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Homework Removed Successfully.", "Success");
            window.location.href = "HomeworkMaintenance?homeworkID=0&branchID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function DownloadHomework(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.Homework + "Downloadhomework", { "homeworkid": branchID });
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
    $('#BranchInfo_BranchID').val(Data);
    LoadSubject(Data);
    LoadStandard(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#StandardInfo_StandardID').val(Data);
});

$("#BatchTime").change(function () {
    var value = $("#BatchTime option:selected").val();
    var text = $("#BatchTime option:selected").text();
    $('#BatchTimeID').val(value);
    $('#BatchTimeText').val(text);
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    $('#SubjectInfo_SubjectID').val(Data);
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

