
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

            var postObj = {
                userName: vm.obj.userName,
                password: vm.obj.password,
                remember: vm.obj.remember
            };

            vm.obj.password = '';

            $http.post('/ApiAccount/SignIn', postObj)
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