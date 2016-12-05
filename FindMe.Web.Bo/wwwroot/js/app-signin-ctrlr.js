
(function () {

    'use strict';

    //Getting the existing module
    angular.module('app-signin', [])
           .controller('app-signin-ctrlr', controllersignin);


    function controllersignin($http) {

        var vm = this;

        vm.obj = {
            userName: '',
            password: '',
            remember: false
        };


        vm.error = {
            msg: '',
            id: 0
        };

        vm.isBusy = false;


        vm.signin = function () {

            vm.isBusy = true;

            $http.post('/ApiAccount/SignIn', vm.obj)
                 .then(
                    function (resp) {
                        var r = resp;
                    },
                    function (error) {
                        var err = error;
                    })
                 .finally(function () {

                     vm.isBusy = false;
                 });
        };
    }

})();