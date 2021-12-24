$(document).ready(function () {
   
        document.getElementById("loader").style.display = "none";
    
});
function OnSubmit() {
    
    var Email = $("#txtmob").val();
    var password = $("#txtpwd").val();
    if (Email == "") {
        SAerror("Please Enter Mobile No");
    }
    else if (password == "") {
        SAerror("Please Enter password");
    }

    else {
        

        var form = $('#FormItem');
        form[0].action = GetSiteURL() + '/Login/GetLoginDetails';
        $.validator.unobtrusive.parse(form);
        $('#FormItem').validate();
        if (form.valid()) {
            form.ajaxSubmit({
                target: "",
                type: "POST",
                async: true,
                success: function (res) {

                    if (res.Status === "1") {
                        SAsuccess(res.Message);
                        var url = GetSiteURL() + "Student/StudentRegister";
                        window.location.href = url;
                        //RedirectTo('Home/Dashboard', 1000);
                    }

                    else if (res.Status === "-1") {
                        SAerror(res.Message);
                    }
                    else if (res.Status === "0") {
                        SAerror(res.Message);
                    }
                    else if (res.Status === "Error") {
                        SAerror(res.Message);
                    }
                    else if (res.Status === "Img") {
                        SAerror(res.Message);
                    }

                },
                error: function (data) {

                }
            });

        }
        else {
            error("Please Fill Mendatory Fields.");
        }



    }

}




function GetSiteURL() {

    var str = window.location.href;
    var res = str.split("/");
    var URL = '';
    if (res[2].toLowerCase() === 'localhost'.toLowerCase() || res[2].toLowerCase() === '127.0.0.1'.toLowerCase()) {
        URL = window.location.protocol + "//" + res[2].toLowerCase() + "/" + res[3].toLowerCase();
    }
    else {
        URL = window.location.protocol + "//" + res[2].toLowerCase() + "/";
    }
    SiteURL = URL;
    return URL;
}
