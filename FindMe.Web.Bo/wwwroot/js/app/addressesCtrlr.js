
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('addressesCtrlr', ['$http', '$scope', '$sce', '$uibModal', 'appProps', 'headerConfigService', addressesCtrlrFunc]);

    function addressesCtrlrFunc($http, $scope, $sce, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Addresses;
        headerConfigService.showToolBar = true;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddAddr;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshAddrs;
        headerConfigService.saveBtnTltp = appProps.msg_SaveAddrs;

        var vm = this;

        vm.appProps = appProps;

        vm.clients = [];
        vm.parentCategorys = [];
        vm.categorys = [];
        vm.cityDetails = [];
        vm.tags = [];
        vm.featuredTypes = [
        {
            value: 10,
            name: appProps.lbl_HmPg.encodeHtml()
        },
        {
            value: 20,
            name: appProps.lbl_MenuAndSrchLstSide.encodeHtml()
        }];

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


        var checkRatinOvrdsRecordState = function (ratingOvrds) {

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

                            ratingOvrd.recordState = 10;
                        }
                    }
                }
            }

        };

        var checkFilesRecordState = function (files) {

            if (files) {


                if (!Array.isArray(files)) {

                    files = [files];
                }

                for (var i = 0; i < files.length; i++) {

                    var file = files[i];

                    file.status = file.active ? 1 : 0;

                    if (file.__comp
                        && (!file.recordState || file.recordState <= 0)) {

                        if (file.id > 0) {

                            if (file.desc !== file.__comp.desc
                                || file.name !== file.__comp.name
                                || file.isDefault !== file.__comp.isDefault
                                || file.alt !== file.__comp.alt
                                || file.active !== file.__comp.active
                                || file.waitingUpload) {

                                file.recordState = 20;
                            }
                            else {

                                file.recordState = 0;
                            }
                        }
                        else {

                            file.recordState = 10;
                        }
                    }
                }
            }
        };

        var checkOpenHoursRecordState = function (openHours) {

            if (openHours) {


                if (!Array.isArray(openHours)) {

                    openHours = [openHours];
                }

                var func = function (dayRow) {

                    if (dayRow) {

                        dayRow.status = dayRow.active ? 1 : 0;

                        if (dayRow.__comp
                            && (!dayRow.recordState || dayRow.recordState <= 0)) {

                            if (dayRow.id > 0) {

                                if (dayRow.hrFrom !== dayRow.__comp.hrFrom
                                    || dayRow.hrTo !== dayRow.__comp.hrTo
                                    || dayRow.minFrom !== dayRow.__comp.minFrom
                                    || dayRow.minTo !== dayRow.__comp.minTo
                                    || dayRow.active !== dayRow.__comp.active) {

                                    dayRow.recordState = 20;
                                }
                                else {

                                    dayRow.recordState = 0;
                                }
                            }
                            else {

                                dayRow.recordState = 10;
                            }
                        }
                    }
                };

                for (var i = 0; i < openHours.length; i++) {

                    var dayRow = openHours[i];

                    if (dayRow.openHours) {

                        $.each(dayRow.openHours, function (index, value) {

                            func(value);
                        });
                    }
                    else {

                        func(dayRow);
                    }
                }
            }
        };

        var checkTripAdWidgetRecordState = function (tripAd) {

            if (tripAd) {

                tripAd.status = tripAd.active ? 1 : 0;

                if (tripAd.__comp
                    && (!tripAd.recordState || tripAd.recordState <= 0)) {

                    if (tripAd.id > 0) {

                        if (tripAd.largeValue !== tripAd.__comp.largeValue
                            || tripAd.smallValue !== tripAd.__comp.smallValue) {

                            tripAd.recordState = 20;
                        }
                        else {

                            tripAd.recordState = 0;
                        }
                    }
                    else {

                        tripAd.recordState = 10;
                    }
                }
            }
        };

        var checkContactsRecordState = function (contacts) {

            if (contacts) {


                if (!Array.isArray(contacts)) {

                    contacts = [contacts];
                }

                var func = function (ctcRow) {

                    if (ctcRow) {

                        ctcRow.status = ctcRow.active ? 1 : 0;

                        if (ctcRow.__comp
                            && (!ctcRow.recordState || ctcRow.recordState <= 0)) {

                            if (ctcRow.id > 0) {

                                if (ctcRow.text !== ctcRow.__comp.text
                                    || ctcRow.link !== ctcRow.__comp.link
                                    || ctcRow.status !== ctcRow.__comp.status
                                    || ctcRow.active !== ctcRow.__comp.active) {

                                    ctcRow.recordState = 20;
                                }
                                else {

                                    ctcRow.recordState = 0;
                                }
                            }
                            else {

                                ctcRow.recordState = 10;
                            }
                        }
                    }
                };

                for (var i = 0; i < contacts.length; i++) {

                    var ctcRow = contacts[i];

                    if (ctcRow.contacts) {

                        $.each(ctcRow.contacts, function (index, value) {

                            func(value);
                        });
                    }
                    else {

                        func(ctcRow);
                    }
                }
            }
        };

        var checkFeaturedsRecordState = function (featureds) {

            if (featureds) {


                if (!Array.isArray(featureds)) {

                    featureds = [featureds];
                }

                for (var i = 0; i < featureds.length; i++) {

                    var ftrd = featureds[i];

                    if (ftrd.__comp
                        && (!ftrd.recordState || ftrd.recordState <= 0)) {

                        if (ftrd.id > 0) {

                            if (ftrd.fromDate !== ftrd.__comp.fromDate
                                || ftrd.toDate !== ftrd.__comp.toDate
                                || ftrd.type !== ftrd.__comp.type
                                || ftrd.fromUtc !== ftrd.__comp.fromUtc
                                || ftrd.toUtc !== ftrd.__comp.toUtc) {

                                ftrd.recordState = 20;
                            }
                            else {

                                ftrd.recordState = 0;
                            }
                        }
                        else {

                            ftrd.recordState = 10;
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
                            || address.desc_en !== compAddress.desc_en
                            || address.desc_fr !== compAddress.desc_fr
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

                checkRatinOvrdsRecordState(address.ratingOverrides);
                checkFilesRecordState(address.images);
                checkFilesRecordState(address.logos);
                checkFilesRecordState(address.documents);
                checkOpenHoursRecordState(address.openHours);
                checkTripAdWidgetRecordState(address.tripAdWidget);
                checkContactsRecordState(address.contacts);
                checkFeaturedsRecordState(address.featureds);
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

                if (vm.categorys.length > 0 && vm.parentCategorys.length > 0) {

                    tempName = $.grep(vm.categorys, function (v) {
                        return v.id === address.category_Id;
                    });

                    if (tempName && tempName.length > 0) {

                        address.parentCategory = tempName[0].parent;
                        address.category = tempName[0];
                    }
                    else {

                        tempName = $.grep(vm.parentCategorys, function (v) {
                            return v.id === address.category_Id;
                        });

                        if (tempName && tempName.length > 0) {

                            address.parentCategory = tempName[0];
                            address.category = null;
                        }
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
                            vmRoot: vm,
                            address: address,
                            save: vm.save,
                            deleteModal: vm.deleteModal,
                            checkRecordState: checkRecordState,
                            revertAddress: revert,
                            checkFilesRecordState: checkFilesRecordState,
                            checkOpenHoursRecordState: checkOpenHoursRecordState,
                            checkContactsRecordState: checkContactsRecordState,
                            checkTripAdWidgetRecordState: checkTripAdWidgetRecordState,
                            checkFeaturedsRecordState: checkFeaturedsRecordState,
                            clients: vm.clients,
                            parentCategorys: vm.parentCategorys,
                            categorys: vm.categorys,
                            cityDetails: vm.cityDetails,
                            tags: vm.tags,
                            featuredTypes: vm.featuredTypes
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
                        v.__comp = jQuery.extend(false, {}, v);
                    });
                }

                if (value.logos) {

                    $.each(value.logos, function (i, v) {

                        v.active = v.status === 1;
                        v.__comp = jQuery.extend(false, {}, v);
                    });
                }

                if (value.documents) {

                    $.each(value.documents, function (i, v) {

                        v.active = v.status === 1;
                        v.__comp = jQuery.extend(false, {}, v);
                    });
                }

                if (value.openHours) {

                    $.each(value.openHours, function (i, day) {

                        switch (day.day) {

                            case 10:
                                day.name = appProps.lbl_Monday;
                                break;
                            case 20:
                                day.name = appProps.lbl_Tuesday;
                                break;
                            case 30:
                                day.name = appProps.lbl_Wednesday;
                                break;
                            case 40:
                                day.name = appProps.lbl_Thursday;
                                break;
                            case 50:
                                day.name = appProps.lbl_Friday;
                                break;
                            case 60:
                                day.name = appProps.lbl_Saturday;
                                break;
                            case 70:
                                day.name = appProps.lbl_Sunday;
                                break;
                            case 80:
                                day.name = appProps.lbl_pHoliday;
                                break;
                        }

                        $.each(day.openHours, function (i, dayRow) {

                            dayRow.timeFrom = new Date(2000, 10, 10, dayRow.hrFrom, dayRow.minFrom);
                            dayRow.timeTo = new Date(2000, 10, 10, dayRow.hrTo, dayRow.minTo);

                            dayRow.active = dayRow.status === 1;
                            dayRow.__comp = jQuery.extend(false, {}, dayRow);
                        });
                    });
                }

                if (value.contacts) {

                    $.each(value.contacts, function (i, grp) {

                        $.each(grp.contacts, function (index, ctc) {

                            ctc.name = grp.name;
                            ctc.active = ctc.status === 1;
                            ctc.__comp = jQuery.extend(false, {}, ctc);
                        });
                    });
                }

                if (value.descs) {

                    $.each(value.descs, function (i, desc) {

                        if (desc && desc.lang_Code) {

                            var langCode = desc.lang_Code.toLowerCase();

                            if (langCode.startsWith('en')) {

                                value.desc_en = desc.value;
                            }
                            else if (langCode.startsWith('fr')) {

                                value.desc_fr = desc.value;
                            }
                        }
                    });

                    if (value.__comp) {

                        value.__comp.desc_en = value.desc_en;
                        value.__comp.desc_fr = value.desc_fr;
                    }

                    delete value.descs;
                }

                if (value.featureds) {

                    $.each(value.featureds, function (i, ftrd) {

                        ftrd.fromDate = new Date(ftrd.fromUtc);
                        ftrd.toDate = ftrd.toUtc ? new Date(ftrd.toUtc) : null;

                        var ftrdType = $.grep(vm.featuredTypes, function (v) {
                            return v.value === ftrd.type;
                        });

                        ftrd.typeObj = ftrdType.length > 0 ? ftrdType[0] : null;

                        ftrd.__comp = jQuery.extend(false, {}, ftrd);
                    });
                }

                if (!value.tripAdWidget) {

                    value.tripAdWidget = {

                        id: 0,
                        largeValue: '',
                        smallValue: '',
                        recordState: 0,
                        status: 1
                    };
                }

                value.tripAdWidget.__comp = jQuery.extend(false, {}, value.tripAdWidget);

                value.tripAdWidget.trustLargeValue = function () {

                    return $sce.trustAsHtml(value.tripAdWidget.largeValue);
                };

                value.tripAdWidget.trustSmallValue = function () {

                    return $sce.trustAsHtml(value.tripAdWidget.smallValue);
                };
            }
        };

        var duplicateContent = function (value) {

            if (value) {

                var temp, i, val;

                if (value.ratingOverrides) {

                    temp = value.ratingOverrides;

                    value.ratingOverrides = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.ratingOverrides.push(val);
                    }
                }

                if (value.tags) {

                    temp = value.tags;

                    value.tags = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.tags.push(val);
                    }
                }

                if (value.images) {

                    temp = value.images;

                    value.images = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.images.push(val);
                    }
                }

                if (value.logos) {

                    temp = value.logos;

                    value.logos = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.logos.push(val);
                    }
                }

                if (value.documents) {

                    temp = value.documents;

                    value.documents = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.documents.push(val);
                    }
                }

                if (value.openHours) {

                    temp = value.openHours;

                    value.openHours = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.openHours.push(val);
                    }
                }

                if (value.contacts) {

                    temp = value.contacts;

                    value.contacts = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;

                        $.each(val.contacts, function () {

                            delete this.__comp;
                        });

                        value.contacts.push(val);
                    }
                }

                if (value.featureds) {

                    temp = value.featureds;

                    value.featureds = [];

                    for (i = 0; i < temp.length; i++) {

                        val = jQuery.extend(true, {}, temp[i]);
                        delete val.__comp;
                        value.featureds.push(val);
                    }
                }

                if (value.tripAdWidget) {

                    value.tripAdWidget = jQuery.extend(true, {}, value.tripAdWidget);
                    delete value.tripAdWidget.__comp;
                    delete value.tripAdWidget.trustLargeValue;
                    delete value.tripAdWidget.trustSmallValue;
                }
            }

            return value;
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


        var prevNames = '';
        vm.populateAddresses = function (limit, offset, names) {

            var forceGetCount = false;

            if (names || prevNames) {

                forceGetCount = (names !== prevNames || vm.addressesCountMod !== 0);
            }
            else {

                forceGetCount = (vm.addressesCountMod !== 0);
            }

            prevNames = names;

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            names = !names ? vm.searchValue : names;

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

                        vm.parentCategorys.length = 0;
                        vm.categorys.length = 0;

                        $.each(resp.data.categorys, function (index, value) {

                            value.name = value.name.encodeHtml();

                            vm.parentCategorys.push(value);

                            if (value.children && value.children.length > 0) {

                                $.each(value.children, function (i, v) {

                                    v.parent = value;
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
                        name: (names ? names : null),
                        getRefClients: (vm.clients.length === 0),
                        getRefCatgs: (vm.categorys.length === 0),
                        getRefCities: (vm.cityDetails.length === 0)
                    })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        var revert = function (address) {

            if (address
                && address.__comp) {

                if (address.id > 0) {

                    var org = address.__comp;

                    address.name = org.name;
                    address.slug = org.slug;
                    address.client_Id = org.client_Id;
                    address.client = org.client;
                    address.cityDetail_Id = org.cityDetail_Id;
                    address.cityDetail = org.cityDetail;
                    address.category_Id = org.category_Id;
                    address.parentCategory = org.parentCategory;
                    address.category = org.category;
                    address.latitude = org.latitude;
                    address.longitude = org.longitude;
                    address.desc_en = org.desc_en;
                    address.desc_fr = org.desc_fr;
                    address.flgRecByFbFans = org.flgRecByFbFans;
                    address.rateOverride = org.rateOverride;
                    address.rateOverrideCount = org.rateOverrideCount;

                    address.status = org.status;
                    address.active = org.active;

                    address.inEditMode = false;
                }
                else {

                    vm.addresses.remove(address);
                }
            }
        };


        var uploadFiles = function (addresses, finallyCallback) {

            if (addresses) {

                if (!Array.isArray(addresses)) {

                    addresses = [addresses];
                }

                var saveSuccess = false;


                var successFunc = function (resp) {

                    saveSuccess = true;

                    if (resp && resp.data && resp.data.result) {

                        for (i = 0; i < resp.data.result.length; i++) {

                            var addr = resp.data.result[i];
                            var clientAddr = $.grep(vm.addresses, function (v) { return v.uid === addr.uid; })[0];

                            if (clientAddr) {

                                setUpContent(addr);

                                if (clientAddr.images && addr.images && addr.images.length > 0) {

                                    clientAddr.images.length = 0;

                                    for (i = 0; i < addr.images.length; i++) {

                                        clientAddr.images.push(addr.images[i]);
                                    }
                                }

                                if (clientAddr.logos && addr.logos && addr.logos.length > 0) {

                                    clientAddr.logos.length = 0;

                                    for (i = 0; i < addr.logos.length; i++) {

                                        clientAddr.logos.push(addr.logos[i]);
                                    }
                                }

                                if (clientAddr.documents && addr.documents && addr.documents.length > 0) {

                                    clientAddr.documents.length = 0;

                                    for (i = 0; i < addr.documents.length; i++) {

                                        clientAddr.documents.push(addr.documents[i]);
                                    }
                                }
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

                    for (var i = 0; i < addresses.length; i++) {

                        addresses[i].toBeUploadedFiles = [];
                    }

                    toggleGlblWaitVisibility(false);

                    if (finallyCallback) {

                        finallyCallback(saveSuccess);
                    }
                };


                var formData = new FormData();

                var hasFiles = false;

                for (var i = 0; i < addresses.length; i++) {

                    var addr = addresses[i];

                    if (addr.toBeUploadedFiles) {

                        formData.append('address', JSON.stringify({ id: addr.id, uid: addr.uid, clientUID: addr.clientUID }));

                        $.each(addr.toBeUploadedFiles, function (index, value) {

                            if (!value.recordState || value.recordState !== 30) {

                                hasFiles = true;

                                var key = addr.uid + '|' + value.value.type + '|' + index;

                                formData.append(key, value.file);

                                checkFilesRecordState(value.value);

                                formData.append(key, JSON.stringify(value.value));
                            }
                        });
                    }
                }

                if (hasFiles) {

                    $http.post(appProps.urlUploadAddressFiles, formData, {
                        transformRequest: angular.identity,
                        headers: { 'Content-Type': undefined }
                    })
                    .then(successFunc, errorFunc)
                    .finally(finallyFunc);
                }
                else {

                    saveSuccess = true;

                    finallyFunc();
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

                        var toBeSaved = duplicateContent(jQuery.extend(false, {}, address));

                        delete toBeSaved.__comp;


                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        toBeSaved.client_Id = toBeSaved.client ? toBeSaved.client.id : 0;
                        delete toBeSaved.client;

                        toBeSaved.category_Id = toBeSaved.category ? toBeSaved.category.id : (toBeSaved.parentCategory ? toBeSaved.parentCategory.id : 0);
                        delete toBeSaved.parentCategory;
                        delete toBeSaved.category;

                        toBeSaved.cityDetail_Id = toBeSaved.cityDetail ? toBeSaved.cityDetail.id : 0;
                        delete toBeSaved.cityDetail;

                        checkRecordState(toBeSaved, address.__comp, hasDeleteFlags ? deleteFlags[i] : false);

                        toBeSaved.status = toBeSaved.active ? 1 : 0;


                        if (toBeSaved.ratingOverrides) {

                            $.each(toBeSaved.ratingOverrides, function (i, v) {

                                v.fromUtc = appProps.swIsoFormat(v.fromDate, true);
                                v.toUtc = appProps.swIsoFormat(v.toDate, true);

                                delete v.fromDate;
                                delete v.toDate;
                            });
                        }

                        if (toBeSaved.featureds) {

                            $.each(toBeSaved.featureds, function (i, ftrd) {

                                ftrd.fromUtc = appProps.swIsoFormat(ftrd.fromDate, true);
                                ftrd.toUtc = appProps.swIsoFormat(ftrd.toDate, true);

                                ftrd.type = ftrd.typeObj ? ftrd.typeObj.value : 0;

                                delete ftrd.fromDate;
                                delete ftrd.toDate;
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

                                        tempValue.desc_en = value.desc_en;
                                        tempValue.desc_fr = value.desc_fr;

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

                                        if (value.images && tempValue.images) {

                                            tempValue.images.length = 0;

                                            for (i = 0; i < value.images.length; i++) {

                                                tempValue.images.push(value.images[i]);
                                            }
                                        }

                                        if (value.logos && tempValue.logos) {

                                            tempValue.logos.length = 0;

                                            for (i = 0; i < value.logos.length; i++) {

                                                tempValue.logos.push(value.logos[i]);
                                            }
                                        }

                                        if (value.documents && tempValue.documents) {

                                            tempValue.documents.length = 0;

                                            for (i = 0; i < value.documents.length; i++) {

                                                tempValue.documents.push(value.documents[i]);
                                            }
                                        }

                                        if (value.openHours && tempValue.openHours) {

                                            tempValue.openHours.length = 0;

                                            for (i = 0; i < value.openHours.length; i++) {

                                                tempValue.openHours.push(value.openHours[i]);
                                            }
                                        }

                                        if (value.contacts && tempValue.contacts) {

                                            tempValue.contacts.length = 0;

                                            for (i = 0; i < value.contacts.length; i++) {

                                                tempValue.contacts.push(value.contacts[i]);
                                            }
                                        }

                                        if (value.featureds && tempValue.featureds) {

                                            tempValue.featureds.length = 0;

                                            for (i = 0; i < value.featureds.length; i++) {

                                                tempValue.featureds.push(value.featureds[i]);
                                            }
                                        }

                                        if (tempValue.tripAdWidget) {

                                            if (value.tripAdWidget) {

                                                tempValue.tripAdWidget.status = value.tripAdWidget.status;
                                                tempValue.tripAdWidget.uid = value.tripAdWidget.uid;
                                                tempValue.tripAdWidget.id = value.tripAdWidget.id;
                                                tempValue.tripAdWidget.largeValue = value.tripAdWidget.largeValue;
                                                tempValue.tripAdWidget.smallValue = value.tripAdWidget.smallValue;
                                            }
                                            else {

                                                tempValue.tripAdWidget.status = 1;
                                                tempValue.tripAdWidget.uid = null;
                                                tempValue.tripAdWidget.id = 0;
                                                tempValue.tripAdWidget.largeValue = null;
                                                tempValue.tripAdWidget.smallValue = null;
                                            }

                                            tempValue.tripAdWidget.recordState = 0;
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

                        if (saveSuccess && validAddresses.length > 0) {

                            uploadFiles(validAddresses, finallyCallback);
                        }
                        else {

                            toggleGlblWaitVisibility(false);

                            if (finallyCallback) {

                                finallyCallback(saveSuccess);
                            }
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

                var activePg = $.grep(vm.pgsCollection, function (pg) { return pg.isActive; });

                if (activePg && Array.isArray(activePg)) {

                    activePg = activePg.length > 0 ? activePg[0] : null;
                }

                if (activePg) {

                    var offset = (appProps.resultItemsPerPg * activePg.index) - 1;

                    $scope.$apply(function () {

                        vm.populateAddresses(appProps.resultItemsPerPg, offset);
                    });
                }
            }
            else if ($btn.hasClass('add')) {

                $scope.$apply(function () {

                    var newItem = {
                        id: 0,
                        name: '',
                        slug: '',
                        client_Id: 0,
                        client: null,
                        cityDetail_Id: 0,
                        cityDetail: null,
                        category_Id: 0,
                        parentCategory: null,
                        category: null,
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
        .controller('addressEditorInstanceCtrlr', ['$http', '$scope', '$sce', '$uibModalInstance', 'appProps', 'miscToolsService', 'param', addressEditorInstanceCtrlrFunc]);

    function addressEditorInstanceCtrlrFunc($http, $scope, $sce, $uibModalInstance, appProps, miscToolsService, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.vmRoot = param.vmRoot;

        vm.address = param.address;

        vm.clients = param.clients;
        vm.parentCategorys = param.parentCategorys;
        vm.categorys = param.categorys;
        vm.cityDetails = param.cityDetails;
        vm.tags = param.tags;
        vm.featuredTypes = param.featuredTypes;

        vm.checkFilesRecordState = param.checkFilesRecordState;
        vm.checkOpenHoursRecordState = param.checkOpenHoursRecordState;
        vm.checkContactsRecordState = param.checkContactsRecordState;

        vm.checkTripAdWidget = function (tripAd) {

            if (tripAd) {

                if (tripAd.largeValue) {

                    tripAd.largeValue = tripAd.largeValue.replaceAll('"', "'");
                }

                if (tripAd.smallValue) {

                    tripAd.smallValue = tripAd.smallValue.replaceAll('"', "'");
                }

                param.checkTripAdWidgetRecordState(tripAd)
            }
        }

        vm.tagFilter = '';

        vm.slugify = function () {

            if (vm.address) {

                if (!vm.address.slug) {

                    vm.address.slug = miscToolsService.slugify(vm.address.name);
                }
            }
        }

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

        vm.tripadCode = true;
        vm.tripadPreview = false;

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


        var removeToBeUploadedFile = function (file) {

            if (file && vm.address && vm.address.toBeUploadedFiles && vm.address.toBeUploadedFiles.length > 0) {

                $.each(
                    $.grep(vm.address.toBeUploadedFiles, function (v) {
                        return v.value === file;
                    }),
                    function (index, value) {

                        vm.address.toBeUploadedFiles.remove(value);
                    });
            }
        };

        vm.revertFile = function (file) {

            if (file && file.__comp) {

                file.name = file.__comp.name;
                file.isDefault = file.__comp.isDefault;
                file.format = file.__comp.format;
                file.alt = file.__comp.alt;
                file.desc = file.__comp.desc;
                file.active = file.__comp.active;

                file.recordState = 0;

                file.waitingUpload = false;

                removeToBeUploadedFile(file);
            }
        }
        vm.uploadImgClick = function ($event, file) {

            if (file) {

                if (file.waitingUpload) {

                    if (file.__comp) {

                        file.name = file.__comp.name;
                        file.format = file.__comp.format;
                    }
                    else {

                        file.name = '';
                        file.format = '';

                        var $localImg = $($event.currentTarget).parents('tr').find('img.local').first();

                        if ($localImg && $localImg.length > 0) {

                            $localImg.attr('src', '');
                        }
                    }

                    file.waitingUpload = false;

                    if (file.recordStateBeforeUpload) {

                        file.recordState = file.recordStateBeforeUpload;
                    }
                    else {

                        file.recordState = 0;
                    }

                    removeToBeUploadedFile(file);
                }
                else {

                    if ($event && $event.currentTarget) {

                        var $fileInput = $($event.currentTarget).siblings('input[type="file"]');

                        if ($fileInput && $fileInput.length > 0) {

                            $fileInput.off('change');

                            $fileInput.on('change', function () {

                                $scope.$apply(function () {

                                    if (vm.address) {

                                        if (!vm.address.toBeUploadedFiles) {

                                            vm.address.toBeUploadedFiles = [];
                                        }


                                        var files = $fileInput.get(0).files;

                                        if (files && files.length > 0) {

                                            $.each(files, function (index, value) {

                                                var name = null;
                                                var format = null;

                                                if (value.name) {

                                                    format = value.name.split('.');

                                                    if (format && format.length > 0) {

                                                        name = String(format[0]).toLowerCase();
                                                        format = String(format[format.length - 1]).toLowerCase();
                                                    }
                                                    else {

                                                        name = null;
                                                        format = null;
                                                    }
                                                }

                                                file.name = name;
                                                file.format = format;

                                                vm.address.toBeUploadedFiles.push({
                                                    file: value,
                                                    key: value.name + '_' + index + '_KEY',
                                                    value: file
                                                });
                                            });


                                            var $localImg = $fileInput.parents('tr').first().find('td img.local');

                                            if ($localImg && $localImg.length > 0) {

                                                var reader = new FileReader();

                                                reader.onload = function (e) {

                                                    $localImg.attr('src', e.target.result);
                                                }

                                                reader.readAsDataURL(files[0]);
                                            }
                                        }

                                        file.waitingUpload = true;
                                        file.recordStateBeforeUpload = file.recordState;
                                        vm.checkFilesRecordState(file);
                                    }
                                });
                            });

                            $fileInput.first().click();
                        }
                    }
                }
            }
        };
        vm.fileSetToDefault = function (file, fileCollection) {

            if (file && fileCollection && fileCollection.length > 0) {

                $.each($.grep(fileCollection, function (v) { return v.isDefault === true; }), function (index, value) {

                    value.isDefault = false;
                });

                file.isDefault = true;
            }

            vm.checkFilesRecordState(file)
        };


        vm.revertContact = function (ctcRow) {

            if (ctcRow && ctcRow.__comp) {

                ctcRow.text = ctcRow.__comp.text;
                ctcRow.link = ctcRow.__comp.link;

                ctcRow.status = ctcRow.__comp.status;
                ctcRow.active = ctcRow.__comp.active;

                ctcRow.recordState = 0;
            }

            vm.checkContactsRecordState(ctcRow);
        };
        vm.contactValueChanged = function (ctcRow) {

            vm.checkContactsRecordState(ctcRow);
        };


        vm.revertDayRow = function (dayRow) {

            if (dayRow && dayRow.__comp) {

                dayRow.timeFrom = dayRow.__comp.timeFrom;
                dayRow.timeTo = dayRow.__comp.timeTo;
                dayRow.hrFrom = dayRow.__comp.hrFrom;
                dayRow.hrTo = dayRow.__comp.hrTo;
                dayRow.minFrom = dayRow.__comp.minFrom;
                dayRow.minTo = dayRow.__comp.minTo;

                dayRow.active = dayRow.__comp.active;

                dayRow.recordState = 0;

                dayRow.waitingUpload = false;
            }

            vm.checkOpenHoursRecordState(dayRow);
        };
        vm.timeFromChanged = function (dayRow) {

            if (dayRow) {
                if (dayRow.timeFrom) {

                    dayRow.hrFrom = dayRow.timeFrom.getHours();
                    dayRow.minFrom = dayRow.timeFrom.getMinutes();
                }
                else {

                    dayRow.hrFrom = 0;
                    dayRow.minFrom = 0;
                }
            }

            vm.checkOpenHoursRecordState(dayRow);
        };
        vm.timeToChanged = function (dayRow) {

            if (dayRow) {
                if (dayRow.timeTo) {

                    dayRow.hrTo = dayRow.timeTo.getHours();
                    dayRow.minTo = dayRow.timeTo.getMinutes();
                }
                else {

                    dayRow.hrTo = 0;
                    dayRow.minTo = 0;
                }
            }
        };

        vm.revertTripAd = function () {

            if (vm.address && vm.address.tripAdWidget && vm.address.tripAdWidget.__comp) {

                vm.address.tripAdWidget.smallValue = vm.address.tripAdWidget.__comp.smallValue;
                vm.address.tripAdWidget.largeValue = vm.address.tripAdWidget.__comp.largeValue;


                vm.address.tripAdWidget.recordState = 0;
            }
        }
        vm.deleteTripAd = function () {

            if (vm.address && vm.address.tripAdWidget) {

                vm.address.tripAdWidget.smallValue = null;
                vm.address.tripAdWidget.largeValue = null;

                if (vm.address.tripAdWidget.recordState
                    && vm.address.tripAdWidget.recordState === 10) {

                    vm.address.tripAdWidget.recordState = 0;
                }
                else {

                    vm.address.tripAdWidget.recordState = 30;
                }
            }
        }

        vm.save = function () {

            param.save(vm.address);
        };

        vm.close = function () {

            if (vm.address) {

                if (vm.address.id > 0) {

                    param.revertAddress(vm.address);
                }
                else {

                    param.deleteModal(vm.address);
                }
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

        vm.addFeatured = function () {

            var newRecord = {
                id: 0,
                fromUtc: null,
                fromDate: new Date(),
                toUtc: null,
                toDate: null,
                status: 1,
                recordState: 10,
                active: true
            };

            vm.address.featureds.insert(0, newRecord);
        };

        vm.addLogo = function () {

            var newRecord = {
                recordState: 10,
                id: 0,
                type: 50,
                isDefault: $.grep(vm.address.logos, function (v) { return v.isDefault === true; }).length <= 0,
                uid: null,
                alt: null,
                desc: null,
                status: 1,
                active: true
            };

            vm.address.logos.insert(0, newRecord);
        };

        vm.addImage = function () {

            var newRecord = {
                recordState: 10,
                id: 0,
                type: 10,
                isDefault: $.grep(vm.address.images, function (v) { return v.isDefault === true; }).length <= 0,
                uid: null,
                alt: null,
                desc: null,
                status: 1,
                active: true
            };

            vm.address.images.insert(0, newRecord);
        };

        vm.addDoc = function () {

            var newRecord = {
                recordState: 10,
                id: 0,
                type: 0,
                isDefault: $.grep(vm.address.documents, function (v) { return v.isDefault === true; }).length <= 0,
                uid: null,
                alt: null,
                desc: null,
                status: 1,
                active: true
            };

            vm.address.documents.insert(0, newRecord);
        };

        vm.addOpenHours = function (day) {

            if (day && day.openHours) {

                var newRecord = {
                    recordState: 10,
                    id: 0,
                    hrFrom: null,
                    hrTo: null,
                    minFrom: null,
                    minTo: null,
                    status: 1,
                    active: true
                };

                day.openHours.insert(0, newRecord);
            }
        };

        vm.addContact = function (ctcGrp) {

            if (ctcGrp && ctcGrp.contacts) {

                var newRecord = {
                    recordState: 10,
                    id: 0,
                    name: ctcGrp.name,
                    link: null,
                    text: null,
                    status: 1,
                    active: true
                };

                ctcGrp.contacts.insert(0, newRecord);
            }
        };

        vm.markRowForDeletion = function (row, rowCollection) {

            if (row && rowCollection) {

                if (row.recordState === 10) {

                    rowCollection.remove(row);
                }
                else if (row.recordState === 30) {

                    row.recordState = row.recordStateBeforeDelete;
                }
                else {

                    row.recordStateBeforeDelete = row.recordState;
                    row.recordState = 30;
                }
            }
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

        vm.setFeaturedToDelete = function (ftrd) {

            if (ftrd) {

                if (ftrd.recordState === 10) {

                    vm.address.featureds.remove(ftrd);
                }
                else {

                    ftrd.prevRecordState = ftrd.recordState;
                    ftrd.recordState = 30;
                }
            }
        };

        vm.revertFeatured = function (ftrd) {

            if (ftrd && ftrd.__comp) {

                ftrd.fromDate = ftrd.__comp.fromDate;
                ftrd.toDate = ftrd.__comp.toDate;
                ftrd.type = ftrd.__comp.type;
                ftrd.fromUtc = ftrd.__comp.fromUtc;
                ftrd.toUtc = ftrd.__comp.toUtc;

                ftrd.status = ftrd.__comp.status;
                ftrd.active = ftrd.__comp.active;

                ftrd.recordState = 0;
            }

            param.checkFeaturedsRecordState(ftrd);
        };

        vm.featuredChanged = function (ftrd) {

            param.checkFeaturedsRecordState(ftrd);
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