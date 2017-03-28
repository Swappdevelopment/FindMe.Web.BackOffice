
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
        vm.categorysCountMod = 0;

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

        vm.goInEditMode = function (e, category) {

            if (category) {

                category.inEditMode = true;

                if (e && e.currentTarget) {

                    var $tr = $(e.currentTarget).parents('tr').first();

                    if ($tr && $tr.length > 0) {

                        $('textarea[elastic]', $tr).trigger('input');
                    }
                }
            }
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
                    else if (category.name_en !== compCategory.name_en
                            || category.name_fr !== compCategory.name_fr
                            || category.slug_en !== compCategory.slug_en
                            || category.slug_fr !== compCategory.slug_fr
                            || category.desc_en !== compCategory.desc_en
                            || category.desc_fr !== compCategory.desc_fr
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

                forceGetCount = (name !== prevName);
            }

            forceGetCount = (forceGetCount || prevParentIdFilter !== vm.parentIdFilter || vm.categorysCountMod !== 0);

            prevParentIdFilter = vm.parentIdFilter;

            prevName = name;

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;
            name = !name ? vm.searchValue : name;

            var successFunc = function (resp) {

                vm.categorys.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0
                        || (resp.data.count === 0 && resp.data.result && resp.data.result.length === 0)) {

                        vm.categorysCountMod = 0;
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

                    vm.showError = true;
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

                    category.name_en = org.name_en;
                    category.name_fr = org.name_fr;
                    category.slug_en = org.slug_en;
                    category.slug_fr = org.slug_fr;
                    category.desc_en = org.desc_en;
                    category.desc_fr = org.desc_fr;
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

                var hasDeleteFlags = (deleteFlags && deleteFlags.length === categorys.length);

                for (var i = 0; i < categorys.length; i++) {

                    var category = categorys[i];

                    if (category
                        && category.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, category);
                        delete toBeSaved.__comp;

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

                            if (validCategorys.length === resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validCategorys[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.level = value.level;
                                        tempValue.path = value.path;
                                        tempValue.parent_Id = value.parent_Id;
                                        tempValue.name_en = value.name_en;
                                        tempValue.name_fr = value.name_fr;
                                        tempValue.slug_en = value.slug_en;
                                        tempValue.slug_fr = value.slug_fr;
                                        tempValue.desc_en = value.desc_en;
                                        tempValue.desc_fr = value.desc_fr;
                                        tempValue.active = value.active;

                                        if (toBeSavedCategorys[index].recordState === 10) {

                                            vm.categorysCountMod += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.categorys.remove(tempValue);

                                        vm.categorysCountMod -= 1;

                                        if ((vm.categorysCount + vm.categorysCountMod) <= 0) {

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

                        for (var i = 0; i < validCategorys.length; i++) {

                            validCategorys[i].saving = false;
                            validCategorys[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };


                    vm.showError = false;
                    vm.errorstatus = '';
                    vm.errormsg = '';
                    vm.errorid = 0;

                    $http.post(appProps.urlSaveCatgs, { catgs: toBeSavedCategorys })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                $scope.$apply(function () {

                    vm.populateCategorys();
                });
            }
            else if ($btn.hasClass('add')) {

                $scope.$apply(function () {

                    var newItem = {
                        id: 0,
                        level: 0,
                        path: '',
                        parent_Id: vm.parentIdFilter > 0 ? vm.parentIdFilter : null,
                        name_en: null,
                        name_fr: null,
                        slug_en: null,
                        slug_fr: null,
                        desc_en: null,
                        desc_fr: null,
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
                vm.populateCategorys(appProps.resultItemsPerPg, 0);
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

            $uibModalInstance.close();
        };
    }

})();