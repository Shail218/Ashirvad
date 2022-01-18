$(document).ready(function () {
    $('#videoul .videolink').each(function () {
        var Prefix = 'https://www.youtube.com/embed/';
        var Sufix = '?controls=0';
        var URL = $(this).attr('src');
        var Middlefix = getId(URL);
        var MainURL = Prefix + Middlefix;
        $(this).attr('src', MainURL);
    });


});

function getId(url) {
    var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
    var regExp2 = 'https://www.youtube.com/watch?v='
    var match = url.match(regExp);

    if (match && match[2].length == 11) {
        return match[2];
    }
    else if (url.includes(regExp2)) {
        var r = url.split(regExp2);
    }
    else {
        return 'error';
    }
}

function openpdf(target) {
    var url = GetSiteURL() + target
    var a = document.createElement('a');
    a.target = "_blank";
    a.href = url;
    a.click();
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

//$('#myId').html(myId);

//$('#myCode').html('<iframe width="560" height="315" src="//www.youtube.com/embed/' + myId + '" frameborder="0" allowfullscreen></iframe>');