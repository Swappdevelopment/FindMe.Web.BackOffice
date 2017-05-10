
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('profileCtrlr', ['$http', '$scope', '$interval', 'appProps', 'listItemService', 'headerConfigService', profileCtrlrFunc]);


    function profileCtrlrFunc($http, $scope, $interval, appProps, listItemService, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });

        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Prfl;
        headerConfigService.showToolBar = true;
        headerConfigService.showAddBtn = false;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshPrfl;
        headerConfigService.saveBtnTltp = appProps.msg_SavePrfl;


        var vm = this;

        vm.appProps = appProps;

        vm.hidePassword = true;

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
            emailToValSent: false,
            userName: '',
            userNameLocked: false,
            contactNumber: '',
            isEmailValidated: false,
            accessFailedCount: 0,
            emailConfirmed: false,
            lockoutEnabled: false,
        };

        vm.cancelingEmail = false;
        vm.resendingEmail = false;

        vm.password = {
            oldPassword: null,
            newPassword: null,
            cnfrmPassword: null
        };


        var successFunc = function (resp) {

            if (resp.data
                && resp.data.result) {

                vm.profile.fName = resp.data.result.fName;
                vm.profile.lName = resp.data.result.lName;
                vm.profile.email = resp.data.result.email;
                vm.profile.emailLocked = resp.data.result.emailLocked;
                vm.profile.emailToVal = resp.data.result.emailToVal;
                vm.profile.emailToValToken = resp.data.result.emailToValToken;
                vm.profile.emailToValSent = resp.data.result.emailToValSent;
                vm.profile.userName = resp.data.result.userName;
                vm.profile.userNameLocked = resp.data.result.userNameLocked;
                vm.profile.contactNumber = resp.data.result.contactNumber;
                vm.profile.isEmailValidated = resp.data.result.isEmailValidated;
                vm.profile.accessFailedCount = resp.data.result.accessFailedCount;
                vm.profile.emailConfirmed = resp.data.result.emailConfirmed;
                vm.profile.lockoutEnabled = resp.data.result.lockoutEnabled;

                if (resp.data.fn) {

                    var fn = globalOptns.lbl_HelloNm.replace('{0}', resp.data.fn).replace('{1}', globalOptns.lbl_NoNm);

                    $('#topbar .container .navbar-right a.dropdown-toggle').text(fn);
                }

                if (!vm.profile.emailToValSent
                    && vm.profile.emailToVal
                    && vm.profile.emailToVal.length > 0
                    && vm.profile.emailToValToken
                    && vm.profile.emailToValToken.length > 0) {

                    vm.resendEmail();
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

        var finallyFunc = function () {

            toggleGlblWaitVisibility(false);
        };



        vm.initPasswordFrm = function (psswrd) {

            if (psswrd) {

                listItemService.setItemFormValidation(
                        psswrd,
                        [{
                            name: 'oldPassword',
                            isReq: true
                        },
                        {
                            name: 'newPassword',
                            isReq: true
                        },
                        {
                            name: 'cnfrmPassword',
                            moreValidations: {
                                name: 'isEqualToNewPassword',
                                func: function () {

                                    return psswrd.newPassword === psswrd.cnfrmPassword;
                                }
                            }
                        }]);
            }
        };



        vm.changePasswordClick = function (psswrd) {

            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            if (vm.hidePassword) {

                vm.hidePassword = !vm.hidePassword;
            }
            else {

                var param = {
                    oldPassword: vm.password.oldPassword,
                    newPassword: vm.password.newPassword
                };

                vm.password.formVal.oldPassword.isTouched = false;
                vm.password.formVal.newPassword.isTouched = false;
                vm.password.formVal.cnfrmPassword.isTouched = false;

                vm.password.oldPassword = null;
                vm.password.newPassword = null;
                vm.password.cnfrmPassword = null;

                var changePasswordSuccessFunc = function (resp) {

                    if (resp && resp.data && resp.data.error) {

                        vm.errorstatus = '';
                        vm.errormsg = resp.data.error;
                        vm.errorid = 0;
                    }
                    else {

                        vm.hidePassword = true;
                    }
                };

                toggleGlblWaitVisibility(true);

                $http.post(appProps.urlChangePassword, param)
                     .then(changePasswordSuccessFunc, errorFunc)
                     .finally(finallyFunc);
            }
        };

        vm.changePasswordKeypressed = function ($event, psswrd) {

            if ($event && $event.keyCode === 13) {

                if (vm.password.formVal && vm.password.formVal.isValid()) {

                    vm.changePasswordClick(psswrd);
                }
            }
        };

        vm.passwordReady = function () {

            var $oldPswrd = $('div[uib-collapse]').find('input[name="oldPassword"]');

            if ($oldPswrd && $oldPswrd.length > 0) {

                $oldPswrd.focus();
            }
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



        var checkEmailToken = function () {

            var scsFunc = function (resp) {

                if (resp
                    && resp.data
                    && resp.data.result) {

                    vm.manageProfile();
                }
                else {
                    $interval(checkEmailToken, 5000, 1);
                }
            };

            $http.post(appProps.urlCheckEmailToken, {
                tokenValue: vm.profile.emailToValToken
            })
            .then(scsFunc, scsFunc);
        };


        vm.resendEmail = function () {

            vm.resendingEmail = true;

            var scsFunc = function (resp) {
                $interval(checkEmailToken, 5000, 1);
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


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                vm.manageProfile();
            }
            else if ($btn.hasClass('save')) {

                vm.manageProfile(vm.profile);
                //$('form[name="userProfileForm"]').submit();
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);
    }

})();