
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('usersCtrlr', ['$http', 'headerConfigService', usersCtrlrFunc]);


    function usersCtrlrFunc($http, headerConfigService) {

        headerConfigService.reset();

        var vm = this;
    }

})();