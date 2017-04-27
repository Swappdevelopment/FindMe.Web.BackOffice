
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

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }


                if (setPage(pg.index)) {

                    vm.currentPgNmbr = pg.index;
                    pg.isActive = true;
                }
            }
        };

        var setPage = function (index) {

            var startIndex = appProps.resultItemsPerPg * index;

            if (vm.addrExceptions.length > startIndex) {

                vm.addrExceptionsPage.length = 0;

                var length = startIndex + appProps.resultItemsPerPg;

                if (vm.addrExceptions.length <= length) {

                    length = vm.addrExceptions.length;
                }

                for (var i = startIndex; i < length; i++) {

                    vm.addrExceptionsPage.push(vm.addrExceptions[i]);
                }

                return true;
            }

            return false;
        };


        var setupPages = function () {

            vm.pgsCollection.length = 0;

            if (vm.totalPgs > 1) {

                var minValue = vm.currentPgNmbr - 2;
                var maxValue = vm.currentPgNmbr + 2;

                if (minValue < 0) {

                    maxValue += (0 - minValue);
                    minValue = 0;
                }

                if (maxValue >= vm.totalPgs) {

                    maxValue = vm.totalPgs - 1;
                }

                while ((maxValue - minValue) < 4) {

                    minValue--;
                }

                minValue = minValue < 0 ? 0 : minValue;

                vm.pgsCollection.push({

                    index: 0,
                    isActive: false,
                    text: '',
                    icon: 'fa fa-step-backward',
                    disabled: (vm.currentPgNmbr <= 0)
                });

                for (var i = minValue; i <= maxValue; i++) {

                    vm.pgsCollection.push({

                        index: i,
                        isActive: (vm.currentPgNmbr === i),
                        text: String(i + 1),
                        icon: '',
                        disabled: false
                    });
                }

                vm.pgsCollection.push({

                    index: vm.totalPgs - 1,
                    isActive: false,
                    text: '',
                    icon: 'fa fa-step-forward',
                    disabled: (vm.currentPgNmbr >= (vm.totalPgs - 1))
                });
            }
        };



        vm.addrExceptionsCount = 0;

        var addrIDs;
        vm.addrIDsCount = 0;

        vm.addrExceptions = [];
        vm.addrExceptionsPage = [];

        vm.hideWarning = false;
        vm.gettingAddressesCount = false;
        vm.verifyingAddresses = false;

        vm.cancelVerification = false;


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


        var readyException = function (raw) {

            if (raw) {

                var status = '';

                switch (raw.status) {

                    case 10:

                        status = 'File Missing';
                        break;

                    case 20:

                        status = 'Invalid File';
                        break;

                    case 30:

                        status = 'Thumbnail Missing';
                        break;

                    case 40:

                        status = 'Invalid Thumbnail';
                        break;

                    case 50:

                        status = 'Multiple Slugs';
                        break;

                    case 60:

                        status = 'Invalid City';
                        break;

                    case 70:

                        status = 'Invalid Category';
                        break;
                }


                var result = { name: raw.name, slug: raw.slug, status: status, showMsgs: false };
                result.data = raw.data;
                delete raw.data;
                result.raw = raw;

                if (result.data && result.data.length > 0) {

                    $.each(result.data, function () {

                        switch (raw.status) {

                            case 10:
                            case 20:
                            case 30:
                            case 40:

                                this.msg = 'File [ ' + (raw.status === 30 || raw.status === 40 ? 'Dimension: ' + this.th + ' | ' : '') + 'Name: ' + (this.file.name ? this.file.name : '<NONE>') + ' | UID: ' + this.file.uid + ' | Type: ' + this.type + ' | Format: ' + this.file.format + ' ]';
                                break;

                            case 50:
                                this.msg = this.addSlugsCount + ' more Address(es) use this Slug';
                                break;
                        }
                    });
                }

                return result;
            }


            return raw;
        };


        vm.verifyAddresses = function (addrIDIndex) {

            vm.hideWarning = true;

            if (!addrIDIndex) {

                addrIDIndex = 0;
            }

            if (addrIDIndex === 0) {

                vm.verifiedAddrs = 0;
                vm.addrExceptionsCount = 0;
                vm.addrExceptions.length = 0;
                vm.addrExceptionsPage.length = 0;
                vm.cancelVerification = false;
            }

            if (vm.cancelVerification || !addrIDs || addrIDIndex < 0 || addrIDIndex >= addrIDs.length) {

                vm.cancelVerification = false;
                vm.verifyingAddresses = false;
            }
            else {

                vm.showError = false;
                vm.errorstatus = '';
                vm.errormsg = '';
                vm.errorid = 0;

                var successFunc = function (resp) {

                    vm.verifiedAddrs += 1;

                    if (resp.data && resp.data.exceptions) {

                        $.each(resp.data.exceptions, function () {

                            var processedEx = readyException(this);

                            vm.addrExceptions.push(processedEx);

                            if (vm.addrExceptionsPage.length < appProps.resultItemsPerPg) {

                                vm.addrExceptionsPage.push(processedEx);
                            }

                            vm.addrExceptionsCount += 1;
                        });
                    }
                };

                var errorFunc = function (error) {

                    vm.verifiedAddrs += 1;

                    if (error.data
                        && checkRedirectForSignIn(error.data)) {

                        var exErr = { hasError: true, status: error.status, msg: error.data.msg, id: error.data.id };

                        vm.addrExceptions.push(exErr);

                        if (vm.addrExceptionsPage.length < appProps.resultItemsPerPg) {

                            vm.addrExceptionsPage.push(exErr);
                        }

                        vm.addrExceptionsCount += 1;
                    }
                };

                var finallyFunc = function () {

                    var ttlPgs = parseInt(vm.addrExceptionsCount / appProps.resultItemsPerPg);

                    ttlPgs += ((vm.addrExceptionsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                    vm.totalPgs = ttlPgs;

                    vm.verifyAddresses(addrIDIndex + 1);

                    setupPages();
                };


                vm.verifyingAddresses = true;

                $http.post(appProps.urlVerifyAddress, { addrID: addrIDs[addrIDIndex], addrUID: null })
                     .then(successFunc, errorFunc)
                     .finally(finallyFunc);
            }
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