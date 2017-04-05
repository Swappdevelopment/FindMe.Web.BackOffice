
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('clientsCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', clientsCtrlrFunc]);

    function clientsCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

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
        vm.clientsCountMod = 0;

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


        vm.deleteModal = function (client) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteClientModal.html',
                controller: 'deleteClientInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#clientsVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            client: client,
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



        var setDispName = function (client) {

            if (client) {

                client.dispName = (!client.civility ? '' : (client.civility.toUpperCase() + ' ')) + (client.lName === null ? '' : client.lName.toUpperCase() + (!client.fName ? '' : ', ')) + (!client.fName ? '' : client.fName);
            }
        };

        var checkRecordState = function (client, compClient, toBeDeleted) {

            if (client
                && compClient) {

                if (client.id > 0) {

                    if (toBeDeleted) {

                        client.recordState = 30;
                    }
                    else if (client.legalName !== compClient.legalName
                            || client.civility !== compClient.civility
                            || client.lName !== compClient.lName
                            || client.fName !== compClient.fName
                            || client.paid !== compClient.paid
                            || client.active !== compClient.active) {

                        client.recordState = 20;
                    }
                    else {

                        client.recordState = 0;
                    }
                }
                else {

                    client.recordState = 10;
                }
            }
        };


        var prevAllNames = '';
        vm.populateClients = function (limit, offset, allNames) {

            var forceGetCount = false;

            if (allNames || prevAllNames) {

                forceGetCount = (allNames !== prevAllNames || vm.clientsCountMod !== 0);
            }
            else {

                forceGetCount = (vm.clientsCountMod !== 0);
            }

            prevAllNames = allNames;

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            allNames = !allNames ? vm.searchValue : allNames;

            var successFunc = function (resp) {

                vm.clients.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0
                        || (resp.data.count === 0 && resp.data.result && resp.data.result.length === 0)) {

                        vm.clientsCountMod = 0;
                        vm.clientsCount = resp.data.count;

                        var ttlPgs = parseInt(vm.clientsCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.clientsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var client = resp.data.result[i];

                            client.__comp = jQuery.extend(true, {}, client);

                            vm.clients.push(client);

                            client.inEditMode = false;
                            client.saving = false;

                            setDispName(client);
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

            $http.post(appProps.urlGetClients, { limit: limit, offset: offset, getTotalClients: (vm.clientsCount <= 0 || forceGetCount), allNames: (allNames ? allNames : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (client) {

            if (client
                && client.__comp) {

                if (client.id > 0) {

                    var org = client.__comp;

                    client.legalName = org.legalName;
                    client.civility = org.civility;
                    client.lName = org.lName;
                    client.fName = org.fName;
                    client.paid = org.paid;
                    client.active = org.active;

                    setDispName(client);

                    client.inEditMode = false;
                }
                else {

                    vm.clients.remove(client);
                }
            }
        };


        vm.save = function (clients, finallyCallback, deleteFlags) {

            if (clients) {

                if (!Array.isArray(clients)) {

                    clients = [clients];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validClients = [];
                var toBeSavedClients = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length === clients.length);

                for (var i = 0; i < clients.length; i++) {

                    var client = clients[i];

                    if (client
                        && client.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, client);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        checkRecordState(toBeSaved, client.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedClients.push(toBeSaved);

                        client.saving = true;
                        validClients.push(client);
                    }
                }

                if (validClients.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validClients.length === resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validClients[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.legalName = value.legalName;
                                        tempValue.civility = value.civility;
                                        tempValue.lName = value.lName;
                                        tempValue.fName = value.fName;
                                        tempValue.paid = value.paid;
                                        tempValue.active = value.active;

                                        if (toBeSavedClients[index].recordState === 10) {

                                            vm.clientsCountMod += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        setDispName(tempValue);

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.clients.remove(tempValue);

                                        vm.clientsCountMod -= 1;

                                        if ((vm.clientsCount + vm.clientsCountMod) <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }

                                    $scope.$apply();
                                });
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

                        for (var i = 0; i < validClients.length; i++) {

                            validClients[i].saving = false;
                            validClients[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    vm.showError = false;
                    vm.errorstatus = '';
                    vm.errormsg = '';
                    vm.errorid = 0;

                    $http.post(appProps.urlSaveClients, { clients: toBeSavedClients })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                $scope.$apply(function () {

                    vm.populateClients();
                });
            }
            else if ($btn.hasClass('add')) {

                $scope.$apply(function () {

                    var newItem = {
                        id: 0,
                        legalName: '',
                        civility: '',
                        lName: '',
                        fName: '',
                        paid: false,
                        active: true
                    };

                    newItem.__comp = jQuery.extend(true, {}, newItem);

                    newItem.inEditMode = true;
                    newItem.saving = false;

                    vm.clients.insert(0, newItem);
                });
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);

        $('#searchBar').on('searchGo', function (e, arg) {

            if (arg && arg.searchValue) {

                $scope.$apply(function () {

                    vm.searchValue = arg.searchValue;
                    vm.populateClients(appProps.resultItemsPerPg, 0, arg.searchValue);
                });
            }
        });
        $('#searchBar').on('searchClear', function (e, arg) {

            $scope.$apply(function () {

                vm.searchValue = '';
                vm.populateClients(appProps.resultItemsPerPg, 0);
            });
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