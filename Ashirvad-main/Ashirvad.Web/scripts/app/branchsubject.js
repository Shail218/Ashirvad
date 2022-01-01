/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();
 
    var IsEdit = $("#IsEdit").val();
    if (IsEdit == "True") {
        checkstatus();
    }

    var studenttbl = $("#branchsubjecttable").DataTable({
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
            url: GetSiteURL() + "/BranchSubject/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        columns: [
            //{ "data": "BranchCourse.course_dtl_id" },
            {
                "SubjectName": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "branch.BranchName" },
            { "data": "BranchCourse.course.CourseName" },
            { "data": "Class.ClassName" }
            //{ "data": "BranchClass.Class_dtl_id" },
            //{ "data": "BranchCourse.course_dtl_id" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        var ch = format(data.BranchSubjectData)
                        data = '<img src="../ThemeData/images/plus.png" height="30" />' + ch;
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            //{
            //    targets: 4,
            //    render: function (data, type, full, meta) {
            //        if (type === 'display') {
            //            data =
            //                '<a href="SubjectMaintenance?SubjectID=' + data + '&CourseID=' + full.BranchCourse.course_dtl_id + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
            //        }
            //        return data;
            //    },
            //    orderable: false,
            //    searchable: false
            //},
            //{
            //    targets: 5,
            //    render: function (data, type, full, meta) {
            //        if (type === 'display') {
            //            data =
            //                '<a onclick = "RemoveSubject(' + data + ',' + full.BranchClass.Class_dtl_id + ')"><img src = "../ThemeData/images/delete.png" /></a >'
            //        }
            //        return data;
            //    },
            //    orderable: false,
            //    searchable: false
            //}
        ]
    });

    LoadCourse();
});

function format(d) {
    var tabledata = tabletd(d);
    return `<div style = "display:none">
                            <div style="max-height: 200px; overflow-y: scroll !important"><table style="width: 100%;" id="subcategorytbl2" class="table table-bordered dataTable no-footer">
                                <thead>
                                    <tr style="background-color:#005cbf;font-style:inherit;color:aliceblue">

                                    <th>
                                        Subject
                                    </th>
                                    <th>
                                        Selected
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
        var SubjectName = d[i].Subject.SubjectName;
        var IsSubject = d[i].isSubject == true ? "YES" : "NO";
        data = data +
            `<tr>
             <td>
             `+ SubjectName + `
             </td>
             <td>
            `+ IsSubject + `
            </td>
            </tr>`;
    }
    return data;
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
        }
        var IsEdit = $("#IsEdit").val();
        if (IsEdit == "True") {
            LoadClass($("#BranchCourse_course_dtl_id").val());
        }
        
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadClass(CourseID) {
    var postCall = $.post(commonData.BranchClass + "GetClassDDL", { "CourseID": CourseID });
    postCall.done(function (data) {
        $('#ClassName').empty();
        $('#ClassName').select2();
        $("#ClassName").append("<option value=" + 0 + ">---Select Class---</option>");
        if (data != 0) {
            for (i = 0; i < data.length; i++) {
                $("#ClassName").append("<option value='" + data[i].Class_dtl_id + "'>" + data[i].Class.ClassName + "</option>");
            }
        }
        if ($("#BranchClass_Class_dtl_id").val() != "") {
            $('#ClassName option[value="' + $("#BranchClass_Class_dtl_id").val() + '"]').attr("selected", "selected");
        }

        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}


function OnSelectStatus(Data, SubjectData) {
    if (Data.checked == true) {
        $('#choiceList .' + SubjectData).each(function () {
            $(this)[0].checked = true;

        });
    }
    else {
        $('#choiceList .' + SubjectData).each(function () {
            $(this)[0].checked = false;

        });
    }
}

function SaveSubjectDetail() {
    var Array = [];
    var isSuccess = ValidateData('fBranchSubjectDetail');
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JsonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.BranchSubject + "SaveSubjectDetails", $('#fBranchSubjectDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "SubjectMaintenance?SubjectID=0&&CourseID=0"; }, 2000);
            }
            else {
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function GetData() {
    var SubjectStatus = [];
    var SubjectData = [];
    var SubjecNameData = [];
    var SubjectDetailData = [];
    var MainArray = [];

    $('#choiceList .isSubject').each(function () {
        var Subject = $(this)[0].checked;
        SubjectStatus.push(Subject);
    });

    $('#choiceList .SubjectName').each(function () {
        var SubjectName = $(this).val();
        SubjecNameData.push(SubjectName);
    });
    $('#choiceList .SubjectID').each(function () {
        var SubjectID = $(this).val();
        SubjectData.push(SubjectID);
    });
    $('#choiceList .Subjectdtlid').each(function () {
        var Subjectdtlid = $(this).val();
        SubjectDetailData.push(Subjectdtlid);
    });

    for (var i = 0; i < SubjectData.length; i++) {
        var IsSubject = SubjectStatus[i];
        var SubjectID = SubjectData[i];
        var SubjectDetailID = SubjectDetailData[i];
        var SubjectName = SubjecNameData[i];

        MainArray.push({
            "Subject": { "SubjectID": SubjectID, "SubjectName": SubjectName},
            "Subject_dtl_id": SubjectDetailID,
            "isSubject": IsSubject,


        })
    }
    return MainArray;
}

function checkstatus() {
    var Create = true;
    $('#choiceList .isSubject').each(function () {
        if ($(this)[0].checked == false) {
            Create = false;
        }
    });
    if (Create) {
        $("#allselect").prop('checked', true);
    }
    else {
        $("#allselect").prop('checked', false);
    }
}

$("#CourseName").change(function () {
    var Data = $("#CourseName option:selected").val();
    $('#BranchCourse_course_dtl_id').val(Data);
    LoadClass(Data);
});

$("#ClassName").change(function () {
    var Data = $("#ClassName option:selected").val();
    $('#BranchClass_Class_dtl_id').val(Data);
    
});
function RemoveSubject(CourseID,ClassID) {
    if (confirm('Are you sure want to delete this Subjects?')) {
        ShowLoader();
        var postCall = $.post(commonData.BranchSubject + "RemoveSubjectDetail", { "CourseID": CourseID, "ClassID": ClassID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "SubjectMaintenance?SubjectID=0&&CourseID=0"; }, 2000);

            }
            else {
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}