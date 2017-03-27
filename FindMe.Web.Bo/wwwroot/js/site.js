
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

    $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });

    $(".dropdown-toggle").dropdown();
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



if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.indexOf(searchString, position) === position;
    };
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

String.prototype.encodeHtml = function () {

    return String($('<textarea>' + this + '</textarea>').text());
};


Array.prototype.insert = function (index, item) {

    this.splice(index, 0, item);
};


Array.prototype.remove = function (item) {

    if (item) {

        this.removeAt(this.indexOf(item));
    }
};


Array.prototype.removeAt = function (index) {

    if (index >= 0 && index < this.length) {

        this.splice(index, 1);
    }
};