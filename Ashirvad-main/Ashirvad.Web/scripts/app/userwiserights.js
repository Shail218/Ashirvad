/// <reference path="common.js" />
/// <reference path="../ashirvad.js" />


$(document).ready(function () {
    ShowLoader();

    var studenttbl = $("#userrightstable").DataTable({
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
            url: GetSiteURL() + "/UserRights/CustomServerSideSearchAction",
            type: 'POST',
            dataFilter: function (data) {
                HideLoader();
                return data;
            }.bind(this)
        },
        columns: [
            {
                "Page": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "userinfo.StaffDetail.Name" },
            { "data": "Roleinfo.RoleName" },
            { "data": "UserWiseRightsID" },
            { "data": "UserWiseRightsID" }
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
                targets: 3,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a href="UserRightMaintenance?UserRightID=' + data + '"><img src = "../ThemeData/images/viewIcon.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            },
            {
                targets: 4,
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data =
                            '<a onclick = "RemoveUserRight(' + data + ')"><img src = "../ThemeData/images/delete.png" /></a >'
                    }
                    return data;
                },
                orderable: false,
                searchable: false
            }
        ]
    });

    LoadUser();
    LoadRole();

    var Id = $("#UserWiseRightsID").val();
    if (Id > 0) {
        update();
    }
});

