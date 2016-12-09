
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('profileCtrlr', ['$http', 'appProps', 'headerConfigService', profileCtrlrFunc]);


    function profileCtrlrFunc($http, appProps, headerConfigService) {

        headerConfigService.reset();
        headerConfigService.title = 'Profile';
        headerConfigService.showToolBar = true;
        headerConfigService.showAddBtn = false;

        var vm = this;

        vm.appProps = appProps;

        vm.profile = {
            fName: '',
            lName: '',
            email: '',
            userName: '',
            contactNumber: '',
            isValidated: false,
            accessFailedCount: 0,
            emailConfirmed: false,
            lockoutEnabled: false,
        };

        var getProfile = function () {

            var successFunc = function (resp) {

                if (resp.data
                    && resp.data.result) {

                    vm.profile = resp.data.result;
                }
            };

            var errorFunc = function (resp) {

                if (resp.data
                    && checkRedirectForSignIn(resp.data)) {

                }
            };

            var finallyFunc = function (resp) {

                toggleGlblWaitVisibility(false);
            };



            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlGetProfile, null)
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };

        getProfile();
    }

})();