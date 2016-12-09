
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('profileCtrlr', ['$http', 'appProps', 'headerConfigService', profileCtrlrFunc]);


    function profileCtrlrFunc($http, appProps, headerConfigService) {

        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Prfl;
        headerConfigService.showToolBar = true;
        headerConfigService.showAddBtn = false;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshPrfl;
        headerConfigService.saveBtnTltp = appProps.msg_SavePrfl;

        var vm = this;


        vm.appProps = appProps;

        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.profile = {
            fName: '',
            lName: '',
            email: '',
            emailLocked: false,
            userName: '',
            userNameLocked: false,
            contactNumber: '',
            isValidated: false,
            accessFailedCount: 0,
            emailConfirmed: false,
            lockoutEnabled: false,
        };



        var successFunc = function (resp) {

            if (resp.data
                && resp.data.result) {

                vm.profile.fName = resp.data.result.fName;
                vm.profile.lName = resp.data.result.lName;
                vm.profile.email = resp.data.result.email;
                vm.profile.emailLocked = resp.data.result.emailLocked;
                vm.profile.userName = resp.data.result.userName;
                vm.profile.userNameLocked = resp.data.result.userNameLocked;
                vm.profile.contactNumber = resp.data.result.contactNumber;
                vm.profile.isValidated = resp.data.result.isValidated;
                vm.profile.accessFailedCount = resp.data.result.accessFailedCount;
                vm.profile.emailConfirmed = resp.data.result.emailConfirmed;
                vm.profile.lockoutEnabled = resp.data.result.lockoutEnabled;
            }
        };

        var errorFunc = function (error) {

            if (error.data
                && checkRedirectForSignIn(error.data)) {

                vm.errorstatus = error.status + ' - ' + error.statusText;
                vm.errormsg = error.data.msg;
                vm.errorid = error.data.id;
            }
        };

        var finallyFunc = function (resp) {

            toggleGlblWaitVisibility(false);
        };

        vm.manageProfile = function (data) {

            if (!data) {
                data = null;
            }

            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlMngProfile, data)
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };

        var buttonClick = function (tag) {
            switch (tag) {

                case 'refresh':
                    vm.manageProfile();
                    break;

                case 'save':
                    vm.manageProfile(vm.profile);
                    break;
            }
        };

        headerConfigService.tbBtnClickCallback = buttonClick;

        vm.manageProfile();
    }

})();