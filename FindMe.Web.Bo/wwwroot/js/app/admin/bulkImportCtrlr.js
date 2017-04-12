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

        vm.hasSuccess = false;

        vm.csvItems = [];
        vm.tableHeaders = [];

        vm.errorCount = 0;
        vm.errorstatus = '';
        vm.errormsg = '';
        vm.errorid = 0;

        vm.filename = '';

        var table = $('#csv');
        var rows = document.getElementById('csv-table').getElementsByTagName('tbody')[0].getElementsByTagName('tr');


        vm.uploadCSvForm = function () {

            var fileUpload = $("#files").get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }

            if (files.length === 1) {

                var successFunc = function (resp) {

                    if (resp && resp.data) {

                        if (resp.data.error) {

                            vm.showUpload = false;
                            vm.showClear = true;
                            vm.showTable = true;
                            vm.errormsg = resp.data.error;
                        }
                        else if (resp.data.addresses) {

                            for (var i = 0; i < resp.data.addresses.length; i++) {

                                var addr = resp.data.addresses[i];

                                if (i === 0) {

                                    for (var property in addr) {

                                        vm.tableHeaders.push(property);
                                    }
                                }

                                vm.csvItems.push(addr);
                            }

                            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

                            for (var i = 0; i < errors.length; i++) {

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

                };

                $http({
                    url: "/ApiBulkImport/UploadCSV",
                    method: 'POST',
                    data: data,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).then(successFunc, errorFunc)
                    .finally(finallyFunc);


                vm.errormsg = '';

                fileUpload.value = '';

                vm.csvItems.length = 0;
                vm.tableHeaders.length = 0;
                vm.errorCount = 0;
            }
        };

        vm.clearForm = function () {

            $("#upload").prop("disabled", true);

            vm.hasSuccess = false;

            vm.showTable = false;
            vm.showClear = false;
            vm.showUpload = true;

            vm.filename = '';

        };


        vm.showAll = function () {

            var errors = vm.csvItems.map(function (csv) { return csv.errorMessage; });

            for (var i = 0; i < errors.length; i++) {

                for (var i = 0; i < rows.length; i++) {

                    var hideRow = rows[i];

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

                for (var i = 0; i < rows.length; i++) {

                    var hideRow = rows[i];

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

                for (var i = 0; i < rows.length; i++) {

                    var hideRow = rows[i];

                    if (errors[i] === null) {

                        hideRow.hidden = true;

                    }
                    else {

                        hideRow.hidden = false;
                    }

                }
            }
        };


        document.getElementById('files').onchange = function () {
            var fileInput = document.getElementById('files');

            $scope.$apply(function () {

                vm.filename = fileInput.files[0].name;
            })
        };

        $("#files").change(function () {
            if ($(this).val()) {

                $("#upload").prop("disabled", false);
            }
            else {

                $("#upload").prop("disabled", true);
            }
        });
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