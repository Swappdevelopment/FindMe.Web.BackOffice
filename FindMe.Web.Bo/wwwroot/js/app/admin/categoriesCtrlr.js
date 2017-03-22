
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('categoriesCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', categoriesCtrlrFunc]);

    function categoriesCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Categories;
        headerConfigService.showToolBar = true;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddCatgs;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshCatgs;
        headerConfigService.saveBtnTltp = appProps.msg_SaveCatgs;

        var vm = this;

        vm.appProps = appProps;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.categorysCount = 0;

        vm.categorys = [];

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateCategorys(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (category) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteCategoryModal.html',
                controller: 'deleteCategoryInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#categorysVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            category: category,
                            save: vm.save
                        };
                    }
                }
            });
        };


        vm.openModal = function (category) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'categoryModal.html',
                controller: 'categoryInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#categorysVw .modal-container'),
                resolve: {
                    category: function () {

                        var copy = jQuery.extend(true, {}, category);

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



        var checkRecordState = function (category, compCategory, toBeDeleted) {

            if (category
                && compCategory) {

                if (category.id > 0) {

                    if (toBeDeleted) {

                        category.recordState = 30;
                    }
                    else if (category.legalName !== compCategory.legalName
                            || category.civility !== compCategory.civility
                            || category.lName !== compCategory.lName
                            || category.fName !== compCategory.fName
                            || category.paid !== compCategory.paid
                            || category.active !== compCategory.active) {

                        category.recordState = 20;
                    }
                    else {

                        category.recordState = 0;
                    }
                }
                else {

                    category.recordState = 10;
                }
            }
        };


        vm.parentIdFilter = 0;
        vm.parentNameFilter = '';
        vm.setParentCatg = function (catg) {

            if (catg && catg.id > 0) {

                vm.parentIdFilter = catg.id;

                if (String(appProps.currentLang).toLowerCase().startsWith('en')) {

                    vm.parentNameFilter = catg.name_en;
                }
                else {

                    vm.parentNameFilter = catg.name_fr;
                }

                vm.searchValue = '';
                vm.populateCategorys(appProps.resultItemsPerPg, 0);
            }
        };

        vm.clearParentFilters = function () {

            vm.parentIdFilter = 0;
            vm.parentNameFilter = '';
            vm.searchValue = $('#searchBar input.input-search').val();
            vm.populateCategorys(appProps.resultItemsPerPg, 0);
        };


        var prevName = '';
        var prevParentIdFilter = 0;
        vm.populateCategorys = function (limit, offset, name) {

            var forceGetCount = false;

            if (name || prevName) {

                forceGetCount = (name != prevName);
            }

            forceGetCount = (forceGetCount || prevParentIdFilter != vm.parentIdFilter);

            prevParentIdFilter = vm.parentIdFilter;

            prevName = name;

            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            name = !name ? vm.searchValue : name;

            var successFunc = function (resp) {

                vm.categorys.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0) {

                        vm.categorysCount = resp.data.count;

                        var ttlPgs = parseInt(vm.categorysCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.categorysCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var category = resp.data.result[i];

                            category.__comp = jQuery.extend(true, {}, category);

                            vm.categorys.push(category);

                            category.inEditMode = false;
                            category.saving = false;
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

            $http.post(appProps.urlGetCatgs, { parentID: vm.parentIdFilter, limit: limit, offset: offset, getTotalCatgs: (vm.categorysCount <= 0 || forceGetCount), name: (name ? name : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (category) {

            if (category
                && category.__comp) {

                if (category.id > 0) {

                    var org = category.__comp;

                    category.legalName = org.legalName;
                    category.civility = org.civility;
                    category.lName = org.lName;
                    category.fName = org.fName;
                    category.paid = org.paid;
                    category.active = org.active;

                    category.inEditMode = false;
                }
                else {

                    vm.categorys.remove(category);
                }
            }
        };


        vm.save = function (categorys, finallyCallback, deleteFlags) {

            if (categorys) {

                if (!Array.isArray(categorys)) {

                    categorys = [categorys];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validCategorys = [];
                var toBeSavedCategorys = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length == categorys.length);

                for (var i = 0; i < categorys.length; i++) {

                    var category = categorys[i];

                    if (category
                        && category.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, category);
                        delete toBeSaved.__comp;

                        toBeSaved.status = toBeSaved.active ? 1 : 0;

                        checkRecordState(toBeSaved, category.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedCategorys.push(toBeSaved);

                        category.saving = true;
                        validCategorys.push(category);
                    }
                }

                if (validCategorys.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validCategorys.length == resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validCategorys[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.legalName = value.legalName;
                                        tempValue.civility = value.civility;
                                        tempValue.lName = value.lName;
                                        tempValue.fName = value.fName;
                                        tempValue.paid = value.paid;
                                        tempValue.active = value.active;

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.categorys.remove(tempValue);
                                    }
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
                        }
                    };

                    var finallyFunc = function () {

                        for (var i = 0; i < validCategorys.length; i++) {

                            validCategorys[i].saving = false;
                            validCategorys[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };

                    $http.post(appProps.urlSaveCategorys, { categorys: toBeSavedCategorys })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                //$('#searchBar input.input-search').val('');
                vm.populateCategorys();
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

                    vm.categorys.insert(0, newItem);
                });
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);

        $('#searchBar').on('searchGo', function (e, arg) {

            if (arg && arg.searchValue) {

                $scope.$apply(function () {

                    vm.searchValue = arg.searchValue;
                    vm.populateCategorys(appProps.resultItemsPerPg, 0, arg.searchValue);
                });
            }
        });
        $('#searchBar').on('searchClear', function (e, arg) {

            $scope.$apply(function () {

                vm.searchValue = '';
            });
        });
    }


    angular.module('app-mainmenu')
        .controller('deleteCategoryInstanceCtrlr', ['$uibModalInstance', 'appProps', 'param', deleteCategoryInstanceCtrlrFunc]);

    function deleteCategoryInstanceCtrlrFunc($uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.category = param.category;

        vm.yes = function () {

            $uibModalInstance.close(vm.category);
            $('#glblWait').removeClass('hidden');

            param.save(param.category, function () {

                $('#glblWait').addClass('hidden');
            }, true);
        };

        vm.no = function () {

            $uibModalInstance.dismiss('cancel');
        };
    }

})();