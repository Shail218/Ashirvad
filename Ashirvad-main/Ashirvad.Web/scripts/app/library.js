/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {

    if ($("#LibraryID").val() > 0 && $("#LibraryData_UniqueID").val() > 0) {
        $("#fuThumbnailImage").addClass("editForm");
        $("#fuDocument").addClass("editForm");
    }

    LoadBranch(function () {
        if ($("#BranchID").val() != "") {
            if ($("#BranchID").val() != "0") {
                $("#rowStaBranch").attr('checked', 'checked');
                $("#BranchDiv").show();
                $('#BranchName option[value="' + $("#BranchID").val() + '"]').attr("selected", "selected");
            } else {
                $("#rowStaAll").attr('checked', 'checked');
                $("#BranchDiv").hide();
                $('#BranchName option[value="' + $("#BranchID").val() + '"]').attr("selected", "selected");
                $("#BranchID").val(0);
            }
        } else {
            $("#BranchDiv").hide();
        }
    });

    LoadStandard(function () {
        if ($("#StandardID").val() != "") {
            $('#StandardName option[value="' + $("#StandardID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#StandardID").val() != "") {
        $('#StandardName option[value="' + $("#StandardID").val() + '"]').attr("selected", "selected");
    }

    LoadSubject(function () {
        if ($("#SubjectID").val() != "") {
            $('#SubjectName option[value="' + $("#SubjectID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#SubjectID").val() != "") {
        $('#SubjectName option[value="' + $("#SubjectID").val() + '"]').attr("selected", "selected");
    }

    if ($("#BranchID").val() != "") {
        if ($("#BranchID").val() != "0") {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchDiv").show();
            $('#BranchName option[value="' + $("#BranchID").val() + '"]').attr("selected", "selected");
        } else {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchDiv").hide();
            $('#BranchName option[value="' + $("#BranchID").val() + '"]').attr("selected", "selected");
            $("#BranchID").val(0);
        }
    } else {
        $("#BranchDiv").hide();
        $("#BranchID").val(0);
    }

    if ($("#Type").val() != "") {
        if ($("#Type").val() == "1") {
            $("#rowGeneral").attr('checked', 'checked');
            $("#standard").hide();
            $("#subject").hide();
            $("#Type").val(1);
        } else {
            $("#rowstandard").attr('checked', 'checked');
            $("#standard").show();
            $("#subject").show();
            $('#StandardName option[value="' + $("#StandardID").val() + '"]').attr("selected", "selected");
            $('#SubjectName option[value="' + $("#SubjectID").val() + '"]').attr("selected", "selected");
            $("#Type").val(2);
        }
    } else {
        $("#standard").hide();
        $("#subject").hide();
        $("#Type").val(1);
    }

    $('input[type=radio][name=Type3]').change(function () {
        if (this.value == 'Branch') {
            $('#BranchName option[value="0"]').attr("selected", "selected");
            $("#BranchDiv").show();
        }
        else {
            $("#BranchDiv").hide();
        }
    });

    $('input[type=radio][name=Type1]').change(function () {
        if (this.value == 'General') {
            $("#standard").hide();
            $("#subject").hide();
            $("#Type").val(1);
        }
        else {
            $("#standard").show();
            $("#subject").show();
            $("#Type").val(2);
        }
    });

    $("#studenttbl tr").each(function () {
        var elemImg = $(this).find("#thumnailImg");
        var LibraryID = $(this).find("#item_LibraryID").val();
        if (elemImg.length > 0 && LibraryID.length > 0) {
            var postCall = $.post(commonData.Library + "GetLibraryThumbnail", { "libraryID": LibraryID });
            postCall.done(function (data) {
                $(elemImg).attr('src', data);
            }).fail(function () {
                $(elemImg).attr('src', "../ThemeData/images/Default.png");
            });
        }
    });

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

function LoadSubject(onLoaded) {
    var postCall = $.post(commonData.Subject + "SubjectData");
    postCall.done(function (data) {
        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SubjectName").append("<option value=" + data[i].SubjectID + ">" + data[i].Subject + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStandard(onLoaded) {
    var postCall = $.post(commonData.Standard + "AllStandardData");
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if (onLoaded != undefined) {
            onLoaded();
        }
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function SaveLibrary() {   
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fLibraryDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('LibraryData.ThumbImageFile', $('input[type=file]')[0].files[0]);
            formData.append('LibraryData.DocFile', $('input[type=file]')[1].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Library + 'SaveLibrary', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage("Library added Successfully.", "Success");
                window.location.href = "LibraryMaintenance?libraryID=0";
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

function RemoveLibrary(libraryID) {
    if (confirm('Are you sure want to delete this Library Details?')) {
        ShowLoader();
        var postCall = $.post(commonData.Library + "RemoveLibrary", { "libraryID": libraryID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Library Removed Successfully.", "Success");
            window.location.href = "LibraryMaintenance?libraryID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

$("#BranchName").change(function () {
    
    var Data = $("#BranchName option:selected").val();
    $('#BranchID').val(Data);
});

$("#StandardName").change(function () {
    
    var Data = $("#StandardName option:selected").val();
    $('#StandardID').val(Data);
});

$("#SubjectName").change(function () {
    
    var Data = $("#SubjectName option:selected").val();
    $('#SubjectID').val(Data);
});