function format(d) {
    var tabledata = tabletd(d);
    return `<div style = "display:none">
                            <div style="max-height: 200px; overflow-y: scroll !important"><table style="width: 100%;" id="subcategorytbl2" class="table table-bordered dataTable no-footer">
                                <thead>
                                    <tr style="background-color:#005cbf;font-style:inherit;color:aliceblue">
                                            <th>
                                                Page
                                            </th>
                                            <th>
                                                Create Status
                                            </th>

                                            <th>
                                                Delete Status
                                            </th>
                                            <th>
                                                View Status
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
        var PageName = d[i].PageInfo.Page;
        var IsCreate = d[i].Createstatus;
        var IsDelete = d[i].Deletestatus;
        var IsView = d[i].Viewstatus;
        data = data +
            `<tr>
             <td>
             `+ PageName + `
             </td>
             <td>
            `+ IsCreate + `
            </td>
            <td>
                ` + IsDelete + `
            </td>
            <td>
                ` + IsView + `
            </td>
            </tr>`;
    }
    return data;
}

function LoadUser() {
    var postCall = $.post(commonData.User + "GetAllStaffUserbyBranch");
    postCall.done(function (data) {

        $('#UserName').empty();
        $('#UserName').select2();
        $("#UserName").append("<option value=" + 0 + ">---Select User---</option>");
        for (i = 0; i < data.length; i++) {
            $("#UserName").append("<option value=" + data[i].UserID + ">" + data[i].StaffDetail.Name + "</option>");
        }
        var t = $("#userinfo_UserID").val();
        if ($("#userinfo_UserID").val() != "") {
            $('#UserName option[value="' + $("#userinfo_UserID").val() + '"]').attr("selected", "selected");
        }

    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

function LoadRole() {
    var postCall = $.post(commonData.Role + "RoleDataByBranch", { "branchID": 0 });
    postCall.done(function (data) {

        $('#RoleName').empty();
        $('#RoleName').select2();
        $("#RoleName").append("<option value=" + 0 + ">---Select Role Name---</option>");
        for (i = 0; i < data.length; i++) {
            $("#RoleName").append("<option value=" + data[i].RoleID + ">" + data[i].RoleName + "</option>");
        }
        var t = $("#Roleinfo_RoleID").val();
        if ($("#Roleinfo_RoleID").val() != "") {
            $('#RoleName option[value="' + $("#Roleinfo_RoleID").val() + '"]').attr("selected", "selected");
        }
        HideLoader();
    }).fail(function () {
        ShowMessage("An unexpected error occcurred while processing request!", "Error");
    });
}

$("#UserName").change(function () {

    var Data = $("#UserName option:selected").val();
    $('#userinfo_UserID').val(Data);
    //LoadRole(Data);
});

$("#RoleName").change(function () {
    var Data = $("#RoleName option:selected").val();
    $('#Roleinfo_RoleID').val(Data);

    var Data = $("#RoleName option:selected").val();
    if (Data > 0) {
        ShowLoader();
        var postCall = $.post(commonData.UserRights + "UserRightUniqueData", { "RoleRightID": Data });
        postCall.done(function (data) {
            HideLoader();
            $("#rightsdiv").html(data);
            var test = $('#Roleinfo_RoleID').val();
            checkstatus();

        }).fail(function () {
            HideLoader();
        });
    }
    else {
        $("#UserDetails").html("");

    }
});

function update() {
    var Data = $("#RoleName option:selected").val();
    if (Data > 0) {
        ShowLoader();
        var postCall = $.post(commonData.UserRights + "UserRightUniqueData", { "RoleRightID": Data });
        postCall.done(function (data) {
            HideLoader();
            $("#rightsdiv").html(data);
            var test = $('#Roleinfo_RoleID').val();
            checkstatus();

        }).fail(function () {
            HideLoader();
        });
    }
    else {
        $("#UserDetails").html("");

    }
}
function checkstatus() {
    var Create = true;
    var Delete = true;
    var View = true;

    $('#choiceList .createStatus').each(function () {
        if ($(this)[0].checked == false) {
            Create = false;
        }
    });

    $('#choiceList .deletestatus').each(function () {
        if ($(this)[0].checked == false) {
            Delete = false;
        }
    });
    $('#choiceList .viewstatus').each(function () {
        if ($(this)[0].checked == false) {
            View = false;
        }
    });

    $('#choiceList thead').each(function () {

        if (Create == true) {
            $(this).find("#allcreate").checked = true;
        }

        if (Delete == true) {
            $(this).find("#alldelete").checked = true;
        } if (View == true) {
            $(this).find("#allview").checked = true;
        }
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

function GetData() {
    var CreateArray = [];
    var DeleteArray = [];
    var ViewArray = [];
    var PageArray = [];
    var RoleIDArray = [];
    var MainArray = [];

    $('#choiceList .createStatus').each(function () {
        var Create = $(this)[0].checked;
        CreateArray.push(Create);
    });

    $('#choiceList .deletestatus').each(function () {
        var Delete = $(this)[0].checked;
        DeleteArray.push(Delete);
    });
    $('#choiceList .viewstatus').each(function () {
        var View = $(this)[0].checked;
        ViewArray.push(View);
    });

    $('#choiceList .pagename').each(function () {
        var Page = $(this).val();
        PageArray.push(Page);
    });

    $('#choiceList .RoleRightsIdlist').each(function () {
        var Role = $(this).val();
        RoleIDArray.push(Role);
    });
    for (var i = 0; i < PageArray.length; i++) {
        var Page = PageArray[i];
        var Create = CreateArray[i];
        var Delete = DeleteArray[i];
        var View = ViewArray[i];
        var Role = RoleIDArray[i];
        MainArray.push({
            "PageInfo": { "PageID": Page },
            "Createstatus": Create,
            "Deletestatus": Delete,
            "Viewstatus": View,
            "RoleRightsId": Role

        })
    }
    return MainArray;
}

function SaveUserWiseRight() {

    var Array = [];
    var isSuccess = ValidateData('drights');
    if (isSuccess) {
        ShowLoader();
        Array = GetData();
        var test = $("#JasonData").val(JSON.stringify(Array))
        var postCall = $.post(commonData.UserRights + "SaveUserRight", $('#fRoleRightDetail').serialize());
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, 'Success');
                setTimeout(function () { window.location.href = "UserRightMaintenance?UserRightID=0"; }, 2000);

            }
            else {
                ShowMessage(data.Message, 'Error');
            }

        }).fail(function () {
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });

    }
}

function RemoveUserRight(UserRightID) {
    if (confirm('Are you sure want to delete this?')) {
        ShowLoader();
        var postCall = $.post(commonData.UserRights + "RemoveUserRight", { "UserRightID": UserRightID });
        postCall.done(function (data) {
            HideLoader();
            if (data.Status) {
                ShowMessage(data.Message, "Success");
                window.location.href = "UserRightMaintenance?UserRightID=0";
            } else {
                ShowMessage(data.Message, "Error");
            }

        }).fail(function () {
            HideLoader();
            ShowMessage("An unexpected error occcurred while processing request!", "Error");
        });
    }
}

