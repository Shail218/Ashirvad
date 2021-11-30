$(document).ready(function () {
    $('.facultyimage').each(function () {
        var Prefix = 'http://highpack-001-site12.dtempurl.com';
       
        var URL = $(this).attr('src');
        var Middlefix = getId(URL);
        var MainURL = Prefix + Middlefix;
        $(this).attr('src', MainURL);
    });


});