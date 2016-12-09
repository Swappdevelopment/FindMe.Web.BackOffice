
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('homeCtrlr', ['$http', 'appProps', homeCtrlrFunc]);


    function homeCtrlrFunc($http, appProps) {

        var vm = this;

        vm.title = "TESTING";
    }

})();