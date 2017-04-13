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

        vm.hasSuccess = false;

        vm.csvItems = [];
        vm.tableHeaders = [];
        vm.categories = [];
        vm.categoryErrorsCount = 0;

        vm.showDbValErrors = false;

        vm.errorCount = 0;
        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.addrFilename = '';
        vm.timeFilename = '';

        vm.log = {
            text: ''
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

                            for (i = 0; i < resp.data.addresses.length; i++) {

                                var addr = resp.data.addresses[i];
                                var _catIndex = addr._CatgIndex;

                                for (var i = 0; i < resp.data.processedCsvCatgs.length; i++) {

                                    var category = resp.data.processedCsvCatgs[i];

                                    if (_catIndex == category.index) {

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

                            var i;

                            for (i = 0; i < resp.data.addresses.length; i++) {

                                var addr = resp.data.addresses[i];

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

                            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

                            for (i = 0; i < errors.length; i++) {

                                if (errors[i] === null) {

                                    vm.hasSuccess = true;

                                }
                                else {

                                    vm.errorCount++;
                                    vm.log.text = errors[i];
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

            $scope.$apply(function () {

                vm.addrFilename = addrCSVFileInput.files[0].name;
            })
        };

        var timeCSVFileInput = document.getElementById('timeCSV');

        timeCSVFileInput.onchange = function () {

            $scope.$apply(function () {

                vm.timeFilename = timeCSVFileInput.files[0].name;
            })
        };


        vm.clearForm = function () {

            vm.hasSuccess = false;

            vm.showTable = false;
            vm.showClear = false;
            vm.showUpload = true;

            vm.showDbValErrors = false;

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

        vm.download = function (text) {
            var data = new Blob([text], { type: 'text/plain;charset=utf-8' });
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

})();