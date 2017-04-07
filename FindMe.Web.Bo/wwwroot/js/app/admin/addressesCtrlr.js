
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
        vm.tags = [];

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



        var checkRatinOvrdRecordState = function (ratingOvrds) {

            if (ratingOvrds) {


                if (!Array.isArray(ratingOvrds)) {

                    ratingOvrds = [ratingOvrds];
                }

                for (var i = 0; i < ratingOvrds.length; i++) {

                    var ratingOvrd = ratingOvrds[i];

                    if (ratingOvrd.__comp
                        && (!ratingOvrd.recordState || ratingOvrd.recordState <= 0)) {

                        if (ratingOvrd.id > 0) {

                            if (ratingOvrd.fromDate !== ratingOvrd.__comp.fromDate
                                || ratingOvrd.toDate !== ratingOvrd.__comp.toDate
                                || ratingOvrd.value !== ratingOvrd.__comp.value
                                || ratingOvrd.clickCount !== ratingOvrd.__comp.clickCount) {

                                ratingOvrd.recordState = 20;
                            }
                            else {

                                ratingOvrd.recordState = 0;
                            }
                        }
                        else {

                            address.recordState = 10;
                        }
                    }
                }
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

                checkRatinOvrdRecordState(address.ratingOverrides);
            }
        };



        var handleDbAddress = function (address) {

            if (address) {

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
            }
        };



        var editModal = function (address) {

            $('body').attr('style', 'overflow: hidden; position: fixed;');

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'addressEditorModal.html',
                controller: 'addressEditorInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#addressesVw .modal-editor'),
                resolve: {
                    param: function () {

                        return {
                            address: address,
                            save: vm.save,
                            deleteModal: vm.deleteModal,
                            checkRecordState: checkRecordState,
                            clients: vm.clients,
                            categorys: vm.categorys,
                            cityDetails: vm.cityDetails,
                            tags: vm.tags
                        };
                    }
                }
            });
        };


        var setUpContent = function (value) {

            if (value) {

                if (value.ratingOverrides) {

                    $.each(value.ratingOverrides, function (i, v) {

                        v.fromDate = new Date(v.fromUtc);
                        v.toDate = v.toUtc ? new Date(v.toUtc) : null;

                        v.__comp = jQuery.extend(false, {}, v);
                    });
                }

                if (value.tags) {

                    $.each(value.tags, function (i, v) {

                        v.name = appProps.currentLang.startsWith('en') ? v.name_en : v.name_fr;
                    });
                }

                if (value.images) {

                    $.each(value.images, function (i, v) {

                        v.active = v.status === 1;
                    });
                }
            }
        };


        vm.goInEditMode = function (address) {

            if (address) {

                if (address.hasContent || address.id <= 0) {

                    editModal(address);
                }
                else {

                    var successFunc = function (resp) {

                        if (resp.data && resp.data.result) {

                            jQuery.extend(address, resp.data.result);

                            address.hasContent = true;

                            setUpContent(address);

                            editModal(address);
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

                        address.gettingContent = false;
                    };


                    address.gettingContent = true;


                    $http.post(appProps.urlGetAddressContent, { addrID: address.id })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        vm.deleteModal = function (address) {

            if (address) {

                if (address.id > 0) {

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
                }
                else {

                    vm.addresses.remove(address);
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

                            handleDbAddress(address);

                            vm.addresses.push(address);
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
                        getRefClients: (vm.clients.length === 0),
                        getRefCatgs: (vm.categorys.length === 0),
                        getRefCities: (vm.cityDetails.length === 0)
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

                        toBeSaved.client_Id = toBeSaved.client ? toBeSaved.client.id : 0;
                        delete toBeSaved.client;

                        toBeSaved.category_Id = toBeSaved.category ? toBeSaved.category.id : 0;
                        delete toBeSaved.category;

                        toBeSaved.cityDetail_Id = toBeSaved.cityDetail ? toBeSaved.cityDetail.id : 0;
                        delete toBeSaved.cityDetail;

                        checkRecordState(toBeSaved, address.__comp, hasDeleteFlags ? deleteFlags[i] : false);


                        if (toBeSaved.ratingOverrides) {

                            $.each(toBeSaved.ratingOverrides, function (i, v) {

                                v.fromUtc = appProps.swIsoFormat(v.fromDate, true);
                                v.toUtc = appProps.swIsoFormat(v.toDate, true);

                                delete v.fromDate;
                                delete v.toDate;
                            });
                        }

                        toBeSavedAddresses.push(toBeSaved);
                        validAddresses.push(address);
                    }
                }

                if (validAddresses.length > 0) {

                    var saveSuccess = false;

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validAddresses.length === resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validAddresses[index];

                                    var i;

                                    if (value && value.address) {

                                        tempValue.id = value.address.id;
                                        tempValue.name = value.address.name;
                                        tempValue.slug = value.address.slug;
                                        tempValue.client_Id = value.address.client_Id;
                                        tempValue.cityDetail_Id = value.address.cityDetail_Id;
                                        tempValue.category_Id = value.address.category_Id;
                                        tempValue.latitude = value.address.latitude;
                                        tempValue.longitude = value.address.longitude;
                                        tempValue.flgRecByFbFans = value.address.flgRecByFbFans;
                                        tempValue.rateOverride = value.address.rateOverride;
                                        tempValue.rateOverrideCount = value.address.rateOverrideCount;
                                        tempValue.rate = value.address.rate;
                                        tempValue.rateCount = value.address.rateCount;
                                        tempValue.status = value.address.status;
                                        tempValue.active = (tempValue.status === 1);

                                        handleDbAddress(tempValue);

                                        setUpContent(value);

                                        if (value.ratingOverrides && tempValue.ratingOverrides) {

                                            tempValue.ratingOverrides.length = 0;

                                            for (i = 0; i < value.ratingOverrides.length; i++) {

                                                tempValue.ratingOverrides.push(value.ratingOverrides[i]);
                                            }
                                        }

                                        if (value.tags && tempValue.tags) {

                                            tempValue.tags.length = 0;

                                            for (i = 0; i < value.tags.length; i++) {

                                                tempValue.tags.push(value.tags[i]);
                                            }
                                        }

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
                                });
                            }

                            saveSuccess = true;
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

                        if (finallyCallback) {

                            finallyCallback(saveSuccess);
                        }
                    };

                    vm.showError = false;
                    vm.errorstatus = '';
                    vm.errormsg = '';
                    vm.errorid = 0;

                    toggleGlblWaitVisibility(true);

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
                        rateOverride: 0,
                        rateOverrideCount: 0,
                        rate: 0,
                        rateCount: 0,
                        status: 1,
                        active: true,
                        recordState: 10
                    };

                    newItem.__comp = jQuery.extend(true, {}, newItem);

                    newItem.inEditMode = true;
                    newItem.saving = false;

                    vm.addresses.insert(0, newItem);

                    vm.goInEditMode(newItem);
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
        .controller('addressEditorInstanceCtrlr', ['$http', '$scope', '$uibModalInstance', 'appProps', 'param', addressEditorInstanceCtrlrFunc]);

    function addressEditorInstanceCtrlrFunc($http, $scope, $uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.address = param.address;

        vm.clients = param.clients;
        vm.categorys = param.categorys;
        vm.cityDetails = param.cityDetails;
        vm.tags = param.tags;
        vm.tagFilter = '';

        $scope.$watch('vm.tagFilter', function (newValue, oldValue) {

            if (vm.tags && vm.tags.length > 0) {

                var tag;
                var i;

                if (newValue && newValue.length > 0) {

                    var filterAccenFold = String(accentFold(newValue)).toLowerCase();

                    for (i = 0; i < vm.tags.length; i++) {

                        tag = vm.tags[i];

                        tag.isFilteredOut = (String(accentFold(tag.name)).toLowerCase().indexOf(filterAccenFold) < 0);
                    }
                }
                else {

                    for (i = 0; i < vm.tags.length; i++) {

                        tag = vm.tags[i];

                        tag.isFilteredOut = false;
                    }
                }
            }
        });

        vm.selectedTagCount = 0;

        vm.selectTag = function (tag) {

            if (tag) {

                if (tag.isSelected) {

                    tag.isSelected = false;
                    if (vm.selectedTagCount > 0) {
                        vm.selectedTagCount--;
                    }
                }
                else {

                    tag.isSelected = true;
                    vm.selectedTagCount++;
                }
            }
        };

        vm.showAddTags = false;
        vm.gettingAddTags = false;

        vm.applyTags = function () {

            if (vm.tags && vm.tags.length > 0) {

                for (var i = 0; i < vm.tags.length; i++) {

                    var tag = vm.tags[i];

                    if (tag.isSelected) {

                        vm.address.tags.insert(0, {
                            recordState: 10,
                            id: tag.id,
                            address_Id: vm.address.id,
                            name: tag.name,
                            active: true,
                            status: 1
                        });
                    }
                }
            }

            vm.showAddTags = false;
        };

        vm.addressTagClick = function (addrTag) {

            if (addrTag) {

                switch (addrTag.recordState) {

                    case 30:
                        addrTag.recordState = 0;
                        break;

                    case 10:
                        addrTag.recordState = 0;
                        vm.address.tags.remove(addrTag);
                        break

                    default:
                        addrTag.recordState = 30;
                        break;
                }
            }
        };

        vm.openAddTags = function () {

            vm.selectedTagCount = 0;

            vm.showAddTags = true;

            if (vm.tags && vm.tags.length > 0) {

                for (var i = 0; i < vm.tags.length; i++) {

                    var tag = vm.tags[i];

                    tag.isSelected = false;
                    tag.isFilteredOut = false;
                    tag.isVisible = true;

                    if ($.grep(vm.address.tags, function (v) { return v.id === tag.id; }).length > 0) {

                        tag.isVisible = false;
                    }
                }
            }
            else {

                vm.gettingAddTags = true;

                var successFunc = function (resp) {

                    vm.tags.length = 0;

                    if (resp.data && resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var tag = resp.data.result[i];

                            tag.name = appProps.currentLang.startsWith('en') ? tag.name_en : tag.name_fr;
                            tag.isSelected = false;
                            tag.isFilteredOut = false;
                            tag.isVisible = true;

                            if ($.grep(vm.address.tags, function (v) { return v.id === tag.id; }).length > 0) {

                                tag.isVisible = false;
                            }

                            vm.tags.push(tag);
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

                    vm.gettingAddTags = false;
                };

                $http.post(appProps.urlGetTags, {})
                     .then(successFunc, errorFunc)
                     .finally(finallyFunc);
            }

            $('#addressesVw .modal-editor .modal-dialog .modal-content .add-tags-bg .add-tags-container .form-group .form-control input').on('focusin focusout', function (e) {

                var $parent = $($(this).parent());

                if ($parent && $parent.length > 0) {

                    $parent.toggleClass('has-focus');
                }
            });
        };


        vm.uploadImgClick = function ($event, img) {

            if ($event && $event.currentTarget) {

                var $fileInput = $($event.currentTarget).siblings('input[type="file"]');

                if ($fileInput && $fileInput.length > 0) {

                    $fileInput.off('change');

                    $fileInput.on('change', function () {

                        var $parentForm = $(this).parents('form.img-upload').first();

                        if ($parentForm && $parentForm.length > 0) {

                            var fd = new FormData();

                            $.each($fileInput.get(0).files, function (index, value) {

                                fd.append(value.name, value);
                                fd.append(value.name + '_' + index + '_KEY', JSON.stringify(img));
                            });


                            $http.post('/ApiAddress/UploadAddressImage', fd, {
                                transformRequest: angular.identity,
                                headers: { 'Content-Type': undefined }
                            });
                        }
                    });

                    $fileInput.first().click();
                }
            }
        };


        vm.uploadFileReady = function ($event) {

            if ($event && $event.currentTarget) {

                var $fileInput = $($event.currentTarget);

                if ($fileInput && $fileInput.length > 0) {

                    $fileInput = $fileInput.parents('form.img-upload').first();

                    if ($fileInput && $fileInput.length > 0) {

                        $fileInput.submit();
                    }
                }
            }
        };



        vm.save = function () {

            param.save(vm.address);
        };

        vm.close = function () {

            if (vm.address && vm.address.id <= 0) {

                param.deleteModal(vm.address);
            }

            $uibModalInstance.close();
        };

        vm.addRatingOverride = function () {

            var newRecord = {
                id: 0,
                fromUtc: null,
                fromDate: new Date(),
                toUtc: null,
                toDate: null,
                value: 0,
                clickCount: 0,
                status: 1,
                recordState: 10,
                active: true
            };

            vm.address.ratingOverrides.insert(0, newRecord);
        };

        vm.setRatingToDelete = function (rt) {

            if (rt) {

                if (rt.recordState === 10) {

                    vm.address.ratingOverrides.remove(rt);
                }
                else {

                    rt.prevRecordState = rt.recordState; rt.recordState = 30;
                }
            }
        };

        $uibModalInstance.closed.then(function () {

            $('body').attr('style', '');
        })
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