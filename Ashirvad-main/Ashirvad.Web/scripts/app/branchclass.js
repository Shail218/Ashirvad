/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />

$(document).ready(function () {
    ShowLoader();

    LoadCourse();

    var IsEdit = $("#IsEdit").val();
    if (IsEdit == "True") {
        checkstatus();
    }
   // var check = GetUserRights('BranchClassMaster');
    var studenttbl = $("#subcategorytbl").DataTable({
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
            url: GetSiteURL() + "/BranchClass/CustomServerSideSearchAction",
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
            { "data": "branch.BranchName" },
            { "data": "BranchCourse.course.CourseName" }
            //{ "data": "BranchCourse.course_dtl_id" },
            //{ "data": "BranchCourse.course_dtl_id" }
        ],
        "columnDefs": [
            {
                targets: 0,
                render: function (data, type, full, meta) {

                    if (type === 'display') {
                        var ch = format(data.BranchClassData)
                        data = '<img src="../ThemeData/images/plus.png" height="30" />' + ch;
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            //{
            //    targets: 3,
            //    render: function (data, type, full, meta) {
            //        if (type === 'display') {
            //            data =
            //                '<a href="ClassMaintenance?ClassID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
            //        }
            //        return data;
            //    },
            //    orderable: false,
            //    searchable: false
            //},
            //{
            //    targets: 4,
            //    render: function (data, type, full, meta) {
            //        if (type === 'display') {
            //            data =
            //                '<a onclick = "RemoveClass(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
            //        }
            //        return data;
            //    },
            //    orderable: false,
            //    searchable: false
            //}
        ]
    });

});

function format(d) {
    // `d` is the original data object for the row
    var tabledata = tabletd(d);
    return `<div style = "display:none">
                            <div style="max-height: 200px; overflow-y: scroll !important"><table style="width: 100%;" id="subcategorytbl2" class="table table-bordered dataTable no-footer">
                                <thead>
                                    <tr style="background-color:#005cbf;font-style:inherit;color:aliceblue">

                                        <th>
                                            Class
                                        </th>
                                        <th>
                                            Selected
                                        </th>

                                    </tr>
                                </thead>

                                <tbody>`+
    tabledata+
                                    `</tbody>
                            </table>
                            </div>

                </div> `;
}

function tabletd(d)
{
    var data = ``;
    for (var i = 0; i < d.length; i++)
    {
        var ClassName = d[i].Class.ClassName;
        var IsClass = d[i].isClass == true ? "YES" : "NO";
        data= data +
            `<tr>
             <td>
             `+ ClassName + `
             </td>
             <td>
            `+ IsClass + `
            </td>
            </tr>`;
    }
    return data;
}

$('#subcategorytbl tbody').on('click', 'td.dt-control', function () {
    var tr = $(this).closest('tr');
    var row = table.row(tr);

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        // Open this row
        row.child(format(row.data())).show();
        tr.addClass('shown');
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
        }

        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function OnSelectStatus(Data, classData) {
    if (Data.checked == true) {
        $('#choiceList .' + classData).each(function () {
            $(this)[0].checked = true;

        });
    }
    else {
        $('#choiceList .' + classData).each(function () {
            $(this)[0].checked = false;

        });
    }


}


function SaveClassDetail() {
    var Array = [];
    var isSuccess = ValidateData('fBranchClassDetail');
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JsonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.BranchClass + "SaveClassDetails", $('#fBranchClassDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "ClassMaintenance?ClassID=0"; }, 2000);

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
    var ClassStatus = [];
    var ClassData = [];
    var ClassNameData = [];
    var ClassDetailData = [];
    var MainArray = [];

    $('#choiceList .isClass').each(function () {
        var Class = $(this)[0].checked;
        ClassStatus.push(Class);
    });


    $('#choiceList .ClassID').each(function () {
        var ClassID = $(this).val();
        ClassData.push(ClassID);
    });
    $('#choiceList .ClassName').each(function () {
        var ClassName = $(this).val();
        ClassNameData.push(ClassName);
    });
    $('#choiceList .Classdtlid').each(function () {
        var Classdtlid = $(this).val();
        ClassDetailData.push(Classdtlid);
    });

    for (var i = 0; i < ClassData.length; i++) {
        var IsClass = ClassStatus[i];
        var ClassID = ClassData[i];
        var ClassDetailID = ClassDetailData[i];
        var ClassName = ClassNameData[i];

        MainArray.push({
            "Class": { "ClassID": ClassID, "ClassName": ClassName },
            "Class_dtl_id": ClassDetailID,
            "isClass": IsClass,


        })
    }
    return MainArray;
}

function checkstatus() {
    var Create = true;
    $('#choiceList .isClass').each(function () {
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
});
function RemoveClass(CourseID) {
    if (confirm('Are you sure want to delete this?')) {
        ShowLoader();
        var postCall = $.post(commonData.BranchClass + "RemoveClassDetail", { "PackageRightID": CourseID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status == true) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "ClassMaintenance?ClassID=0"; }, 2000);

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