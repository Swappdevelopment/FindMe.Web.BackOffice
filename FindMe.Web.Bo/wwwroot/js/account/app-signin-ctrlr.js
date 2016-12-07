
(function () {

    'use strict';

    //Getting the existing module
    angular.module('app-signin', [])
           .controller('app-signin-ctrlr', controllersignin);

    var $btnSignIn = $('#signin .btn.btn-signin').first();


    function controllersignin($http) {

        var vm = this;

        vm.obj = {
            userName: '',
            password: '',
            remember: false
        };


        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.isBusy = false;


        vm.signin = function (redirectUrl) {

            vm.errormsg = '';
            vm.errorid = 0;

            if (!redirectUrl) {
                redirectUrl = '/App/Index';
            }

            vm.isBusy = true;
            $btnSignIn.children('.fa.fa-spinner').removeClass('hidden');
            $btnSignIn.children('.fa.fa-chevron-right').addClass('hidden');

            $http.post('/ApiAccount/SignIn', vm.obj)
                 .then(
                    function (resp) {

                        if (resp
                            && resp.data) {

                            if (resp.data.error) {

                                vm.errorstatus = '';
                                vm.errormsg = resp.data.error.msg;
                                vm.errorid = resp.data.error.id;
                            }
                            else {

                                window.location.replace(redirectUrl);
                            }
                        }
                    },
                    function (error) {

                        if (error) {

                            vm.errorstatus = error.status + ' - ' + error.statusText;
                            vm.errormsg = error.data;
                            vm.errorid = 0;

                            vm.obj.password = '';
                        }
                    })
                 .finally(function () {

                     vm.isBusy = false;
                     $btnSignIn.children('.fa.fa-spinner').addClass('hidden');
                     $btnSignIn.children('.fa.fa-chevron-right').removeClass('hidden');
                 });
        };
    }

})();