﻿
var globalOptns;

$(document).ready(function () {
    $('.show-on-load').removeClass('show-on-load');

    $('#signOut').on('click', function () {

        window.location.replace(globalOptns.signOutUrl);
    });
});