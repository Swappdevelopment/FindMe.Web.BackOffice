(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('bulkImportCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', bulkImportCtrlrFunc]);

    function bulkImportCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

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
        vm.catExist = true;
        vm.subCatExist = true;

        vm.hasSuccess = false;

        vm.csvItems = [];
        vm.tableHeaders = [];
        vm.categories = [];
        vm.subCategories = [];

        vm.categoryCount = 0;
        vm.subCategoryCount = 0;

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


        vm.uploadCSvForm = function () {

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

                            for (var i = 0; i < resp.data.processedCsvCatgs.length; i++) {

                                var category = resp.data.processedCsvCatgs;
                                var subCategory = resp.data.processedCsvCatgs[i].subCategories;

                                var categoryExist = resp.data.processedCsvCatgs[i].foundInDb;
                                var subCategoryExist = resp.data.processedCsvCatgs[i].subCategories[i].foundInDb;


                                if (categoryExist == false) {

                                    vm.categoryCount++;
                                    vm.catExist = false;
                                }
                                else {

                                    if (subCategoryExist == false) {

                                        vm.subCategoryCount++;
                                        vm.subCatExist = false;

                                    }
                                    else {

                                    }
                                }

                                vm.categories.push(category[i]);
                                vm.subCategories.push(subCategory[i])
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
                                }
                            }

                            vm.showUpload = false;
                            vm.showClear = true;
                            vm.showTable = true;
                            vm.catExist = false;
                            vm.subCatExist = false;

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
                vm.categoryCount = 0;
                vm.subCategoryCount = 0;


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
            vm.catExist = true;
            vm.subCatExist = true;

            vm.addrFilename = '';
            vm.timeFilename = '';

            addrCSVFileInput.value = '';
            timeCSVFileInput.value = '';

            vm.csvItems.length = 0;
            vm.tableHeaders.length = 0;
            vm.categories.length = 0;
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


        //$(addrCSVFileInput).change(function () {
        //    if ($(this).val()) {

        //        $("#upload").prop("disabled", false);
        //    }
        //    else {

        //        $("#upload").prop("disabled", true);
        //    }
        //});
    }


    angular.module('app-mainmenu')
        .controller('deleteClientInstanceCtrlr', ['$uibModalInstance', 'appProps', 'param', deleteClientInstanceCtrlrFunc]);

    function deleteClientInstanceCtrlrFunc($uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.client = param.client;

        vm.yes = function () {

            $uibModalInstance.close(vm.client);

            toggleGlblWaitVisibility(true);

            param.save(param.client, function () {

                toggleGlblWaitVisibility(false);
            }, true);
        };

        vm.no = function () {

            $uibModalInstance.close();
        };
    }

})();