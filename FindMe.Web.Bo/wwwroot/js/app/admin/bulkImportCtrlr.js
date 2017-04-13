
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

        vm.hasSuccess = false;

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

        vm.errorCount = 0;
        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.addrFilename = '';
        vm.timeFilename = '';

        vm.log = {
            row: ''
        };

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

        var hasErrors = function (addr) {

            if (addr) {

                if (addr.errorMessage) return true;

                if (addr._linkParentCatg && !addr._linkParentCatg.foundInDb) return true;
                if (addr._linkCatg && !addr._linkCatg.foundInDb) return true;
                if (addr._linkCityDetail && !addr._linkCityDetail.foundInDb) return true;

                if (addr._linkTags && !addr._linkTags.length > 0) {

                    return addr._linkParentCatg = $.grep(addr._linkTags, function (v) {
                        return !v.foundInDb;
                    }) > 0;

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

            //$scope.$apply(function () {

            //    vm.timeFilename = timeCSVFileInput.files[0].name;
            //});
        };


        vm.clearForm = function () {

            vm.hasSuccess = false;

            vm.showTable = false;
            vm.showClear = false;
            vm.showUpload = true;

            vm.showCatgDbValErrors = false;
            vm.showTagDbValErrors = false;
            vm.showCityDetailDbValErrors = false;

            vm.addrFilename = '';
            vm.timeFilename = '';

            addrCSVFileInput.value = '';
            timeCSVFileInput.value = '';

            vm.csvItems.length = 0;
            vm.tableHeaders.length = 0;
            vm.categories.length = 0;

            vm.categoryErrorsCount = 0;
        };


        vm.showAll = function () {

            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

            for (var i = 0; i < errors.length; i++) {

                for (var j = 0; j < rows.length; j++) {

                    var hideRow = rows[j];

                    if (errors[i] === null) {

                        hideRow.hidden = false;

                    }
                    else {

                        hideRow.hidden = false;
                    }

                }
            }
        };

        vm.showAllSuccess = function () {

            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

            for (var i = 0; i < errors.length; i++) {

                for (var j = 0; j < rows.length; j++) {

                    var hideRow = rows[j];

                    if (errors[i] === null) {

                        hideRow.hidden = false;
                    }
                    else {

                        hideRow.hidden = true;
                    }

                }
            }
        };

        vm.showAllErrors = function () {

            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

            for (var i = 0; i < errors.length; i++) {

                for (var j = 0; j < rows.length; j++) {

                    var hideRow = rows[j];

                    if (errors[i] === null) {

                        hideRow.hidden = true;

                    }
                    else {

                        hideRow.hidden = false;
                    }

                }
            }
        };

        vm.download = function () {

            $.each(vm.csvItems, function (index, addr) {

                if (addr.errorMessage) {

                    vm.log.row += addr.errorMessage + "\r\n\r\n";
                }
                else {



                    if (addr._linkParentCatg && !addr._linkParentCatg.foundInDb) {

                        vm.log.row += "Error caught for Row " + (index + 1) + ":\r\n";
                        vm.log.row += "The Category '" + addr._linkParentCatg.name + "' does not exist in the database.\r\n"

                        if (addr._linkCatg && !addr._linkCatg.foundInDb) {

                            vm.log.row += "The Sub Category '" + addr._linkCatg.name + "' does not exist in the database.\r\n\r\n"
                        }
                    }

                    if (addr._linkCatg && !addr._linkCatg.foundInDb) {

                        vm.log.row += "The Sub Category '" + addr._linkCatg.name + "' does not exist in the database.\r\n\r\n"
                    }
                }

            });



            //for (j = 0; j < errors.length; j++) {

            //    if (errors[j] === null) {


            //        if (addr._linkParentCatg.foundInDb === false || addr._linkParentCatg.foundInDb != null) {

            //            if (addr._linkCatg.foundInDb === false || addr._linkCatg.foundInDb != null) {

            //                vm.log.row += "Error caught for Row " + [j] + ":\r\n";
            //                vm.log.row += "The Category '" + addr._linkParentCatg.name + "' does not exist in the database\r\n"
            //                vm.log.row += "The Sub Category '" + addr._linkCatg.name + "' does not exist in the database\r\n\r\n"
            //            }

            //            else {

            //                vm.log.row += "Error caught for Row " + [j] + ":\r\n";
            //                vm.log.row += "The Category '" + addr._linkParentCatg.name + "' does not exist in the database\r\n\r\n"
            //            }

            //        }
            //    }
            //    else {

            //        if (addr._linkParentCatg.foundInDb === false || addr._linkParentCatg.foundInDb != null) {

            //            if (addr._linkCatg.foundInDb === false || addr._linkCatg.foundInDb != null) {

            //                vm.log.row += errors[j] + "\r\n";
            //                vm.log.row += "The Category '" + addr._linkParentCatg.name + "' does not exist in the database\r\n"
            //                vm.log.row += "The Sub Category '" + addr._linkCatg.name + "' does not exist in the database\r\n\r\n"
            //            }
            //            else {

            //                vm.log.row += errors[j] + "\r\n";
            //                vm.log.row += "The Category '" + addr._linkParentCatg.name + "' does not exist in the database\r\n\r\n"
            //            }
            //        }

            //        vm.errorCount++;

            //    }
            //}

            //if (!$scope.$$phase) {

            //    $scope.$apply();
            //}

            var data = new Blob([vm.log.row], { type: 'text/plain;charset=utf-8' });
            FileSaver.saveAs(data, 'log.txt');
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
                            rootVm: vm
                        };
                    }
                }
            });
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

        vm.cityDetail = {
            name: param.cityDetail.name,
            slug: param.cityDetail.slug,
            name_en: param.cityDetail.name,
            name_fr: '',
            slug_en: param.cityDetail.slug,
            slug_fr: '',
            region_Id: param.cityDetail.region_Id,
            district_Id: param.cityDetail.region_Id,
            group_Id: param.cityDetail.region_Id,
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