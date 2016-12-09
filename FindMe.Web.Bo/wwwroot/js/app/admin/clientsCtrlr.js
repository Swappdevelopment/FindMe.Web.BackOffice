
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('clientsCtrlr', ['$http', 'headerConfigService', clientsCtrlrFunc]);


    function clientsCtrlrFunc($http, headerConfigService) {

        headerConfigService.reset();
        headerConfigService.title = 'YYYYY';
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showToolBar = true;

        var vm = this;
    }

})();