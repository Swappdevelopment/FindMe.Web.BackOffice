
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