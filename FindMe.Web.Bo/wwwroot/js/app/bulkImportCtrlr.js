﻿
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('bulkImportCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', 'FileSaver', 'Blob', bulkImportCtrlrFunc]);

    function bulkImportCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService, FileSaver, Blob) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });

        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Import;
        headerConfigService.showToolBar = false;
        headerConfigService.showSearchCtrl = false;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddClnts;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshClnts;
        headerConfigService.saveBtnTltp = appProps.msg_SaveClnts;

        var vm = this;

        vm.appProps = appProps;

        vm.showUpload = true;
        vm.showClear = false;
        vm.showTable = false;
        vm.showError = false;
        vm.hasFile = false;
        vm.Errors = false;
        vm.CsvErrors = false;

        vm.hasSuccess = false;

        vm.cityRegions = [];
        vm.cityDistricts = [];
        vm.cityGroups = [];
        vm.postSaveLogs = [];

        vm.csvItems = [];
        vm.tableHeaders = [];
        vm.categories = [];
        vm.subCategories = [];
        vm.categoryErrorsCount = 0;
        vm.tags = [];
        vm.tagErrorsCount = 0;
        vm.cityDetails = [];
        vm.cityDetailErrorsCount = 0;

        vm.showCatgDbValErrors = false;
        vm.showTagDbValErrors = false;
        vm.showCityDetailDbValErrors = false;
        vm.showPostSaveErrors = false;

        vm.errorCount = 0;
        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.addrFilename = '';
        vm.timeFilename = '';

        var table = $('#csv');
        var rows = document.getElementById('csv-table').getElementsByTagName('tbody')[0].getElementsByTagName('tr');


        vm.browseGo = function (elementName) {

            if (elementName) {

                $('#' + elementName).click();
            }
        };

        var validateAddressUtfValues = function (addr) {

            if (addr) {

                addr.addressName = addr.addressName ? addr.addressName.encodeHtml() : addr.addressName;
            }
        };

        var hasErrors = function (addr, skipPostSaveErrors) {

            if (addr) {

                if (addr.hasPostSaveError && !skipPostSaveErrors) return true;
                if (addr.errorMessage) return true;

                if (addr._linkParentCatg && !addr._linkParentCatg.foundInDb) return true;
                if (addr._linkCatg && !addr._linkCatg.foundInDb) return true;
                if (addr._linkCityDetail && !addr._linkCityDetail.foundInDb) return true;

                if (addr._linkTags && addr._linkTags.length > 0) {

                    return $.grep(addr._linkTags, function (v) {
                        return !v.foundInDb;
                    }) > 0;
                }

                if (addr._DaysCsv && addr._DaysCsv.length > 0) {

                    var hasDayError = false;

                    $.each(addr._DaysCsv, function (index, day) {

                        if ($.grep(day.values, function (t) { return (t.errorMessage && t.errorMessage.length > 0) }).length > 0) {

                            hasDayError = true;
                        }
                    });

                    if (hasDayError) return true;
                }
            }

            return false;
        }

        vm.uploadCSvForm = function () {

            vm.categoryErrorsCount = 0;

            var htmlElement = $('#addrCSV').get(0);

            if (htmlElement && htmlElement.files.length === 1) {

                var data = new FormData();

                data.append('ADDR_CSV', htmlElement.files[0]);

                htmlElement = $('#timeCSV').get(0);


                if (htmlElement && htmlElement.files.length === 1) {

                    data.append('TIME_CSV', htmlElement.files[0]);
                }


                var successFunc = function (resp) {

                    if (resp && resp.data) {

                        if (resp.data.error) {

                            vm.showUpload = false;
                            vm.showClear = true;
                            vm.showTable = true;
                            vm.errormsg = resp.data.error;
                        }
                        else if (resp.data.addresses) {

                            var i;

                            for (i = 0; i < resp.data.processedCsvCatgs.length; i++) {

                                var category = resp.data.processedCsvCatgs[i];

                                if (category) {

                                    vm.categories.push(category);
                                    vm.categoryErrorsCount += category.foundInDb ? 0 : 1;

                                    category.showSubCatgs = true;

                                    if (category.subCategories) {

                                        for (var j = 0; j < category.subCategories.length; j++) {

                                            var subCatg = category.subCategories[j];
                                            subCatg.parentUID = category.uid;
                                            subCatg.parentName = category.name;
                                            vm.categoryErrorsCount += subCatg.foundInDb ? 0 : 1;
                                        }
                                    }
                                }
                            }

                            for (i = 0; i < resp.data.processedCsvTags.length; i++) {

                                var tag = resp.data.processedCsvTags[i];

                                if (tag) {

                                    vm.tags.push(tag);
                                    vm.tagErrorsCount += tag.foundInDb ? 0 : 1;
                                }
                            }

                            for (i = 0; i < resp.data.processedCsvCityDetails.length; i++) {

                                var city = resp.data.processedCsvCityDetails[i];

                                if (city) {

                                    vm.cityDetails.push(city);
                                    vm.cityDetailErrorsCount += city.foundInDb ? 0 : 1;
                                }
                            }

                            for (i = 0; i < resp.data.addresses.length; i++) {

                                var addr = resp.data.addresses[i];

                                addr._linkParentCatg = $.grep(vm.categories, function (v) {
                                    return v.index === addr._ParentCatgIndex;
                                });

                                if (Array.isArray(addr._linkParentCatg)) {

                                    if (addr._linkParentCatg.length > 0) {
                                        addr._linkParentCatg = addr._linkParentCatg[0];
                                    }
                                    else {

                                        addr._linkParentCatg = null;
                                    }
                                }

                                if (addr._linkParentCatg) {

                                    addr._linkCatg = $.grep(addr._linkParentCatg.subCategories, function (v) {
                                        return v.index === addr._CatgIndex;
                                    });
                                }
                                else {

                                    addr._linkCatg = $.grep(vm.categories, function (v) {
                                        return v.index === addr._CatgIndex;
                                    });
                                }

                                if (Array.isArray(addr._linkCatg)) {

                                    if (addr._linkCatg.length > 0) {
                                        addr._linkCatg = addr._linkCatg[0];
                                    }
                                    else {

                                        addr._linkCatg = null;
                                    }
                                }


                                if (addr._TagsIndexes && addr._TagsIndexes.length > 0) {

                                    addr._linkTags = $.grep(vm.tags, function (v) {
                                        return $.inArray(v.index, addr._TagsIndexes) >= 0;
                                    });
                                }
                                else {

                                    addr._linkTags = [];
                                }


                                addr._linkCityDetail = $.grep(vm.cityDetails, function (v) {
                                    return v.index === addr._CityDetailIndex;
                                });

                                if (Array.isArray(addr._linkCityDetail)) {

                                    if (addr._linkCityDetail.length > 0) {
                                        addr._linkCityDetail = addr._linkCityDetail[0];
                                    }
                                    else {

                                        addr._linkCityDetail = null;
                                    }
                                }



                                if (i === 0) {

                                    for (var property in addr) {

                                        if (!property.startsWith('_')) {

                                            vm.tableHeaders.push(property);
                                        }
                                    }
                                }

                                validateAddressUtfValues(addr);


                                vm.csvItems.push(addr);

                                addr.hasErrors = function () {

                                    return this && hasErrors(this);
                                };
                            }

                            $.each(vm.csvItems, function (index, addr) {

                                if (hasErrors(addr)) {

                                    vm.Errors = true;
                                }

                                if (addr.errorMessage) {

                                    vm.CsvErrors = true;
                                }
                            });

                            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

                            for (i = 0; i < errors.length; i++) {

                                var error = errors[i];

                                if (error !== null) {

                                    vm.errorCount++;
                                }
                            }

                            vm.showUpload = false;
                            vm.showClear = true;
                            vm.showTable = true;
                        }
                    }
                };

                var errorFunc = function (error) {

                    if (error.data) {

                        vm.errorstatus = error.status + ' - ' + error.statusText;
                        vm.errormsg = error.data.msg;
                        vm.errorid = error.data.id;
                        vm.showError = true;
                    }
                };

                var finallyFunc = function () {

                    toggleGlblWaitVisibility(false);
                };


                vm.errormsg = '';

                vm.csvItems.length = 0;
                vm.tableHeaders.length = 0;
                vm.errorCount = 0;


                toggleGlblWaitVisibility(true);

                $http({
                    url: "/ApiBulkImport/UploadCSV",
                    method: 'POST',
                    data: data,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).then(successFunc, errorFunc)
                    .finally(finallyFunc);
            }
        };


        var addrCSVFileInput = document.getElementById('addrCSV');

        addrCSVFileInput.onchange = function () {

            vm.addrFilename = addrCSVFileInput.files[0].name;

            if (!$scope.$$phase) {

                $scope.$apply();
            }

            //$scope.$apply(function () {

            //    vm.addrFilename = addrCSVFileInput.files[0].name;
            //});
        };

        var timeCSVFileInput = document.getElementById('timeCSV');

        timeCSVFileInput.onchange = function () {

            vm.timeFilename = timeCSVFileInput.files[0].name;

            if (!$scope.$$phase) {

                $scope.$apply();
            }

        };

        vm.clearForm = function () {

            vm.Errors = false;
            vm.CsvErrors = false;

            vm.hasSuccess = false;

            vm.showTable = false;
            vm.showClear = false;
            vm.showUpload = true;

            vm.showCatgDbValErrors = false;
            vm.showTagDbValErrors = false;
            vm.showCityDetailDbValErrors = false;
            vm.showPostSaveErrors = false;

            vm.addrFilename = '';
            vm.timeFilename = '';

            addrCSVFileInput.value = '';
            timeCSVFileInput.value = '';

            vm.csvItems.length = 0;
            vm.tableHeaders.length = 0;
            vm.categories.length = 0;

            vm.postSaveLogs.length = 0;

            vm.categoryErrorsCount = 0;
            vm.tagErrorsCount = 0;
            vm.cityDetailErrorsCount = 0;

            vm.errormsg = null;
        };



        vm.showRows = function (mode) {

            if (vm.csvItems) {

                switch (mode) {

                    case 'errors':

                        $.each(vm.csvItems, function (index, value) {

                            if (hasErrors(value)) {

                                value.forceHide = false;
                            }
                            else {

                                value.forceHide = true;
                            }
                        });
                        break;

                    case 'success':

                        $.each(vm.csvItems, function (index, value) {

                            if (hasErrors(value)) {

                                value.forceHide = true;
                            }
                            else {

                                value.forceHide = false;
                            }
                        });
                        break;

                    default:

                        $.each(vm.csvItems, function (index, value) {

                            value.forceHide = false;
                        });
                        break;
                }
            }
        };

        vm.downloadingPreSaveLog = false;

        vm.downloadPreSaveLog = function () {

            vm.downloadingPreSaveLog = true;

            var logRow = '';

            $.each(vm.csvItems, function (index, addr) {

                if (hasErrors(addr)) {

                    if (addr.errorMessage) {

                        logRow += addr.errorMessage + "\r\n\r\n";
                    }
                    else {

                        logRow += "Error caught for Row " + addr.addressUUID + ":\r\n";

                        if (addr._linkParentCatg.foundInDb === false) {

                            logRow += "The Category '" + addr._linkParentCatg.name + "' does not exist in the database.\r\n";
                        }

                        if (addr._linkCatg.foundInDb === false) {

                            logRow += "The Sub Category '" + addr._linkCatg.name + "' does not exist in the database.\r\n";
                        }

                        if (addr._linkCityDetail.foundInDb === false) {

                            logRow += "The City '" + addr._linkCityDetail.name + "' does not exist in the database.\r\n\r\n";
                        }

                        if (addr._linkTags && addr._linkTags.length > 0) {

                            var tempTags = '';

                            $.each(addr._linkTags, function (index, value) {

                                if (!value.foundInDb) {

                                    if (tempTags != '') {

                                        tempTags += ', ';
                                    }
                                    tempTags += value.name;
                                }
                            });

                            if (tempTags.length > 0) {

                                logRow += "The Tags '" + tempTags + "' does not exist in the database.\r\n\r\n";
                            }
                        }

                        if (addr._DaysCsv && addr._DaysCsv.length > 0) {

                            var tempDays = '';

                            $.each(addr._DaysCsv, function (index, day) {

                                if (day.values && day.values.length > 0) {

                                    var tempTimes = $.grep(day.values, function (rng) { return (rng.errorMessage && rng.errorMessage.length > 0); });

                                    if (tempTimes.length > 0) {

                                        tempDays += '[' + day.name + ']:\r\n';

                                        $.each(tempTimes, function (index, rng) {

                                            if (index > 0) {

                                                tempDays += '\r\n';
                                            }

                                            tempDays += '\t' + rng.errorMessage;
                                        });

                                        tempDays += '\r\n';
                                    }
                                }
                            });

                            if (tempDays.length > 0) {

                                logRow += tempDays + '\r\n';
                            }
                        }
                    }
                }
            });

            if (logRow.length > 0) {

                var data = new Blob([logRow], { type: 'text/plain;charset=utf-8' });


                var now = new Date();

                var year = now.getFullYear();
                var month = now.getMonth() + 1;
                var date = now.getDate();

                var hours = now.getHours();
                var mins = now.getMinutes();
                var secs = now.getSeconds();

                month = month > 9 ? String(month) : ('0' + month);
                date = date > 9 ? String(date) : ('0' + date);
                hours = hours > 9 ? String(hours) : ('0' + hours);
                mins = mins > 9 ? String(mins) : ('0' + mins);
                secs = secs > 9 ? String(secs) : ('0' + secs);


                FileSaver.saveAs(data, 'Pre-Save-Log_' + year + '-' + month + '-' + date + '_' + hours + ':' + mins + ':' + secs + '.log');

                vm.downloadingPreSaveLog = false;
            }
        };


        vm.downloadingPostSaveLog = false;

        vm.downloadPostSaveLog = function () {

            vm.downloadingPostSaveLog = true;

            if (vm.postSaveLogs && vm.postSaveLogs.length > 0) {

                var logFile = '';

                $.each(vm.postSaveLogs, function (index, value) {

                    if (logFile !== '') {

                        logFile += '\r\n\r\n';
                    }

                    logFile += value.value;
                });

                var data = new Blob([logFile], { type: 'text/plain;charset=utf-8' });

                var now = new Date();

                var year = now.getFullYear();
                var month = now.getMonth() + 1;
                var date = now.getDate();

                var hours = now.getHours();
                var mins = now.getMinutes();
                var secs = now.getSeconds();

                month = month > 9 ? String(month) : ('0' + month);
                date = date > 9 ? String(date) : ('0' + date);
                hours = hours > 9 ? String(hours) : ('0' + hours);
                mins = mins > 9 ? String(mins) : ('0' + mins);
                secs = secs > 9 ? String(secs) : ('0' + secs);


                FileSaver.saveAs(data, 'Post-Save-Log_' + year + '-' + month + '-' + date + '_' + hours + ':' + mins + ':' + secs + '.log');
            }

            vm.downloadingPostSaveLog = false;
        };

        vm.createCatgModal = function (category) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'createCategoryModal.html',
                controller: 'createCategoryInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#bulkimportVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            category: category,
                            rootVm: vm
                        };
                    }
                }
            });
        };


        vm.createTagModal = function (tag) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'createTagModal.html',
                controller: 'createTagInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#bulkimportVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            tag: tag,
                            rootVm: vm
                        };
                    }
                }
            });
        };


        vm.createCityDetailModal = function (cityDetail) {

            if (cityDetail) {

                var funcOpen = function () {

                    $uibModal.open({
                        animation: true,
                        ariaLabelledBy: 'modal-title',
                        ariaDescribedBy: 'modal-body',
                        templateUrl: 'createCityDetailModal.html',
                        controller: 'createCityDetailInstanceCtrlr',
                        controllerAs: 'vm',
                        size: 'lg',
                        appendTo: $('#bulkimportVw .modal-container'),
                        resolve: {
                            param: function () {

                                return {
                                    cityDetail: cityDetail,
                                    cityRegions: vm.cityRegions,
                                    cityGroups: vm.cityGroups,
                                    cityDistricts: vm.cityDistricts,
                                    rootVm: vm
                                };
                            }
                        }
                    });
                };

                if (vm.cityRegions && vm.cityRegions.length > 0
                    && vm.cityDistricts && vm.cityDistricts.length > 0
                    && vm.cityGroups && vm.cityGroups.length > 0) {

                    funcOpen();
                }
                else {

                    var successFunc = function (resp) {

                        if (resp && resp.data) {

                            if (resp.data.regions) {

                                vm.cityRegions = vm.cityRegions ? vm.cityRegions : [];

                                $.each(resp.data.regions, function (index, value) {

                                    value.name = appProps.currentLang.startsWith('en') ? value.name_en : value.name_fr;
                                    vm.cityRegions.push(value);
                                });
                            }

                            if (resp.data.districts) {

                                vm.cityDistricts = vm.cityDistricts ? vm.cityDistricts : [];

                                $.each(resp.data.districts, function (index, value) {

                                    vm.cityDistricts.push(value);
                                });
                            }

                            if (resp.data.groups) {

                                vm.cityGroups = vm.cityGroups ? vm.cityGroups : [];

                                $.each(resp.data.groups, function (index, value) {

                                    vm.cityGroups.push(value);
                                });
                            }
                        }
                    };

                    var errorFunc = function (error) {

                        if (error.data) {

                            vm.errorstatus = error.status + ' - ' + error.statusText;
                            vm.errormsg = error.data.msg;
                            vm.errorid = error.data.id;
                            vm.showError = true;
                        }
                    };

                    var finallyFunc = function () {

                        toggleGlblWaitVisibility(false);

                        funcOpen();
                    };

                    toggleGlblWaitVisibility(true);

                    $http.get('/ApiBulkImport/GetRegionsDistrictsGroups')
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };

        vm.addressesHaveErrors = function (skipPostSaveErrors) {

            return (vm.csvItems
                        && $.grep(vm.csvItems, function (v) { return hasErrors(v, skipPostSaveErrors); }).length > 0);
        };

        vm.addressesHaveSuccess = function () {

            return (vm.csvItems
                        && $.grep(vm.csvItems, function (v) { return !hasErrors(v); }).length > 0);
        };

        vm.getErrorCount = function () {

            return $.grep(vm.csvItems, function (v) { return hasErrors(v); }).length;
        };


        vm.saveAddressCsvs = function () {

            vm.postSaveLogs.length = 0;

            //if (!vm.addressesHaveErrors()) {

            var successFunc = function (resp) {

                if (resp && resp.data && resp.data.logs) {

                    var temp = [];

                    $.each(resp.data.logs, function (index, value) {

                        vm.postSaveLogs.push(value);



                        var arr = $.grep(vm.csvItems, function (v) { return v.addressUUID === value.key; });

                        if (arr.length > 0) {

                            temp.push(arr[0]);
                        }
                    });

                    vm.csvItems.length = 0;

                    $.each(temp, function (index, value) {

                        value.postLogValue = value.value;
                        value.hasPostSaveError = true;
                        vm.csvItems.push(value);
                    });
                }
            };

            var errorFunc = function (error) {

                if (error.data) {

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

            $http.post('/ApiBulkImport/SaveCsvAddresses', { csvs: vm.csvItems })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
            //}
        };
    }


    angular.module('app-mainmenu')
        .controller('createCategoryInstanceCtrlr', ['$http', '$uibModalInstance', 'appProps', 'param', createCategoryInstanceCtrlrFunc]);

    function createCategoryInstanceCtrlrFunc($http, $uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.category = {
            name: param.category.name,
            slug: param.category.slug,
            parentUID: param.category.parentUID,
            parentName: param.category.parentName,
            name_en: param.category.name,
            name_fr: '',
            slug_en: param.category.slug,
            slug_fr: '',
            iconClass: '',
            id: 0,
            parent_Id: null,
            level: 0,
            path: '',
            active: true,
            status: 1,
            recordState: 10,
            desc_en: '',
            desc_fr: ''
        };

        vm.save = function () {

            var success = false;

            var successFunc = function (resp) {

                success = true;

                if (resp.data
                    && resp.data.result
                    && resp.data.result.length > 0) {

                    if (resp.data.result.length === 1) {

                        var result = resp.data.result[0];

                        param.category.uid = result.uid;
                        param.category.foundInDb = true;
                        param.category.name = appProps.currentLang.startsWith('en') ? result.name_en : result.name_fr;

                        param.rootVm.categoryErrorsCount--;

                        if (param.category.subCategories) {

                            for (var i = 0; i < param.category.subCategories.length; i++) {

                                var subCatg = param.category.subCategories[i];

                                subCatg.parentUID = param.category.uid;
                                subCatg.parentName = param.category.name;
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

                toggleGlblWaitVisibility(false);

                if (success) {

                    $uibModalInstance.close();
                }
            };


            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlSaveCatgs, { catgs: [vm.category] })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };

        vm.cancel = function () {

            $uibModalInstance.close();
        };
    }


    angular.module('app-mainmenu')
        .controller('createTagInstanceCtrlr', ['$http', '$uibModalInstance', 'appProps', 'param', createTagInstanceCtrlrFunc]);

    function createTagInstanceCtrlrFunc($http, $uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.tag = {
            name: param.tag.name,
            slug: param.tag.slug,
            name_en: param.tag.name,
            name_fr: '',
            slug_en: param.tag.slug,
            slug_fr: '',
            id: 0,
            active: true,
            status: 1,
            recordState: 10
        };

        vm.save = function () {

            var success = false;

            var successFunc = function (resp) {

                success = true;

                if (resp.data
                    && resp.data.result
                    && resp.data.result.length > 0) {

                    if (resp.data.result.length === 1) {

                        var result = resp.data.result[0];

                        param.tag.uid = result.uid;
                        param.tag.foundInDb = true;
                        param.tag.name = appProps.currentLang.startsWith('en') ? result.name_en : result.name_fr;

                        param.rootVm.tagErrorsCount--;
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

                if (success) {

                    $uibModalInstance.close();
                }
            };


            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlSaveTags, { tags: [vm.tag] })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };

        vm.cancel = function () {

            $uibModalInstance.close();
        };
    }


    angular.module('app-mainmenu')
        .controller('createCityDetailInstanceCtrlr', ['$http', '$uibModalInstance', 'appProps', 'param', createCityDetailInstanceCtrlrFunc]);

    function createCityDetailInstanceCtrlrFunc($http, $uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        var region = $.grep(param.cityRegions, function (v) { return v.id === param.cityDetail.region_Id; });
        var district = $.grep(param.cityDistricts, function (v) { return v.id === param.cityDetail.district_Id; });
        var group = $.grep(param.cityGroups, function (v) { return v.id === param.cityDetail.group_Id; });

        vm.cityRegions = param.cityRegions;
        vm.cityDistricts = param.cityDistricts;
        vm.cityGroups = param.cityGroups;

        vm.regionChanged = function () {

            vm.cityDetail.region_Id = vm.cityDetail.region ? vm.cityDetail.region.id : 0;
        };

        vm.districtChanged = function () {

            vm.cityDetail.district_Id = vm.cityDetail.district ? vm.cityDetail.district.id : 0;
        };

        vm.groupChanged = function () {

            vm.cityDetail.group_Id = vm.cityDetail.group ? vm.cityDetail.group.id : 0;
        };


        vm.cityDetail = {
            name: param.cityDetail.name,
            slug: param.cityDetail.slug,
            name_en: param.cityDetail.name,
            name_fr: '',
            slug_en: param.cityDetail.slug,
            slug_fr: '',

            region_Id: param.cityDetail.region_Id,
            region: region && region.length > 0 ? region[0] : null,

            district_Id: param.cityDetail.region_Id,
            district: district && district.length > 0 ? district[0] : null,

            group_Id: param.cityDetail.region_Id,
            group: group && group.length > 0 ? group[0] : null,

            id: 0,
            active: true,
            status: 1,
            recordState: 10
        };

        vm.save = function () {

            var success = false;

            var successFunc = function (resp) {

                success = true;

                if (resp.data
                    && resp.data.result
                    && resp.data.result.length > 0) {

                    if (resp.data.result.length === 1) {

                        var result = resp.data.result[0];

                        param.cityDetail.uid = result.uid;
                        param.cityDetail.foundInDb = true;
                        param.cityDetail.name = appProps.currentLang.startsWith('en') ? result.name_en : result.name_fr;
                        param.cityDetail.region_Id = result.region_Id;
                        param.cityDetail.district_Id = result.district_Id;
                        param.cityDetail.group_Id = result.group_Id;

                        param.rootVm.cityDetailErrorsCount--;
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

                if (success) {

                    $uibModalInstance.close();
                }
            };

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlSaveCityDetails, { cityDetails: [vm.cityDetail] })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };

        vm.cancel = function () {

            $uibModalInstance.close();
        };
    }

})();