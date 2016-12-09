
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('mainCtrlr', ['$route', '$scope', '$http', 'headerConfigService', mainCtrlrFunc]);


    function mainCtrlrFunc($route, $scope, $http, headerConfigService) {

        var vm = this;

        vm.headerConfig = headerConfigService;


        var viewContentLoaded = function () {

            if (headerConfigService) {

                if (headerConfigService.showSearchCtrl) {
                    showMainSearchCtrl();
                }
                else {
                    hideMainSearchCtrl();
                }
            }

            //var route = String($route.current.templateUrl).toLowerCase().replace('.html', '');

            //route = route.split('/');
            //route = String(route[route.length - 1]);

            //switch (route) {
            //    case '':
            //    case 'home':
            //    case 'profile':
            //        hideMainSearchCtrl();
            //        break;

            //    default:
            //        showMainSearchCtrl();
            //        break;
            //}
        }

        $scope.$on('$viewContentLoaded', viewContentLoaded);
    }


    var showMainSearchCtrl = function () {

        //$('#searchBar .search-ctrl').removeClass('hidden');
        $('#searchBar .search-ctrl').removeClass('clsp');
    };


    var hideMainSearchCtrl = function () {

        $('#searchBar .search-ctrl').addClass('clsp');

        setTimeout(function () {
            $('#searchBar .search-ctrl').addClass('hidden');
        }, 300);
    };

})();