﻿
var globalOptns;

var $respTrackerLg = $('#respTracker span.visible-lg');
var $respTrackerMd = $('#respTracker span.visible-md');
var $respTrackerSm = $('#respTracker span.visible-sm');
var $respTrackerXs = $('#respTracker span.visible-xs');

$(document).ready(function () {
    $('.show-on-load').removeClass('show-on-load');

    $('#signOut').on('click', function () {

        window.location.replace(globalOptns.signOutUrl);
    });
});


function getBootstrapResStatus() {

    var result = '';

    if (!$respTrackerLg.is(':hidden')) {
        result = 'lg';
    }
    else if (!$respTrackerMd.is(':hidden')) {
        result = 'md';
    }
    else if (!$respTrackerSm.is(':hidden')) {
        result = 'sm';
    }
    else if (!$respTrackerXs.is(':hidden')) {
        result = 'xs';
    }

    return result;
}


function trackBootstrapRes(callback) {

    if (callback) {
        var state = getBootstrapResStatus();

        callback(state);

        $(window).resize(function () {
            var s = getBootstrapResStatus();

            if (s != state) {
                state = s;

                callback(state);
            }
        });
    }
}


function toggleGlblWaitVisibility(value) {

    if (typeof value === "undefined") {

        $('#glblWait').toggleClass('hidden');
    }
    else {

        if (value) {

            $('#glblWait').removeClass('hidden');
        }
        else {

            $('#glblWait').addClass('hidden');
        }
    }
}


function checkRedirectForSignIn(data) {

    if (data && data.id) {

        if (data.id == globalOptns.refreshTokenDeadID) {
            window.location.replace(globalOptns.signInUrl);

            return false;
        }
    }

    return true;
}