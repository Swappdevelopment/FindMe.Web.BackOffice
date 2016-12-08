
(function () {

    'use strict';


    angular.module('app-main')
           .controller('mainCtrlr', ['$http', mainCtrlrFunc]);


    function mainCtrlrFunc($http) {

        var vm = this;

        vm.title = "TESTING";
    }


    $('#profile').on('click', function () {

        var url = window.location.origin + '/#/profile';

        window.location.replace(url);
    });

})();