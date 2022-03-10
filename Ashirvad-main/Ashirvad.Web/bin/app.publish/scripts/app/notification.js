/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

/*const { data } = require("jquery");*/

$(document).ready(function () {
    ShowLoader();
    var check = GetUserRights('NotificationMaster');
    var table = $('#notificationtable').DataTable({
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
            url: "" + GetSiteURL() + "/Notification/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        "data": HideLoader(),
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "BranchCourse.course.CourseName" },
            { "data": "Notification_Date" },
            { "data": "NotificationMessage" },
            { "data": "NotificationTypeText" },
            { "data": "NotificationID" },
            { "data": "NotificationID" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        var ch = format(data.list)
                        data = '<img src="../ThemeData/images/plus.png" height="30" />' + ch;
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
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
                targets: 5,
                render: function (data, type, full, meta) {
                    if (check[0].Create) {
                        if (type === 'display') {
                            data =
                                '<a href="NotificationMaintenance?notificationID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                        }
                    }
                    else {
                        data = "";
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 6,
                render: function (data, type, full, meta) {
                    if (check[0].Delete) {
                        if (type === 'display') {
                            data =
                                '<a onclick = "RemoveNotification(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                        }
                    }
                    else {
                        data = "";
                    }
                    return data;
                },                
                orderable: false,
                searchable: false                
            }            
        ]
    });

    $("#notification").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
    });

    if ($("#Branch_BranchID").val() != "") {
        if ($("#Branch_BranchID").val() == "0") {
            $("#rowStaAll").attr('checked', 'checked');
            $("#BranchType").val(0);
        } else {
            $("#rowStaBranch").attr('checked', 'checked');
            $("#BranchType").val(1);
        }
    } else {
        $("#Branch_BranchID").val(0);
    }

    if ($("#RowStatus_RowStatusId").val() != "") {
        var rowStatus = $("#RowStatus_RowStatusId").val();
        if (rowStatus == "1") {
            $("#rowStaActive").attr('checked', 'checked');
        }
        else {
            $("#rowStaInactive").attr('checked', 'checked');
        }
    }
    LoadCourse();
    if ($("#NotificationID").val() == 0) {

        $("#course").hide();
        $("#standard").hide();
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
function SaveNotification() {    
    var isSuccess = ValidateData('dInformation');
    var NotificationTypeList = [];
    if ($('input[type=checkbox][id=rowStaAdmin]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Admin",
            TypeID: 1
        });
    }
    if ($('input[type=checkbox][id=rowStaTeacher]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Teacher",
            TypeID: 2
        });
    }
    if ($('input[type=checkbox][id=rowStaStudent]').is(":checked")) {
        NotificationTypeList.push({
            TypeText: "Student",
            TypeID: 3
        });
    }
    $('#JSONData').val(JSON.stringify(NotificationTypeList));
    var date2 = $("#Notification_Date").val();
    $("#Notification_Date").val(ConvertData(date2));
    if (isSuccess) {
        var Isvalidate = true;
        var teacher = document.getElementById("rowStaTeacher");
        var student = document.getElementById("rowStaStudent");
        if (teacher.checked == true || student.checked == true) {
            Isvalidate = CustomValidation('dInformation');
        } if (Isvalidate) {
            ShowLoader();
            var postCall = $.post(commonData.Notification + "SaveNotification", $('#fNotificationDetail').serialize());
            postCall.done(function (data) {
                HideLoader();
                ShowMessage('Notification details saved!', 'Success');
                window.location.href = "NotificationMaintenance?notificationID=0";
            }).fail(function () {
                HideLoader();
                ShowMessage("An unexpected error occcurred while processing request!", "Error");
            });
        }
    }
}

function RemoveNotification(branchID) {
    if (confirm('Are you sure want to delete this Notification?')) {
        ShowLoader();
        var postCall = $.post(commonData.Notification + "RemoveNotification", { "notificationID": branchID });
        postCall.done(function (data) {
            HideLoader();
            ShowMessage("Notification Removed Successfully.", "Success");
            window.location.href = "NotificationMaintenance?notificationID=0";
        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

function checkboxclick() {
    var admin = document.getElementById("rowStaAdmin");
    var teacher =document.getElementById("rowStaTeacher");
    var student = document.getElementById("rowStaStudent");
    if (admin.checked == true) {
        $("#standard").hide();
        $("#course").hide();
        $("#CourseName").addClass("editForm");
        $("#StandardName").addClass("editForm");
    }
    if (teacher.checked == true || student.checked == true) {
        $("#standard").show();
        $("#course").show();
        $("#CourseName").addClass("editForm");
        $("#StandardName").addClass("editForm");
    } else {
        $("#standard").hide();
        $("#course").hide();
        $("#CourseName").addClass("editForm");
        $("#StandardName").addClass("editForm");
    }

}

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
    return "";;
}
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
        if ($("#NotificationID").val() > 0) {
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
$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    if (Data > 0) {
        clearclass();
        $("#standard").show();
        LoadClass(Data);
    } else {
        clearclass();
        $("#standard").hide();
    }
 
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
   
});
function clearclass() {
    $('#StandardName').empty();
    $('#StandardName').select2();
    $("#StandardName").append("<option value=" + 0 + ">---Select Standard---</option>");
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
        var ClassName = d[i].standard;

        data = data +
            `<tr>
             <td>
             `+ ClassName + `
             </td>
            
            </tr>`;
    }
    return data;
}