
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('exceptionsCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', exceptionsCtrlrFunc]);

    function exceptionsCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Exceptions;
        headerConfigService.showToolBar = false;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddAddrException;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshAddrExceptions;
        headerConfigService.saveBtnTltp = appProps.msg_SaveAddrExceptions;


        var vm = this;

        vm.appProps = appProps;

        vm.addrExceptionsCount = 0;

        var addrIDs;
        vm.addrIDsCount = 0;

        vm.addrExceptions = [];

        vm.hideWarning = false;
        vm.gettingAddressesCount = false;
        vm.verifyingAddresses = false;


        vm.getAddressesCountMessage = function () {

            return vm.appProps.msg_LkngExptnsTime.replace('{0}', vm.addrIDsCount);
        }


        vm.verifiedAddrs = 0;

        vm.getVerifyAddrMessage = function () {

            return vm.appProps.msg_ExAddrVrfd.replace('{0}', vm.verifiedAddrs).replace('{1}', vm.addrIDsCount);
        }


        vm.getAllAddressIDs = function () {

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            var successFunc = function (resp) {

                vm.addrIDsCount = 0;

                if (resp.data && resp.data.result) {

                    addrIDs = resp.data.result;

                    vm.addrIDsCount = addrIDs.length;
                }
            };

            var errorFunc = function (error) {

                if (error.data
                    && checkRedirectForSignIn(error.data)) {

                    vm.errorstatus = error.status + ' - ' + error.statusText;
                    vm.errormsg = error.data.msg;
                    vm.errorid = error.data.id;

                    vm.showError = true;
                }
            };

            var finallyFunc = function () {

                vm.gettingAddressesCount = false;
            };

            vm.gettingAddressesCount = true;

            $http.get(appProps.urlGetAllAddressIDs)
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };

        $('#searchBar').on('searchGo', function (e, arg) {

            if (arg && arg.searchValue) {

                $scope.$apply(function () {

                    vm.searchValue = arg.searchValue;
                    vm.populateAddrExceptions(appProps.resultItemsPerPg, 0, arg.searchValue);
                });
            }
        });
        $('#searchBar').on('searchClear', function (e, arg) {

            $scope.$apply(function () {

                vm.searchValue = '';
                vm.populateAddrExceptions(appProps.resultItemsPerPg, 0);
            });
        });
    }

})();