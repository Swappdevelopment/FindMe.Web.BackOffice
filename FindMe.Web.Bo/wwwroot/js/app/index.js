
(function (viewport) {

    'use strict';


    var trackingBootstrapRes = function (state) {

        switch (state) {
            case 'xs':
            case 'sm':
                $('#wrapper, #sideBar .sb, #wrapper .main-ctrlr').addClass('bs-small-res');
                break;

            default:
                $('#wrapper, #sideBar .sb, #wrapper .main-ctrlr').removeClass('bs-small-res');
                break;
        }
    };


    trackBootstrapRes(trackingBootstrapRes);

    var btnBurgerClick = function () {

        $('#wrapper, #sideBar .sb').toggleClass('hide-lbls');
    };

    $('#wrapper .btn.btn-burger').on('click', btnBurgerClick);



    var $liIcons = $('#sideBar .sb-icons li');
    var $liLbls = $('#sideBar .sb-lbls li');

    var iCount = $liIcons.length;
    var lCount = $liLbls.length;

    var removeActive = function () {
        $(this).removeClass('active');
        $(this).find('a').removeClass('active');
    };

    if (iCount == lCount) {

        var handle = function ($liTarget, $liTargets, $liItems, type) {

            var index = $liTargets.index($liTarget);

            var $item = $($liItems.get(index));

            switch (type) {

                case 'click':

                    var removeActive = function () {
                        $(this).removeClass('active');
                        $(this).find('a').removeClass('active');
                    };

                    $liTargets.each(removeActive);
                    $liItems.each(removeActive);

                    $liTarget.addClass('active');
                    $liTarget.find('a').addClass('active');
                    $item.addClass('active');
                    $item.find('a').addClass('active');

                    break;

                case 'mouseenter':

                    $liTarget.children().addClass('over');
                    $item.addClass('over');
                    $item.children().addClass('over');
                    break;

                default:

                    $liTarget.children().removeClass('over');
                    $item.removeClass('over');
                    $item.children().removeClass('over');
                    break;
            }
        }

        var liIconOver = function (e) {

            handle($(this), $liIcons, $liLbls, e.type);
        };

        var liLblOver = function (e) {

            handle($(this), $liLbls, $liIcons, e.type);
        };

        for (var i = 0; i < iCount; i++) {

            $($liIcons.get(i)).on('mouseenter mouseleave click', liIconOver);
            $($liLbls.get(i)).on('mouseenter mouseleave click', liLblOver);
        }
    }


    var searchFocusChanged = function (e) {

        switch (e.type) {
            case 'focusin':
                $('#searchBar span.input-bg').addClass('focus');
                break;

            default:
                $('#searchBar span.input-bg').removeClass('focus');
                break;
        }
    };

    $('#searchBar span.input-bg input').on('focusin focusout', searchFocusChanged);



    //var btnProfileClick = function () {

    //    $liIcons.each(removeActive);
    //    $liLbls.each(removeActive);

    //    var url = window.location.origin + '/#/profile';

    //    window.location.replace(url);
    //};

    //$('#profile').on('click', btnProfileClick);

})();

