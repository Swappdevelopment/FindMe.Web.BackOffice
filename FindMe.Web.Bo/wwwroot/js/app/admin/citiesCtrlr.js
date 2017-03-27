
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('citiesCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', citysCtrlrFunc]);

    function citysCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = $('<textarea>' + appProps.lbl_CtsRgnsDstrctsGrps + '</textarea>').text();
        headerConfigService.showToolBar = true;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddClnts;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshClnts;
        headerConfigService.saveBtnTltp = appProps.msg_SaveClnts;


        var vm = this;

        vm.appProps = appProps;

        vm.tabIndex = 0;



        var tabIndexChanged = function (newIndex, oldIndex) {

            if (newIndex !== oldIndex) {

                switch (newIndex) {

                    case 0:

                        break;

                    case 1:

                        if (vm.rVm) {

                            if (!vm.rVm.regions || vm.rVm.regions.length == 0) {

                                if (vm.rVm.populateRegions) {

                                    vm.rVm.populateRegions();
                                }
                            }
                        }
                        break;

                    case 2:

                        if (vm.dVm) {

                            if (!vm.dVm.districts || vm.dVm.districts.length == 0) {

                                if (vm.dVm.populateDistricts) {

                                    vm.dVm.populateDistricts();
                                }
                            }
                        }
                        break;

                    case 3:

                        if (vm.gVm) {

                            if (!vm.gVm.cityGroups || vm.gVm.cityGroups.length == 0) {

                                if (vm.gVm.populateCityGroups) {

                                    vm.gVm.populateCityGroups();
                                }
                            }
                        }
                        break;
                }
            }
        };

        $scope.$watch('vm.tabIndex', tabIndexChanged);


        tabIndex0CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, vm);
        tabIndex1CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, vm);
        tabIndex2CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, vm);
        tabIndex3CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, vm);





        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                switch (vm.tabIndex) {

                    case 0:

                        if (vm.cVm && vm.cVm.populateRegions) {

                            vm.cVm.populateCitys();
                        }
                        break;

                    case 1:

                        if (vm.rVm && vm.rVm.populateRegions) {

                            vm.rVm.populateRegions();
                        }
                        break;

                    case 2:

                        if (vm.dVm && vm.dVm.populateDistricts) {

                            vm.dVm.populateDistricts();
                        }
                        break;

                    case 3:

                        if (vm.gVm && vm.gVm.populateCityGroups) {

                            vm.gVm.populateCityGroups();
                        }
                        break;
                }
            }
            else if ($btn.hasClass('add')) {

                $scope.$apply(function () {

                    switch (vm.tabIndex) {

                        case 0:

                            if (vm.cVm && vm.cVm.addCity) {

                                vm.cVm.addCity();
                            }
                            break;

                        case 1:

                            if (vm.rVm && vm.rVm.addRegion) {

                                vm.rVm.addRegion();
                            }
                            break;

                        case 2:

                            if (vm.dVm && vm.dVm.addDistrict) {

                                vm.dVm.addDistrict();
                            }
                            break;

                        case 3:

                            if (vm.gVm && vm.gVm.addCityGroup) {

                                vm.gVm.addCityGroup();
                            }
                            break;
                    }
                });
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);

        $('#searchBar').on('searchGo', function (e, arg) {

            if (arg && arg.searchValue) {

                $scope.$apply(function () {

                    vm.searchValue = arg.searchValue;
                    vm.populateCitys(appProps.resultItemsPerPg, 0, arg.searchValue);
                });
            }
        });
        $('#searchBar').on('searchClear', function (e, arg) {

            $scope.$apply(function () {

                vm.searchValue = '';
                vm.populateCitys(appProps.resultItemsPerPg, 0);
            });
        });
    }


    function tabIndex0CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, viewModel) {

        viewModel.cVm = {};

        var vm = viewModel.cVm;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.citysCount = 0;

        vm.citys = [];

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateCitys(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (city) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteCityModal.html',
                controller: 'deleteCityInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#citysVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            city: city,
                            save: vm.save
                        };
                    }
                }
            });
        };


        vm.openModal = function (city) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'cityModal.html',
                controller: 'cityInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#citysVw .modal-container'),
                resolve: {
                    city: function () {

                        var copy = jQuery.extend(true, {}, city);

                        return copy;
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



        var checkRecordState = function (city, compCity, toBeDeleted) {

            if (city
                && compCity) {

                if (city.id > 0) {

                    if (toBeDeleted) {

                        city.recordState = 30;
                    }
                    else if (city.legalName !== compCity.legalName
                            || city.civility !== compCity.civility
                            || city.lName !== compCity.lName
                            || city.fName !== compCity.fName
                            || city.paid !== compCity.paid
                            || city.active !== compCity.active) {

                        city.recordState = 20;
                    }
                    else {

                        city.recordState = 0;
                    }
                }
                else {

                    city.recordState = 10;
                }
            }
        };


        var prevAllNames = '';
        vm.populateCitys = function (limit, offset, allNames) {

            var forceGetCount = false;

            if (allNames || prevAllNames) {

                forceGetCount = (allNames != prevAllNames);
            }

            prevAllNames = allNames;

            viewModel.showError = false;
            viewModel.errorstatus = '';
            viewModel.errormsg = '';
            viewModel.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            allNames = !allNames ? vm.searchValue : allNames;

            var successFunc = function (resp) {

                vm.citys.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0
                        || (resp.data.count == 0 && resp.data.result && resp.data.result.length == 0)) {

                        vm.citysCount = resp.data.count;

                        var ttlPgs = parseInt(vm.citysCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.citysCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var city = resp.data.result[i];

                            city.__comp = jQuery.extend(true, {}, city);

                            vm.citys.push(city);

                            city.inEditMode = false;
                            city.saving = false;
                        }
                    }
                }
            };

            var errorFunc = function (error) {

                if (error.data
                    && checkRedirectForSignIn(error.data)) {

                    viewModel.errorstatus = error.status + ' - ' + error.statusText;
                    viewModel.errormsg = error.data.msg;
                    viewModel.errorid = error.data.id;
                    viewModel.showError = true;
                }
            };

            var finallyFunc = function () {

                toggleGlblWaitVisibility(false);
            };

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlGetCitys, { limit: limit, offset: offset, getTotalClients: (vm.citysCount <= 0 || forceGetCount), allNames: (allNames ? allNames : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (city) {

            if (city
                && city.__comp) {

                if (city.id > 0) {

                    var org = city.__comp;

                    city.legalName = org.legalName;
                    city.civility = org.civility;
                    city.lName = org.lName;
                    city.fName = org.fName;
                    city.paid = org.paid;
                    city.active = org.active;

                    city.inEditMode = false;
                }
                else {

                    vm.citys.remove(city);
                }
            }
        };


        vm.save = function (citys, finallyCallback, deleteFlags) {

            if (citys) {

                if (!Array.isArray(citys)) {

                    citys = [citys];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validCitys = [];
                var toBeSavedCitys = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length == citys.length);

                for (var i = 0; i < citys.length; i++) {

                    var city = citys[i];

                    if (city
                        && city.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, city);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        checkRecordState(toBeSaved, city.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedCitys.push(toBeSaved);

                        city.saving = true;
                        validCitys.push(city);
                    }
                }

                if (validCitys.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validCitys.length == resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validCitys[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.legalName = value.legalName;
                                        tempValue.civility = value.civility;
                                        tempValue.lName = value.lName;
                                        tempValue.fName = value.fName;
                                        tempValue.paid = value.paid;
                                        tempValue.active = value.active;

                                        if (toBeSavedCitys[index].recordState == 10) {

                                            vm.citysCount += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.citys.remove(tempValue);

                                        vm.citysCount -= 1;

                                        if (vm.citysCount <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }
                                });
                            }
                        }
                    };

                    var errorFunc = function (error) {

                        if (error.data
                            && checkRedirectForSignIn(error.data)) {

                            viewModel.errorstatus = error.status + ' - ' + error.statusText;
                            viewModel.errormsg = error.data.msg;
                            viewModel.errorid = error.data.id;
                            viewModel.showError = true;
                        }
                    };

                    var finallyFunc = function () {

                        for (var i = 0; i < validCitys.length; i++) {

                            validCitys[i].saving = false;
                            validCitys[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    viewModel.showError = false;
                    viewModel.errorstatus = '';
                    viewModel.errormsg = '';
                    viewModel.errorid = 0;

                    $http.post(appProps.urlSaveCitys, { citys: toBeSavedCitys })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        vm.populateCitys();
    }

    function tabIndex1CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, viewModel) {

        viewModel.rVm = {};

        var vm = viewModel.rVm;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.regionsCount = 0;

        vm.regions = [];
        vm.defCountry = null;

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateRegions(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (region) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteObjectModal.html',
                controller: 'deleteObjectInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#citiesVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            target: region,
                            targetName: appProps.currentLang.startsWith('fr') ? region.name_fr : region.name_en,
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


        var checkRecordState = function (region, compRegion, toBeDeleted) {

            if (region
                && compRegion) {

                if (region.id > 0) {

                    if (toBeDeleted) {

                        region.recordState = 30;
                    }
                    else if (region.name_en !== compRegion.name_en
                            || region.name_fr !== compRegion.name_fr
                            || region.active !== compRegion.active) {

                        region.recordState = 20;
                    }
                    else {

                        region.recordState = 0;
                    }
                }
                else {

                    region.recordState = 10;
                }
            }
        };


        var prevAllNames = '';
        vm.populateRegions = function (limit, offset, name) {

            var forceGetCount = false;

            if (name || prevAllNames) {

                forceGetCount = (name != prevAllNames);
            }

            prevAllNames = name;

            viewModel.showError = false;
            viewModel.errorstatus = '';
            viewModel.errormsg = '';
            viewModel.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            name = !name ? vm.searchValue : name;

            var successFunc = function (resp) {

                vm.regions.length = 0;

                if (resp.data) {

                    vm.defCountry = resp.data.defCountry;

                    if (resp.data.count > 0
                        || (resp.data.count == 0 && resp.data.result && resp.data.result.length == 0)) {

                        vm.regionsCount = resp.data.count;

                        var ttlPgs = parseInt(vm.regionsCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.regionsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var region = resp.data.result[i];

                            region.__comp = jQuery.extend(true, {}, region);

                            vm.regions.push(region);

                            region.inEditMode = false;
                            region.saving = false;
                        }
                    }
                }
            };

            var errorFunc = function (error) {

                if (error.data
                    && checkRedirectForSignIn(error.data)) {

                    viewModel.errorstatus = error.status + ' - ' + error.statusText;
                    viewModel.errormsg = error.data.msg;
                    viewModel.errorid = error.data.id;
                    viewModel.showError = true;
                }
            };

            var finallyFunc = function () {

                toggleGlblWaitVisibility(false);
            };

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlGetRegions, { limit: limit, offset: offset, getTotalCatgs: (vm.regionsCount <= 0 || forceGetCount), allNames: (name ? name : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (region) {

            if (region
                && region.__comp) {

                if (region.id > 0) {

                    var org = region.__comp;

                    region.name_en = org.name_en;
                    region.name_fr = org.name_fr;
                    region.seqn = org.seqn;
                    region.active = org.active;

                    region.inEditMode = false;
                }
                else {

                    vm.regions.remove(region);
                }
            }
        };


        vm.addRegion = function () {

            var newItem = {
                id: 0,
                country_Id: vm.defCountry ? vm.defCountry.id : 0,
                name_en: '',
                name_fr: '',
                seqn: 0,
                active: true
            };

            newItem.__comp = jQuery.extend(true, {}, newItem);

            newItem.inEditMode = true;
            newItem.saving = false;

            vm.regions.insert(0, newItem);
        };


        vm.save = function (regions, finallyCallback, deleteFlags) {

            if (regions) {

                if (!Array.isArray(regions)) {

                    regions = [regions];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validRegions = [];
                var toBeSavedRegions = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length == regions.length);

                for (var i = 0; i < regions.length; i++) {

                    var region = regions[i];

                    if (region
                        && region.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, region);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        checkRecordState(toBeSaved, region.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedRegions.push(toBeSaved);

                        region.saving = true;
                        validRegions.push(region);
                    }
                }

                if (validRegions.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validRegions.length == resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validRegions[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.legalName = value.legalName;
                                        tempValue.civility = value.civility;
                                        tempValue.lName = value.lName;
                                        tempValue.fName = value.fName;
                                        tempValue.paid = value.paid;
                                        tempValue.active = value.active;

                                        if (toBeSavedRegions[index].recordState == 10) {

                                            vm.regionsCount += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.regions.remove(tempValue);

                                        vm.regionsCount -= 1;

                                        if (vm.regionsCount <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }
                                });
                            }
                        }
                    };

                    var errorFunc = function (error) {

                        if (error.data
                            && checkRedirectForSignIn(error.data)) {

                            viewModel.errorstatus = error.status + ' - ' + error.statusText;
                            viewModel.errormsg = error.data.msg;
                            viewModel.errorid = error.data.id;
                            viewModel.showError = true;
                        }
                    };

                    var finallyFunc = function () {

                        for (var i = 0; i < validRegions.length; i++) {

                            validRegions[i].saving = false;
                            validRegions[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    viewModel.showError = false;
                    viewModel.errorstatus = '';
                    viewModel.errormsg = '';
                    viewModel.errorid = 0;

                    $http.post(appProps.urlSaveRegions, { regions: toBeSavedRegions })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };
    }

    function tabIndex2CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, viewModel) {

        viewModel.dVm = {};

        var vm = viewModel.dVm;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.districtsCount = 0;

        vm.districts = [];
        vm.defCountry = null;

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateDistricts(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (district) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteObjectModal.html',
                controller: 'deleteObjectInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#citiesVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            target: district,
                            targetName: district.name,
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


        var checkRecordState = function (district, compDistrict, toBeDeleted) {

            if (district
                && compDistrict) {

                if (district.id > 0) {

                    if (toBeDeleted) {

                        district.recordState = 30;
                    }
                    else if (district.name !== compDistrict.name
                            || district.active !== compDistrict.active) {

                        district.recordState = 20;
                    }
                    else {

                        district.recordState = 0;
                    }
                }
                else {

                    district.recordState = 10;
                }
            }
        };


        var prevAllNames = '';
        vm.populateDistricts = function (limit, offset, name) {

            var forceGetCount = false;

            if (name || prevAllNames) {

                forceGetCount = (name != prevAllNames);
            }

            prevAllNames = name;

            viewModel.showError = false;
            viewModel.errorstatus = '';
            viewModel.errormsg = '';
            viewModel.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            name = !name ? vm.searchValue : name;

            var successFunc = function (resp) {

                vm.districts.length = 0;

                if (resp.data) {

                    vm.defCountry = resp.data.defCountry;

                    if (resp.data.count > 0
                        || (resp.data.count == 0 && resp.data.result && resp.data.result.length == 0)) {

                        vm.districtsCount = resp.data.count;

                        var ttlPgs = parseInt(vm.districtsCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.districtsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var district = resp.data.result[i];

                            district.__comp = jQuery.extend(true, {}, district);

                            vm.districts.push(district);

                            district.inEditMode = false;
                            district.saving = false;
                        }
                    }
                }
            };

            var errorFunc = function (error) {

                if (error.data
                    && checkRedirectForSignIn(error.data)) {

                    viewModel.errorstatus = error.status + ' - ' + error.statusText;
                    viewModel.errormsg = error.data.msg;
                    viewModel.errorid = error.data.id;
                    viewModel.showError = true;
                }
            };

            var finallyFunc = function () {

                toggleGlblWaitVisibility(false);
            };

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlGetDistricts, { limit: limit, offset: offset, getTotalDistricts: (vm.districtsCount <= 0 || forceGetCount), allNames: (name ? name : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (district) {

            if (district
                && district.__comp) {

                if (district.id > 0) {

                    var org = district.__comp;

                    district.name = org.name;
                    district.active = org.active;

                    district.inEditMode = false;
                }
                else {

                    vm.districts.remove(district);
                }
            }
        };


        vm.addDistrict = function () {

            var newItem = {
                id: 0,
                country_Id: vm.defCountry ? vm.defCountry.id : 0,
                name: '',
                seqn: 0,
                active: true
            };

            newItem.__comp = jQuery.extend(true, {}, newItem);

            newItem.inEditMode = true;
            newItem.saving = false;

            vm.districts.insert(0, newItem);
        };


        vm.save = function (districts, finallyCallback, deleteFlags) {

            if (districts) {

                if (!Array.isArray(districts)) {

                    districts = [districts];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validDistricts = [];
                var toBeSavedDistricts = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length == districts.length);

                for (var i = 0; i < districts.length; i++) {

                    var district = districts[i];

                    if (district
                        && district.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, district);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        checkRecordState(toBeSaved, district.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedDistricts.push(toBeSaved);

                        district.saving = true;
                        validDistricts.push(district);
                    }
                }

                if (validDistricts.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validDistricts.length == resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validDistricts[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.seqn = value.seqn;
                                        tempValue.name = value.name;
                                        tempValue.country_Id = value.country_Id;
                                        tempValue.active = value.active;

                                        if (toBeSavedDistricts[index].recordState == 10) {

                                            vm.districtsCount += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.districts.remove(tempValue);

                                        vm.districtsCount -= 1;

                                        if (vm.districtsCount <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }
                                });
                            }
                        }
                    };

                    var errorFunc = function (error) {

                        if (error.data
                            && checkRedirectForSignIn(error.data)) {

                            viewModel.errorstatus = error.status + ' - ' + error.statusText;
                            viewModel.errormsg = error.data.msg;
                            viewModel.errorid = error.data.id;
                            viewModel.showError = true;
                        }
                    };

                    var finallyFunc = function () {

                        for (var i = 0; i < validDistricts.length; i++) {

                            validDistricts[i].saving = false;
                            validDistricts[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    viewModel.showError = false;
                    viewModel.errorstatus = '';
                    viewModel.errormsg = '';
                    viewModel.errorid = 0;

                    $http.post(appProps.urlSaveDistricts, { districts: toBeSavedDistricts })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };
    }

    function tabIndex3CtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, viewModel) {

        viewModel.gVm = {};

        var vm = viewModel.gVm;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.cityGroupsCount = 0;

        vm.cityGroups = [];
        vm.defCountry = null;

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateCityGroups(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (cityGroup) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteObjectModal.html',
                controller: 'deleteObjectInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#citiesVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            target: cityGroup,
                            targetName: cityGroup.name,
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


        var checkRecordState = function (cityGroup, compCityGroup, toBeDeleted) {

            if (cityGroup
                && compCityGroup) {

                if (cityGroup.id > 0) {

                    if (toBeDeleted) {

                        cityGroup.recordState = 30;
                    }
                    else if (cityGroup.name !== compCityGroup.name
                            || cityGroup.active !== compCityGroup.active) {

                        cityGroup.recordState = 20;
                    }
                    else {

                        cityGroup.recordState = 0;
                    }
                }
                else {

                    cityGroup.recordState = 10;
                }
            }
        };


        var prevAllNames = '';
        vm.populateCityGroups = function (limit, offset, name) {

            var forceGetCount = false;

            if (name || prevAllNames) {

                forceGetCount = (name != prevAllNames);
            }

            prevAllNames = name;

            viewModel.showError = false;
            viewModel.errorstatus = '';
            viewModel.errormsg = '';
            viewModel.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            name = !name ? vm.searchValue : name;

            var successFunc = function (resp) {

                vm.cityGroups.length = 0;

                if (resp.data) {

                    vm.defCountry = resp.data.defCountry;

                    if (resp.data.count > 0
                        || (resp.data.count == 0 && resp.data.result && resp.data.result.length == 0)) {

                        vm.cityGroupsCount = resp.data.count;

                        var ttlPgs = parseInt(vm.cityGroupsCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.cityGroupsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var cityGroup = resp.data.result[i];

                            cityGroup.__comp = jQuery.extend(true, {}, cityGroup);

                            vm.cityGroups.push(cityGroup);

                            cityGroup.inEditMode = false;
                            cityGroup.saving = false;
                        }
                    }
                }
            };

            var errorFunc = function (error) {

                if (error.data
                    && checkRedirectForSignIn(error.data)) {

                    viewModel.errorstatus = error.status + ' - ' + error.statusText;
                    viewModel.errormsg = error.data.msg;
                    viewModel.errorid = error.data.id;
                    viewModel.showError = true;
                }
            };

            var finallyFunc = function () {

                toggleGlblWaitVisibility(false);
            };

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlGetCityGroups, { limit: limit, offset: offset, getTotalCityGroups: (vm.cityGroupsCount <= 0 || forceGetCount), allNames: (name ? name : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (cityGroup) {

            if (cityGroup
                && cityGroup.__comp) {

                if (cityGroup.id > 0) {

                    var org = cityGroup.__comp;

                    cityGroup.name = org.name;
                    cityGroup.active = org.active;

                    cityGroup.inEditMode = false;
                }
                else {

                    vm.cityGroups.remove(cityGroup);
                }
            }
        };


        vm.addCityGroup = function () {

            var newItem = {
                id: 0,
                country_Id: vm.defCountry ? vm.defCountry.id : 0,
                name: '',
                seqn: 0,
                active: true
            };

            newItem.__comp = jQuery.extend(true, {}, newItem);

            newItem.inEditMode = true;
            newItem.saving = false;

            vm.cityGroups.insert(0, newItem);
        };


        vm.save = function (cityGroups, finallyCallback, deleteFlags) {

            if (cityGroups) {

                if (!Array.isArray(cityGroups)) {

                    cityGroups = [cityGroups];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validCityGroups = [];
                var toBeSavedCityGroups = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length == cityGroups.length);

                for (var i = 0; i < cityGroups.length; i++) {

                    var cityGroup = cityGroups[i];

                    if (cityGroup
                        && cityGroup.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, cityGroup);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        checkRecordState(toBeSaved, cityGroup.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedCityGroups.push(toBeSaved);

                        cityGroup.saving = true;
                        validCityGroups.push(cityGroup);
                    }
                }

                if (validCityGroups.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validCityGroups.length == resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validCityGroups[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.seqn = value.seqn;
                                        tempValue.name = value.name;
                                        tempValue.country_Id = value.country_Id;
                                        tempValue.active = value.active;

                                        if (toBeSavedCityGroups[index].recordState == 10) {

                                            vm.cityGroupsCount += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.cityGroups.remove(tempValue);

                                        vm.cityGroupsCount -= 1;

                                        if (vm.cityGroupsCount <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }
                                });
                            }
                        }
                    };

                    var errorFunc = function (error) {

                        if (error.data
                            && checkRedirectForSignIn(error.data)) {

                            viewModel.errorstatus = error.status + ' - ' + error.statusText;
                            viewModel.errormsg = error.data.msg;
                            viewModel.errorid = error.data.id;
                            viewModel.showError = true;
                        }
                    };

                    var finallyFunc = function () {

                        for (var i = 0; i < validCityGroups.length; i++) {

                            validCityGroups[i].saving = false;
                            validCityGroups[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    viewModel.showError = false;
                    viewModel.errorstatus = '';
                    viewModel.errormsg = '';
                    viewModel.errorid = 0;

                    $http.post(appProps.urlSaveCityGroups, { cityGroups: toBeSavedCityGroups })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };
    }


    angular.module('app-mainmenu')
        .controller('deleteObjectInstanceCtrlr', ['$uibModalInstance', 'appProps', 'param', deleteObjectInstanceCtrlr]);

    function deleteObjectInstanceCtrlr($uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.target = param.target;
        vm.targetName = param.targetName;

        vm.yes = function () {

            $uibModalInstance.close(vm.target);

            toggleGlblWaitVisibility(true);

            param.save(param.target, function () {

                toggleGlblWaitVisibility(false);
            }, true);
        };

        vm.no = function () {

            $uibModalInstance.close();
        };
    }

})();