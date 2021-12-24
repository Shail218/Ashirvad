/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    if ($("#LibraryID").val() > 0) {
        $("#fuThumbnailImage").addClass("editForm");
        $("#fuDocument").addClass("editForm");
    }

    if ($("#BranchID").val() != "") {
        if ($("#BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchID").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchID").val(null);
        }
    } else {
        $("#BranchID").val(0);
    }

    LoadCategory(function () {
        if ($("#CategoryInfo_CategoryID").val() != "") {
            $('#CategoryName option[value="' + $("#CategoryInfo_CategoryID").val() + '"]').attr("selected", "selected");
        }
    });

    if ($("#CategoryInfo_CategoryID").val() != "") {
        $('#CategoryName option[value="' + $("#CategoryInfo_CategoryID").val() + '"]').attr("selected", "selected");
    }

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


    if ($("#Type").val() != "") {
        if ($("#Type").val() == "1") {
            $("#rowGeneral").attr('checked', 'checked');
            $("#standard").hide();
            $("#subject").hide();
            $("#SubjectName").addClass("editForm");
            $("#StandardName").addClass("editForm");
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
        $("#SubjectName").addClass("editForm");
        $("#StandardName").addClass("editForm");
        $("#Type").val(1);
    }


});

function LoadSubject(onLoaded) {
    var postCall = $.post(commonData.Subject + "SubjectDataByBranchLibrary");
    postCall.done(function (data) {
        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#SubjectName").append("<option value=" + data[i].SubjectID + ">" + data[i].Subject + "</option>");
        }
        if ($("#subject_Subject").val() != "") {
            var text1 = $("#subject_Subject").val();
            $("#SubjectName option").filter(function () {
                return this.text == text1;
            }).attr('selected', true);
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadStandard(onLoaded) {
    var postCall = $.post(commonData.Standard + "StandardData", { "branchID": 0 });
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        for (i = 0; i < data.length; i++) {
            $("#StandardName").append("<option value=" + data[i].StandardID + ">" + data[i].Standard + "</option>");
        }
        if ($("#LibraryID").val() > 0) {
            SetData();
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadCategory() {
    var postCall = $.post(commonData.Category + "CategoryData");
    postCall.done(function (data) {
        $('#CategoryName').empty();
        $('#CategoryName').select2();
        $("#CategoryName").append("<option value=" + 0 + ">---Select Category---</option>");
        for (i = 0; i < data.length; i++) {
            $("#CategoryName").append("<option value=" + data[i].CategoryID + ">" + data[i].Category + "</option>");
        }

        if ($("#CategoryInfo_CategoryID").val() != "") {

            $('#CategoryName option[value="' + $("#CategoryInfo_CategoryID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function SaveLibrary() {
    var Isvalidate = true;
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        if ($("#Type") == 2) {
            Isvalidate = CustomValidation('dInformation');
        }
        if (Isvalidate) {
            ShowLoader();
            var frm = $('#fLibraryDetail');
            var formData = new FormData(frm[0]);
            var item = $('input[type=file]');
            if (item[0].files.length > 0) {
                formData.append('ThumbImageFile', $('input[type=file]')[0].files[0]);
                formData.append('DocFile', $('input[type=file]')[1].files[0]);
            }
            AjaxCallWithFileUpload(commonData.Library + 'SaveLibrary', formData, function (data) {
                if (data) {
                    HideLoader();
                    ShowMessage("Library added Successfully.", "Success");
                    window.location.href = "LibraryMaintenance?libraryID=0&Type=2";
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
}

function SaveLibraryVideo() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var postCall = $.post(commonData.Library + 'SaveLibrary', $('#fLibraryDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Library Video added Successfully.", "Success");
                window.location.href = "LibraryMaintenance?libraryID=0&Type=1";
            } else {
                ShowMessage(data.Message, "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveLibrary(libraryID) {
    if (confirm('Are you sure want to delete this Library Details?')) {
        ShowLoader();
        var postCall = $.post(commonData.Library + "RemoveLibrary", { "libraryID": libraryID });
        postCall.done(function (data) {
            HideLoader();
            if (data) {               
                ShowMessage("Library Removed Successfully.", "Success");
                window.location.href = "LibraryMaintenance?libraryID=0&Type=2";
            }
            else {               
                ShowMessage("Library Is Already In Use.", "Error");               
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveLibraryImage(LibraryID) {
    if (confirm('Are you sure want to delete this Library Book?')) {
        ShowLoader();
        var postCall = $.post(commonData.Library + "RemoveLibrary", { "libraryID": LibraryID });
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Library Book Removed Successfully.", "Success");
                window.location.href = "LibraryMaintenance?libraryID=0&Type=2";
            }
            else {
                ShowMessage("Library Is Already In Use.", "Error");
            }
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function RemoveLibraryVideo(LibraryID) {
    if (confirm('Are you sure want to delete this Video?')) {
        ShowLoader();
        var postCall = $.post(commonData.Library + "RemoveLibrary", { "libraryID": LibraryID });
        postCall.done(function (data) {
            HideLoader();
            if (data) {
                ShowMessage("Library video Removed Successfully.", "Success");
                window.location.href = "LibraryMaintenance?libraryID=0&Type=1";
            }
            else {
                ShowMessage("Library Is Already In Use.", "Error");
            }
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
    $('#StandardID').val("");
    var std = [];
    var stdName = [];
    var std1 = $('#StandardName')[0].selectedOptions;
    $.each(std1, function (index, value) {
        var vl = value.value;
        std.push(vl);
        var v2 = value.text;
        stdName.push(v2);
    });
    $('#StandardArray').val(std)
    $('#StandardNameArray').val(stdName)
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    var DataName = $("#SubjectName option:selected").text();
    $('#SubjectID').val(Data);
    $('#subject_Subject').val(DataName);
});

$("#CategoryName").change(function () {
    var Data = $("#CategoryName option:selected").val();
    $('#CategoryInfo_CategoryID').val(Data);
});

$('input[type=radio][name=Type3]').change(function () {
    if (this.value == 'All') {
        $("#BranchID").val(0);
    } else {
        $("#BranchID").val(null);
    }
});

$('input[type=radio][name=Type1]').change(function () {
    if (this.value == 'General') {
        $("#standard").hide();
        $("#subject").hide();
        $("#SubjectName").addClass("editForm");
        $("#StandardName").addClass("editForm");
        $("#Type").val(1);
    }
    else {
        $("#standard").show();
        $("#subject").show();
        $("#Type").val(2);
    }
});

function SetData() {
    var std = [];
    var StandardList = $.parseJSON($("#JsonList").val());
   
    for (var item of StandardList) {
        var Standard = item.Standard;
        $("#StandardName option").filter(function () {
            return this.text == Standard;
        }).attr('selected', true);
        std.push(Standard);
    };
    var Array = $("#StandardNameArray").val(std);
}

function CustomValidation(divName) {

    var isSuccess = true;
    $('#' + divName + ' .requiredStd').each(function () {
        var test = $(this).val();
        if ($(this).val() == '') {
            ShowMessage('Please Enter ' + $(this).attr('alt'), "Error");
            //alert();
            $(this).focus();
            isSuccess = false;
            return false;
        }
    });

    if (isSuccess) {
        $('#' + divName + ' .requiredSub').each(function () {
            var test = $(this).val();
            if ($(this).val() == '') {
                ShowMessage('Please Enter ' + $(this).attr('alt'), "Error");
                //alert();
                $(this).focus();
                isSuccess = false;
                return false;
            }
        });

    }



    return isSuccess;

}