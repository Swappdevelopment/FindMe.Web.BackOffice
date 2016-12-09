
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('homeCtrlr', ['$http', 'appProps', homeCtrlrFunc]);


    function homeCtrlrFunc($http, appProps) {

        var vm = this;

        vm.title = "TESTING";
    }


    $('#profile').on('click', function () {

        var url = window.location.origin + '/#/profile';

        window.location.replace(url);
    });

})();