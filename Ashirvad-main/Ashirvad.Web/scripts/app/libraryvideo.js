/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
    var check = GetUserRights('LibraryVideo');
    var studenttbl = $("#studenttbl").DataTable({
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
            processing: '<img ID="imgUpdateProgress" src="~/ThemeData/images/preview.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 40%;Width:200px; Height:160px" />'
        },
        "ajax": {
            url: GetSiteURL() + "/Library/CustomServerSideSearchAction2",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        columns: [
            //{ "data": "BranchCourse.course_dtl_id" },
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "BranchID" },
            { "data": "CategoryInfo.Category" },
            { "data": "BranchCourse.course.CourseName" },
            { "data": "subject.Subject" },
            { "data": "VideoLink" },           
            { "data": "Description" },
            { "data": "LibraryID" },
            { "data": "LibraryID" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        var ch = format(data.libraryEntities)
                        data = '<img src="../ThemeData/images/plus.png" height="30" />' + ch;
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 1,
                render: function (data, type, full, meta) {
                    var BranchName = $("#Lbranch").val();

                    if (type === 'display') {

                        data = (data == 0) ? "All Branch" : BranchName
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },

            {
                targets: 5,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        data = '<a href="' + data+'" target="_blank" style="color:blue;text-decoration:underline;">Go to link</a>';
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
                        if (check[0].Create) {
                            data =
                                '<a href="LibraryMaintenance?libraryID=' + data + '&Type=1"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                        else {
                            data = "";
                        }
                       
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
                        if (check[0].Delete) {
                            data =
                                `<a style="margin-left:20px;" href="#" onclick="RemoveLibraryImage(` + data + `);">
                            <img src = "../ThemeData/images/delete.png" />
                            </a >`
                        }
                        else {
                            data = "";
                        }
                      
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });

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

    LoadCourse();

    if ($("#Type").val() != "") {
        if ($("#Type").val() == "1") {
            $("#rowGeneral").attr('checked', 'checked');
            $("#standard").hide();
            $("#subject").hide();
            $("#course").hide();
            $("#CourseName").addClass("editForm");
            $("#SubjectName").addClass("editForm");
            $("#StandardName").addClass("editForm");
            $("#Type").val(1);
        } else {
            $("#rowstandard").attr('checked', 'checked');
            $("#standard").show();
            $("#subject").show();
            $("#course").show();
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
            $('#StandardName option[value="' + $("#StandardID").val() + '"]').attr("selected", "selected");
            $('#SubjectName option[value="' + $("#SubjectID").val() + '"]').attr("selected", "selected");
            $("#Type").val(2);
        }
    } else {
        $("#standard").hide();
        $("#subject").hide();
        $("#course").hide();
        $("#CourseName").addClass("editForm");
        $("#SubjectName").addClass("editForm");
        $("#StandardName").addClass("editForm");
        $("#Type").val(1);
    }

});

function LoadCourse() {
    var postCall = $.post(commonData.BranchCourse + "GetCourseDDL");
    postCall.done(function (data) {
        $('#CourseName').empty();
        $('#CourseName').select2();
        $("#CourseName").append("<option value=" + 0 + ">---Select Course---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#CourseName").append("<option value='" + data[i].course_dtl_id + "'>" + data[i].course.CourseName + "</option>");
            }
        }

        if ($("#BranchCourse_course_dtl_id").val() != "") {
            $('#CourseName option[value="' + $("#BranchCourse_course_dtl_id").val() + '"]').attr("selected", "selected");
            LoadClass($("#BranchCourse_course_dtl_id").val());
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadClass(CourseID) {
    ShowLoader();
    var postCall = $.post(commonData.BranchClass + "GetClassDDL", { "CourseID": CourseID });
    postCall.done(function (data) {
        $('#StandardName').empty();
        $('#StandardName').select2();
        $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#StandardName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
            }
        }
        if ($("#LibraryID").val() > 0) {
            SetData();
        }

        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#StandardName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
        }

        HideLoader();
    }).fail(function () {
        HideLoader();
    });
}

