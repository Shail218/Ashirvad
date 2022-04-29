/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    //ShowLoader();

    $("#datepickertest").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
    });

    if ($("#DocType").val() != "") {
        var Data = $("#DocType").val();
        if (Data == "False") {
            $('#link').show();
            $('#testpaper').hide();
        } else if (Data == "True") {
            $('#testpaper').show();
            $('#link').hide();
        } else {
            $('#testpaper').hide();
            $('#link').hide();
        }
        $('#Type option[value="' + $("#DocType").val() + '"]').attr("selected", "selected");
    }
    if ($("#RowStatus_RowStatusId").val() != "") {
        var Data = $("#RowStatus_RowStatusId").val();
        $('#Status option[value="' + $("#RowStatus_RowStatusId").val() + '"]').attr("selected", "selected");
    }

});

function RemoveCompetition(competitonID) {
    debugger;
    if (confirm('Are you sure want to delete this Competition?')) {
        ShowLoader();
        var postCall = $.post(commonData.Competition + "RemoveCompetition", { "competitonID": competitonID });
        postCall.done(function (data) {
            if (data.Status) {
                HideLoader();
                ShowMessage(data.Message, "Success");
                window.location.href = "CompetitionMaintenance?competitonID=0";
            } else {
                HideLoader();
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function SaveCompetition() {
    var isSuccess = ValidateData('dInformation');
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#CompetitionDt").val();
        $("#CompetitionDt").val(ConvertData(date1));
        var frm = $('#fCompetitionDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.Competition + 'SaveCompetition', formData, function (data) {
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                setTimeout(function () { window.location.href = "CompetitionMaintenance?competitonID=0" }, 2000);
            } else {
                ShowMessage(data.Message, 'Error');
            }
            HideLoader();
        }, function (xhr) {
            HideLoader();
        });
    }
}

$("#Type").change(function () {
    var Data = $("#Type option:selected").val();
    if (Data == 2) {
        $('#link').show();
        $('#fuPaperDoc').removeClass("fileRequired");
        $('#testpaper').hide();
    } else if (Data == 1) {
        $('#testpaper').show();
        $('#fuPaperDoc').addClass("fileRequired");
        $('#link').hide();
    } else {
        $('#testpaper').hide();
        $('#link').hide();
        $('#test_fuPaperDoc').removeClass("fileRequired");
    }
    $('#DocType').val(Data);
});

$("#Status").change(function () {
    var Data = $("#Status option:selected").val();
    $('#RowStatus_RowStatusId').val(Data);
});