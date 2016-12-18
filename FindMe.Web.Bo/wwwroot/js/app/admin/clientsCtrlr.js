
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('clientsCtrlr', ['$http', 'appProps', 'headerConfigService', clientsCtrlrFunc]);


    function clientsCtrlrFunc($http, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Clnts;
        headerConfigService.showToolBar = true;
        headerConfigService.addBtnTltp = appProps.msg_AddClnts;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshClnts;
        headerConfigService.saveBtnTltp = appProps.msg_SaveClnts;

        var vm = this;

        vm.appProps = appProps;

        vm.clients = [];

        for (var i = 0; i < 1000; i++) {

            vm.clients.push({
            });
        }
    }

})();