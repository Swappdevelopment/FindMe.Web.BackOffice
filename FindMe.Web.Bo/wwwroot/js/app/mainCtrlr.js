
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('mainCtrlr', ['$route', '$scope', '$http', 'headerConfigService', mainCtrlrFunc]);


    function mainCtrlrFunc($route, $scope, $http, headerConfigService) {

        var vm = this;

        vm.headerConfig = headerConfigService;


        var viewContentLoaded = function () {

            if (headerConfigService) {

                $('#searchBar input.input-search').val('');
                $('#searchBar span.input-bg button.clear').addClass('hidden');

                if (headerConfigService.showSearchCtrl) {
                    showMainSearchCtrl();
                }
                else {
                    hideMainSearchCtrl();
                }

                if (headerConfigService.showToolBar) {
                    showMainToolbar();

                    if (headerConfigService.showRefreshBtn) {
                        showMainTbBtn('refresh');
                    }
                    else {
                        hideMainTbBtn('refresh');
                    }

                    if (headerConfigService.showAddBtn) {
                        showMainTbBtn('add');
                    }
                    else {
                        hideMainTbBtn('add');
                    }

                    if (headerConfigService.showSaveBtn) {
                        showMainTbBtn('save');
                    }
                    else {
                        hideMainTbBtn('save');
                    }
                }
                else {
                    hideMainToolbar();
                }
            }
        }

        $scope.$on('$viewContentLoaded', viewContentLoaded);
    }


    var showMainSearchCtrl = function () {

        $('#searchBar .search-ctrl').removeClass('hidden');

        setTimeout(function () {
            $('#searchBar .search-ctrl').removeClass('clsp');
        }, 30);
    };


    var hideMainSearchCtrl = function () {

        $('#searchBar .search-ctrl').addClass('clsp');

        setTimeout(function () {
            $('#searchBar .search-ctrl').addClass('hidden');
        }, 300);
    };


    var showMainToolbar = function () {

        $('#searchBar .tb').removeClass('hidden');

        setTimeout(function () {
            $('#searchBar .tb').removeClass('clsp');
        }, 30);
    };


    var hideMainToolbar = function () {

        $('#searchBar .tb').addClass('clsp');

        setTimeout(function () {
            $('#searchBar .tb').addClass('hidden');
        }, 300);
    };


    var showMainTbBtn = function (btnName) {

        $('#searchBar .tb .btn-tb.' + btnName).removeClass('hidden');

        setTimeout(function () {
            $('#searchBar .tb .btn-tb.' + btnName).removeClass('clsp');
        }, 30);
    };


    var hideMainTbBtn = function (btnName) {

        $('#searchBar .tb .btn-tb.' + btnName).addClass('clsp');

        setTimeout(function () {
            $('#searchBar .tb .btn-tb.' + btnName).addClass('hidden');
        }, 300);
    };

})();