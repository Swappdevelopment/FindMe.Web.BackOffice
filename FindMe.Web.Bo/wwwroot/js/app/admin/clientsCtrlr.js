
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('clientsCtrlr', ['$http', '$uibModal', 'appProps', 'headerConfigService', clientsCtrlrFunc]);

    function clientsCtrlrFunc($http, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Clnts;
        headerConfigService.showToolBar = true;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddClnts;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshClnts;
        headerConfigService.saveBtnTltp = appProps.msg_SaveClnts;

        var vm = this;

        vm.appProps = appProps;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.clientsCount = 0;

        vm.clients = [];

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateClients(appProps.resultItemsPerPg, offset);
            }
        };


        vm.openModal = function (client) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'clientModal.html',
                controller: 'clientInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#clientsVw .modal-container'),
                resolve: {
                    client: function () {

                        var copy = jQuery.extend(true, {}, client);

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


        vm.populateClients = function (limit, offset) {

            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;

            var successFunc = function (resp) {

                vm.clients.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0) {

                        vm.clientsCount = resp.data.count;

                        var ttlPgs = parseInt(vm.clientsCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.clientsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var client = resp.data.result[i];

                            vm.clients.push(client);
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
                }
            };

            var finallyFunc = function () {

                toggleGlblWaitVisibility(false);
            };

            toggleGlblWaitVisibility(true);

            $http.post(appProps.urlGetClients, { limit: limit, offset: offset, getTotalClients: (vm.clientsCount <= 0) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                vm.populateClients();
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);
    }


    angular.module('app-mainmenu')
        .controller('clientInstanceCtrlr', ['$uibModalInstance', 'appProps', 'client', clientInstanceCtrlrFunc]);

    function clientInstanceCtrlrFunc($uibModalInstance, appProps, client) {

        var vm = this;
        vm.appProps = appProps;

        vm.client = client;

        vm.ok = function () {
            $uibModalInstance.close(vm.client);
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }

})();