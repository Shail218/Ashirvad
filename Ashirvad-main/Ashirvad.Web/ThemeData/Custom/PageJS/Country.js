function ConfirmationDialog(Id) {
    if (confirm("Are you sure to continue?")) {
        DeleteCountry(Id);
    }
    else {

    } return false;
    
       
}
function OnSubmit() {

    debugger;
    var CountryName = $("#CountryName").val();

    if (CountryName == "") {
        SAerror("Please Select Country");
    }

    else {


        var form = $('#FormItem');
        form[0].action = GetSiteURL() + '/Home/CreateCountry';
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

                        var url = GetSiteURL() + "Home/CountryMaster";
                        window.location.href = url;
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

function DeleteCountry(Id) {
    var Id = Id;
    $.ajax({
        url: GetSiteURL() + "/Home/DeleteCountry",
        type: "POST",
        data: jQuery.param({
            Id: Id
           

        }),
        async: true,
        success: function (res)
        {
            
            if (res.Status === "1") {
                SAsuccess(res.Message);

                var url = GetSiteURL() + "Home/CountryMaster";
                window.location.href = url;
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