function LoadSubject(ClassID, CourseID) {
    ShowLoader();
    var postCall = $.post(commonData.BranchSubject + "GetLibrarySubjectDDL", { "std": ClassID, "courseid": CourseID });
    postCall.done(function (data) {
        $('#SubjectName').empty();
        $('#SubjectName').select2();
        $("#SubjectName").append("<option value=" + 0 + ">---Select Subject---</option>");
        if (data != null) {
            for (i = 0; i < data.length; i++) {
                $("#SubjectName").append("<option value='" + data[i].Subject_dtl_id + "'>" + data[i].Subject.SubjectName + "</option>");
            }
        }

        if ($("#BranchSubject_Subject_dtl_id").val() != "") {
            /*$('#SubjectName option[value="' + $("#BranchSubject_Subject_dtl_id").val() + '"]').attr("selected", "selected");*/
            var text1 = $("#BranchSubject_Subject_SubjectName").val();
            $("#SubjectName option").filter(function () {
                return this.text == text1;
            }).attr('selected', true);
        }
        HideLoader();
    }).fail(function () {
        HideLoader();

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

function clearsubject() {
    $('#SubjectName').empty();
    $('#SubjectName').select2();
    $("#SubjectName").append("<option value=" + 0 + ">---Select Subject---</option>");
}

function clearclass() {
    $('#StandardName').empty();
    $('#StandardName').select2();
    $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    clearclass();
    clearsubject();
    $("#standard").show();
    $("#subject").show();
    LoadClass(Data);
});

$("#BranchName").change(function () {
    var Data = $("#BranchName option:selected").val();
    $('#BranchID').val(Data);
});

$("#StandardName").change(function () {
    var Data = $("#StandardName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
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
    LoadSubject($('#StandardArray').val(), $('#BranchCourse_course_dtl_id').val());
});

$("#SubjectName").change(function () {
    var Data = $("#SubjectName option:selected").val();
    var DataName = $("#SubjectName option:selected").text();
    $('#BranchSubject_Subject_dtl_id').val(Data);
    $('#BranchSubject_Subject_SubjectName').val(DataName);
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
        $("#course").hide();
        $("#CourseName").addClass("editForm");
        $("#SubjectName").addClass("editForm");
        $("#StandardName").addClass("editForm");
        $("#Type").val(1);
    }
    else {
        $("#course").show();
        $("#Type").val(2);
    }
});

function SetData() {
    var std = [];
    var id = [];
    var StandardList = $.parseJSON($("#JsonList").val());

    for (var item of StandardList) {
        var Standard = item.Standard;
        var stdid = item.StandardID;
        $("#StandardName option").filter(function () {
            return this.text == Standard;
        }).attr('selected', true);
        std.push(Standard);
        id.push(stdid);
    };
    var Array = $("#StandardNameArray").val(std);
    $('#StandardArray').val(id)
    LoadSubject($('#StandardArray').val(), $('#BranchCourse_course_dtl_id').val());
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

function format(d) {
    // `d` is the original data object for the row
    var tabledata = tabletd(d);
    return `<div style = "display:none">
                            <div style="max-height: 200px; overflow-y: scroll !important"><table style="width: 100%;" id="subcategorytbl2" class="table table-bordered dataTable no-footer">
                                <thead>
                                    <tr style="background-color:#005cbf;font-style:inherit;color:aliceblue">

                                        <th>
                                            Standard
                                        </th>
                                        

                                    </tr>
                                </thead>

                                <tbody>`+
        tabledata +
        `</tbody>
                            </table>
                            </div>

                </div> `;
}

function tabletd(d) {
   
    var data = ``;
    for (var i = 0; i < d.length; i++) {
        var ClassName = d[i].standard.Standard;
        
        data = data +
            `<tr>
             <td>
             `+ ClassName + `
             </td>
            
            </tr>`;
    }
    return data;
}