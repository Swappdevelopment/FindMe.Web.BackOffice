
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