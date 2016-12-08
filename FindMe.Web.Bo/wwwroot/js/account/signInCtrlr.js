
(function () {

    'use strict';

    //Getting the existing module
    angular.module('app-signin')
           .controller('signInCtrlr', ['$http', signInCtrlrFunc]);

    var $btnSignIn = $('#signin .btn.btn-signin').first();


    function signInCtrlrFunc($http) {

        var vm = this;

        vm.obj = {
            userName: '',
            password: '',
            remember: false
        };


        vm.redirectUrl = '';
        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.isBusy = false;




        var canSignIn = function () {

            return (vm.obj.userName && vm.obj.password);
        };

        var funcSignInSuccess = function (resp) {

            if (resp
                && resp.data) {

                if (resp.data.error) {

                    vm.errorstatus = '';
                    vm.errormsg = resp.data.error.msg;
                    vm.errorid = resp.data.error.id;
                }
                else {

                    window.location.replace(vm.redirectUrl);
                }
            }
        };

        var funcSignInError = function (error) {

            if (error) {

                vm.errorstatus = error.status + ' - ' + error.statusText;
                vm.errormsg = error.data;
                vm.errorid = 0;

                vm.obj.password = '';
            }
        };

        var funcSignInFinally = function () {

            vm.isBusy = false;
            $btnSignIn.children('.fa.fa-spinner').addClass('hidden');
            $btnSignIn.children('.fa.fa-chevron-right').removeClass('hidden');
        };


        vm.signin = function (redirectUrl) {

            vm.redirectUrl = '';
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            if (canSignIn()) {

                if (!redirectUrl) {
                    redirectUrl = '/App/Index';
                }

                vm.redirectUrl = redirectUrl;

                vm.isBusy = true;
                $btnSignIn.children('.fa.fa-spinner').removeClass('hidden');
                $btnSignIn.children('.fa.fa-chevron-right').addClass('hidden');

                $http.post('/ApiAccount/SignIn', vm.obj)
                     .then(funcSignInSuccess, funcSignInError)
                     .finally(funcSignInFinally);
            }
        };
    }

})();