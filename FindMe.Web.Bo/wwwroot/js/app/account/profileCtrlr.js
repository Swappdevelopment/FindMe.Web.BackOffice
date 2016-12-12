
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('profileCtrlr', ['$http', 'appProps', 'headerConfigService', profileCtrlrFunc]);


    function profileCtrlrFunc($http, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


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
            emailToVal: '',
            emailToValToken: '',
            userName: '',
            userNameLocked: false,
            contactNumber: '',
            isValidated: false,
            accessFailedCount: 0,
            emailConfirmed: false,
            lockoutEnabled: false,
        };

        vm.cancelingEmail = false;
        vm.resendingEmail = false;


        var successFunc = function (resp) {

            if (resp.data
                && resp.data.result) {

                vm.profile.fName = resp.data.result.fName;
                vm.profile.lName = resp.data.result.lName;
                vm.profile.email = resp.data.result.email;
                vm.profile.emailLocked = resp.data.result.emailLocked;
                vm.profile.emailToVal = resp.data.result.emailToVal;
                vm.profile.emailToValToken = resp.data.result.emailToValToken;
                vm.profile.userName = resp.data.result.userName;
                vm.profile.userNameLocked = resp.data.result.userNameLocked;
                vm.profile.contactNumber = resp.data.result.contactNumber;
                vm.profile.isValidated = resp.data.result.isValidated;
                vm.profile.accessFailedCount = resp.data.result.accessFailedCount;
                vm.profile.emailConfirmed = resp.data.result.emailConfirmed;
                vm.profile.lockoutEnabled = resp.data.result.lockoutEnabled;

                if (resp.data.fn) {

                    var fn = globalOptns.lbl_HelloNm.replace('{0}', resp.data.fn).replace('{1}', globalOptns.lbl_NoNm);

                    $('#topbar .container .navbar-right a.dropdown-toggle').text(fn);
                }
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

            $http.post(appProps.urlMngProfile, { profile: data })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.cancelEmail = function () {

            vm.cancelingEmail = true;

            var scsFunc = function (resp) {

                vm.profile.emailLocked = false;
                vm.profile.emailToVal = '';
                vm.profile.emailToValToken = '';
            };

            var finFunc = function (resp) {

                vm.cancelingEmail = false;
            };

            $http.post(appProps.urlMngProfile, {
                profile: null,
                action: 'cancelemailconfirmation',
                skipGet: true
            })
            .then(scsFunc, errorFunc)
            .finally(finFunc);
        };

        vm.resendEmail = function () {

            vm.resendingEmail = true;

            var scsFunc = function (resp) {
            };

            var finFunc = function (resp) {

                vm.resendingEmail = false;
            };

            $http.post(appProps.urlMngProfile, {
                profile: {
                    emailToVal: vm.profile.emailToVal,
                    emailToValToken: vm.profile.emailToValToken,
                },
                action: 'resendemailconfirmation',
                skipGet: true
            })
            .then(scsFunc, errorFunc)
            .finally(finFunc);

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