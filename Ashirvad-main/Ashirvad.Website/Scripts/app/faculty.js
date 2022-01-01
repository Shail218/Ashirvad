$(document).ready(function () {
    $('.facultyimage').each(function () {
        var Prefix = 'https://mastermind.org.in';
       
        var URL = $(this).attr('src');
        var Middlefix = getId(URL);
        var MainURL = Prefix + Middlefix;
        $(this).attr('src', MainURL);
    });


});