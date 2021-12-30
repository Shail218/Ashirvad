﻿/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    if ($("#TestID").val() > 0 && $("#test_TestPaperID").val() > 0) {
        $("#FileInfo").addClass("editForm");
    }

    var table = $('#testpapertable').DataTable({
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true,
        "proccessing": true,
        "sLoadingRecords": "Loading...",
        "sProcessing": true,
        "serverSide": true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            url: "" + GetSiteURL() + "/TestPaper/CustomServerSideSearchAction",
            type: 'POST'
        },
        "columns": [
            { "data": "Standard.Standard" },
            { "data": "BatchTimeText" },
            { "data": "TestDate" },
            { "data": "TestStartTime" },
            { "data": "Subject.Subject" },
            { "data": "Marks" },
            { "data": "test.FilePath" },
            { "data": "TestID" },
            { "data": "TestID" },
            { "data": "TestID" },
        ],
        "columnDefs": [
            {
                targets: 2,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = ConvertMiliDateFrom(data)
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 6,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        if (data.replace("http://highpack-001-site12.dtempurl.com", "") != null && data.replace("http://highpack-001-site12.dtempurl.com", "") != "") {
                            '<a href="' + data.replace("http://highpack-001-site12.dtempurl.com", "") + '" id="paperdownload" download="' + full.test.FileName + '"><img src="~/ThemeData/images/icons8-desktop-download-24 (1).png" /></a>'
                        } else {
                            '<a href="' + full.test.DocLink + '" target="_blank" style="color:blue;text-decoration:underline;">Go to link</a>'
                        }
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 7,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a href="TestPaperMaintenance?testID=' + data + '&paperID=' + full.test.TestPaperID + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 8,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a href="StudentAnswerSheetMaintenance?testID=' + data + '"><img src = "../ThemeData/images/tick.png" style="height:35px;width:35px;margin-left:20px"/></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 9,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a onclick = "RemoveTest(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });

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

        if (commonData.BranchID != "0") {
            $('#BranchName option[value="' + commonData.BranchID + '"]').attr("selected", "selected");
            $("#Branch_BranchID").val(commonData.BranchID);
            LoadStandard(commonData.BranchID);
            LoadSubject(commonData.BranchID);
        }
    });

    if ($("#Branch_BranchID").val() != "") {
        $('#BranchName option[value="' + $("#Branch_BranchID").val() + '"]').attr("selected", "selected");
        LoadStandard($("#Branch_BranchID").val());
        LoadSubject($("#Branch_BranchID").val());
        HideLoader();
    }

    if ($("#BatchTimeID").val() != "") {
        $('#BatchTime option[value="' + $("#BatchTimeID").val() + '"]').attr("selected", "selected");
    }

    if ($("#test_PaperTypeID").val() != "") {
        var Data = $("#test_PaperTypeID").val();
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
        $('#Type option[value="' + $("#test_PaperTypeID").val() + '"]').attr("selected", "selected");
    }
    if ($("#RowStatus_RowStatusId").val() != "") {
        var Data = $("#RowStatus_RowStatusId").val();     
        $('#Status option[value="' + $("#RowStatus_RowStatusId").val() + '"]').attr("selected", "selected");
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
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function Savetest() {
    var isSuccess = ValidateData('dInformation');    
    if (isSuccess) {
        ShowLoader();
        var date1 = $("#TestDate").val();
        $("#TestDate").val(ConvertData(date1));
        var frm = $('#fTestDetail');
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.TestPaper + 'SaveTest', formData, function (data) {
            if (data) {
                ShowMessage("Test paper added Successfully.", "Success");
                setTimeout(function () { window.location.href = "TestPaperMaintenance?testID=0" }, 2000);
            } else {
                ShowMessage('Test Already Exists!!', 'Error');
            }
            HideLoader();
        }, function (xhr) {
            HideLoader();
        });
    }
}

function Savetestpaper(testID,date) {
    var isSuccess = ValidateData('dpaperInformation');
    if (isSuccess) {
        ShowLoader();
        var frm = $('#fTestPaperDetail');
        TestID: $("#test_id").val(testID);
        var sd = date.split("/Date(");
        var sd2 = sd[1].split(")/");
        var date1 = new Date(sd2[0]);
        TestDate: $("#test_date").val(date1)
        var formData = new FormData(frm[0]);
        var item = $('input[type=file]');
        if (item[0].files.length > 0) {
            formData.append('FileInfo', $('input[type=file]')[0].files[0]);
        }
        AjaxCallWithFileUpload(commonData.TestPaper + 'SaveTestPaper', formData, function (data) {
            if (data) {
                HideLoader();
                ShowMessage("Test paper added Successfully.", "Success");
                window.location.href = "TestPaperMaintenance?testID=0";
            } else {
                HideLoader();
                ShowMessage('An unexpected error occcurred while processing request!', 'Error');
            }
        }, function (xhr) {
            HideLoader();
        });
    } else {
        HideLoader();
    }
}

function RemoveTest(testID) {
    if (confirm('Are you sure want to delete this Test Paper?')) {
        ShowLoader();
        var postCall = $.post(commonData.TestPaper + "RemoveTest", { "testID": testID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Test Removed Successfully.", "Success");
            window.location.href = "TestPaperMaintenance?testID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function DownloadTestPaper(branchID) {
    ShowLoader();
    var postCall = $.post(commonData.TestPaper + "Downloadtestpaper", { "paperid": branchID });
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
        $("#test_DocContent").val('');
        $('#link').show();
        $('#test_fuPaperDoc').removeClass("fileRequired");
        $('#testpaper').hide();
    } else if (Data == 1) {
        $("#test_DocLink").val(' ');
        $('#testpaper').show();
        $('#test_fuPaperDoc').addClass("fileRequired");
        $('#link').hide();
    } else {
        $('#testpaper').hide();
        $('#link').hide();
        $('#test_fuPaperDoc').removeClass("fileRequired");
    }
    $('#test_PaperTypeID').val(Data);
});

$("#Status").change(function () {
    var Data = $("#Status option:selected").val();
    $('#RowStatus_RowStatusId').val(Data);
   
});

function ConvertMiliDateFrom(date) {
    if (date != null) {
        var sd = date.split("/Date(");
        var sd2 = sd[1].split(")/");
        var date1 = new Date(parseInt(sd2[0]));
        var d = date1.getDate();
        var m = date1.getMonth() + 1;
        var y = date1.getFullYear();
        var hr = date1.getHours();
        var min = date1.getMinutes();
        var sec = date1.getSeconds();

        if (parseInt(d) < 10) {
            d = "0" + d;
        }
        if (parseInt(m) < 10) {
            m = "0" + m;
        }
        var Final = d + "-" + m + "-" + y + " ";
        var d = date1.toString("dd/MM/yyyy HH:mm:SS");
        return Final;
    }
    return "";
}
