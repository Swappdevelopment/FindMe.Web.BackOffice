
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('addressesCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', addressesCtrlrFunc]);

    function addressesCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Clnts;
        headerConfigService.showToolBar = true;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddAddr;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshAddrs;
        headerConfigService.saveBtnTltp = appProps.msg_SaveAddrs;

        var vm = this;

        vm.appProps = appProps;

        vm.clients = [];
        vm.categorys = [];
        vm.cityDetails = [];

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.addressesCount = 0;
        vm.addressesCountMod = 0;

        vm.addresses = [];

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateAddresses(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (address) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteAddressModal.html',
                controller: 'deleteAddressInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#addressesVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            address: address,
                            save: vm.save
                        };
                    }
                }
            });
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



        var checkRecordState = function (address, compAddress, toBeDeleted) {

            if (address
                && compAddress) {

                if (address.id > 0) {

                    if (toBeDeleted) {

                        address.recordState = 30;
                    }
                    else if (address.name !== compAddress.name
                            || address.slug !== compAddress.slug
                            || address.client_Id !== compAddress.client_Id
                            || address.cityDetail_Id !== compAddress.cityDetail_Id
                            || address.category_Id !== compAddress.category_Id
                            || address.latitude !== compAddress.latitude
                            || address.longitude !== compAddress.longitude
                            || address.flgRecByFbFans !== compAddress.flgRecByFbFans
                            || address.rateOverride !== compAddress.rateOverride
                            || address.rateOverrideCount !== compAddress.rateOverrideCount
                            || address.active !== compAddress.active) {

                        address.recordState = 20;
                    }
                    else {

                        address.recordState = 0;
                    }
                }
                else {

                    address.recordState = 10;
                }
            }
        };


        vm.goInEditMode = function (addr) {

            if (addr) {

                addr.inEditMode = true;

                if (!addr.contentLoaded) {

                    var successFunc = function (resp) {

                        if (resp && resp.data && resp.data.result) {

                            jQuery.extend(addr, resp.data.result);

                            addr.contentLoaded = true;
                        }
                    };

                    var errorFunc = function (error) {

                    };

                    var finallyFunc = function () {

                    };

                    $http.post(appProps.urlGetAddressContent, { addrID: addr.id })
                            //{
                            //    addrID: addr.id
                            //})
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        var prevAllNames = '';
        vm.populateAddresses = function (limit, offset, allNames) {

            var forceGetCount = false;

            if (allNames || prevAllNames) {

                forceGetCount = (allNames !== prevAllNames || vm.addressesCountMod !== 0);
            }
            else {

                forceGetCount = (vm.addressesCountMod !== 0);
            }

            prevAllNames = allNames;

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            allNames = !allNames ? vm.searchValue : allNames;

            var successFunc = function (resp) {

                vm.addresses.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0
                        || (resp.data.count === 0 && resp.data.result && resp.data.result.length === 0)) {

                        vm.addressesCountMod = 0;
                        vm.addressesCount = resp.data.count;

                        var ttlPgs = parseInt(vm.addressesCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.addressesCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.clients && resp.data.clients.length > 0) {

                        vm.clients.length = 0;

                        $.each(resp.data.clients, function (index, value) {

                            if (value.legalName) {

                                value.name = value.legalName.encodeHtml();
                            }
                            else {

                                value.name = (!value.civility ? '' : (value.civility.toUpperCase() + ' ')) + (value.lName === null ? '' : value.lName.toUpperCase() + (!value.fName ? '' : ', ')) + (!value.fName ? '' : value.fName).encodeHtml();
                            }

                            vm.clients.push(value);
                        });
                    }

                    if (resp.data.categorys && resp.data.categorys.length > 0) {

                        vm.categorys.length = 0;

                        $.each(resp.data.categorys, function (index, value) {

                            value.name = value.name.encodeHtml();

                            vm.categorys.push(value);

                            if (value.children && value.children.length > 0) {

                                $.each(value.children, function (i, v) {

                                    v.name = v.name.encodeHtml();
                                    vm.categorys.push(v);
                                });
                            }
                        });
                    }

                    if (resp.data.cityDetails && resp.data.cityDetails.length > 0) {

                        vm.cityDetails.length = 0;

                        $.each(resp.data.cityDetails, function (index, value) {

                            value.name = value.name.encodeHtml();
                            vm.cityDetails.push(value);
                        });
                    }


                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var address = resp.data.result[i];

                            address.active = address.status === 1;

                            var tempName;

                            if (vm.clients.length > 0) {

                                tempName = $.grep(vm.clients, function (v) {
                                    return v.id === address.client_Id;
                                });

                                if (tempName && tempName.length > 0) {

                                    address.client = tempName[0];
                                }
                            }

                            if (vm.categorys.length > 0) {

                                tempName = $.grep(vm.categorys, function (v) {
                                    return v.id === address.category_Id;
                                });

                                if (tempName && tempName.length > 0) {

                                    address.category = tempName[0];
                                }
                            }

                            if (vm.cityDetails.length > 0) {

                                tempName = $.grep(vm.cityDetails, function (v) {
                                    return v.id === address.cityDetail_Id;
                                });

                                if (tempName && tempName.length > 0) {

                                    address.cityDetail = tempName[0];
                                }
                            }

                            address.__comp = jQuery.extend(false, {}, address);

                            vm.addresses.push(address);
                            vm.addresses.push({ link: address });

                            address.inEditMode = false;
                            address.saving = false;
                        }
                    }
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

                toggleGlblWaitVisibility(false);
            };

            toggleGlblWaitVisibility(true);

            $http.post(
                    appProps.urlGetAddresses,
                    {
                        limit: limit,
                        offset: offset,
                        getTotalAddresses: (vm.addressesCount <= 0 || forceGetCount),
                        allNames: (allNames ? allNames : null),
                        getRefClients: (vm.clients.length == 0),
                        getRefCatgs: (vm.categorys.length == 0),
                        getRefCities: (vm.cityDetails.length == 0)
                    })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (address) {

            if (address
                && address.__comp) {

                if (address.id > 0) {

                    var org = address.__comp;

                    address.name = org.name;
                    address.slug = org.slug;
                    address.client_Id = org.client_Id;
                    address.cityDetail_Id = org.cityDetail_Id;
                    address.category_Id = org.category_Id;
                    address.latitude = org.latitude;
                    address.longitude = org.longitude;
                    address.flgRecByFbFans = org.flgRecByFbFans;
                    address.rateOverride = org.rateOverride;
                    address.rateOverrideCount = org.rateOverrideCount;
                    address.client = org.client;
                    address.category = org.category;
                    address.cityDetail = org.cityDetail;

                    address.active = org.active;

                    address.inEditMode = false;
                }
                else {

                    vm.addresses.remove(address);
                }
            }
        };


        vm.save = function (addresses, finallyCallback, deleteFlags) {

            if (addresses) {

                if (!Array.isArray(addresses)) {

                    addresses = [addresses];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validAddresses = [];
                var toBeSavedAddresses = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length === addresses.length);

                for (var i = 0; i < addresses.length; i++) {

                    var address = addresses[i];

                    if (address
                        && address.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, address);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        toBeSaved.client_Id = toBeSaved.client.id;
                        delete toBeSaved.client;

                        toBeSaved.category_Id = toBeSaved.category.id;
                        delete toBeSaved.category;

                        toBeSaved.cityDetail_Id = toBeSaved.cityDetail.id;
                        delete toBeSaved.cityDetail;

                        checkRecordState(toBeSaved, address.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedAddresses.push(toBeSaved);

                        address.saving = true;
                        validAddresses.push(address);
                    }
                }

                if (validAddresses.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validAddresses.length === resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validAddresses[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.name = value.name;
                                        tempValue.slug = value.slug;
                                        tempValue.client_Id = value.client_Id;
                                        tempValue.cityDetail_Id = value.cityDetail_Id;
                                        tempValue.category_Id = value.category_Id;
                                        tempValue.latitude = value.latitude;
                                        tempValue.longitude = value.longitude;
                                        tempValue.flgRecByFbFans = value.flgRecByFbFans;
                                        tempValue.rateOverride = value.rateOverride;
                                        tempValue.rateOverrideCount = value.rateOverrideCount;
                                        tempValue.rate = value.rate;
                                        tempValue.rateCount = value.rateCount;
                                        tempValue.status = value.status;
                                        toBeSaved.active = (toBeSaved.status === 1);

                                        if (toBeSavedAddresses[index].recordState === 10) {

                                            vm.addressesCountMod += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.addresses.remove(tempValue);

                                        vm.addressesCountMod -= 1;

                                        if ((vm.addressesCount + vm.addressesCountMod) <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }

                                    $scope.$apply();
                                });
                            }
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

                        for (var i = 0; i < validAddresses.length; i++) {

                            validAddresses[i].saving = false;
                            validAddresses[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    vm.showError = false;
                    vm.errorstatus = '';
                    vm.errormsg = '';
                    vm.errorid = 0;

                    $http.post(appProps.urlSaveAddresses, { addresses: toBeSavedAddresses })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                $scope.$apply(function () {

                    vm.populateAddresses();
                });
            }
            else if ($btn.hasClass('add')) {

                $scope.$apply(function () {

                    var newItem = {
                        id: 0,
                        name: '',
                        slug: '',
                        client_Id: 0,
                        cityDetail_Id: 0,
                        category_Id: 0,
                        latitude: 0,
                        longitude: 0,
                        flgRecByFbFans: false,
                        rateOverride: -1,
                        rateOverrideCount: -1,
                        rate: 0,
                        rateCount: 0,
                        status: 1,
                        active: true
                    };

                    newItem.__comp = jQuery.extend(true, {}, newItem);

                    newItem.inEditMode = true;
                    newItem.saving = false;

                    vm.addresses.insert(0, newItem);
                });
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);

        $('#searchBar').on('searchGo', function (e, arg) {

            if (arg && arg.searchValue) {

                $scope.$apply(function () {

                    vm.searchValue = arg.searchValue;
                    vm.populateAddresses(appProps.resultItemsPerPg, 0, arg.searchValue);
                });
            }
        });
        $('#searchBar').on('searchClear', function (e, arg) {

            $scope.$apply(function () {

                vm.searchValue = '';
                vm.populateAddresses(appProps.resultItemsPerPg, 0);
            });
        });
    }


    angular.module('app-mainmenu')
        .controller('deleteAddressInstanceCtrlr', ['$uibModalInstance', 'appProps', 'param', deleteAddressInstanceCtrlrFunc]);

    function deleteAddressInstanceCtrlrFunc($uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.address = param.address;

        vm.yes = function () {

            $uibModalInstance.close(vm.address);

            toggleGlblWaitVisibility(true);

            param.save(param.address, function () {

                toggleGlblWaitVisibility(false);
            }, true);
        };

        vm.no = function () {

            $uibModalInstance.close();
        };
    }

})();