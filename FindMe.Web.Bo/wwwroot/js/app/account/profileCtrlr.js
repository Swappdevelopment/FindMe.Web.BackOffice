
(function () {

    'use strict';


    angular.module('app-main')
           .controller('profileCtrlr', ['$http', profileCtrlrFunc]);


    function profileCtrlrFunc($http) {

        var vm = this;

        vm.title = "TESTING";
    }

})();