
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('addressesCtrlr', ['$http', 'headerConfigService', addressesCtrlrFunc]);


    function addressesCtrlrFunc($http, headerConfigService) {

        headerConfigService.reset();

        var vm = this;
    }

})();