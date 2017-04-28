
(function () {

    'use strict';


    angular.module('app-mainmenu')
           .controller('tagsCtrlr', ['$http', '$scope', '$uibModal', 'appProps', 'headerConfigService', tagsCtrlrFunc]);

    function tagsCtrlrFunc($http, $scope, $uibModal, appProps, headerConfigService) {

        $('[data-toggle=tooltip]').tooltip({ trigger: 'hover' });


        headerConfigService.reset();
        headerConfigService.title = appProps.lbl_Tags;
        headerConfigService.showToolBar = true;
        headerConfigService.showSearchCtrl = true;
        headerConfigService.showSaveBtn = false;
        headerConfigService.addBtnTltp = appProps.msg_AddTag;
        headerConfigService.refreshBtnTltp = appProps.msg_RfrshTags;
        headerConfigService.saveBtnTltp = appProps.msg_SaveTags;


        var vm = this;

        vm.appProps = appProps;

        vm.pgsCollection = [];
        vm.currentPgNmbr = 0;
        vm.totalPgs = 0;

        vm.tagsCount = 0;
        vm.tagsCountMod = 0;

        vm.tags = [];

        vm.gotoPage = function (pg, scrollToTop) {

            if (pg && !pg.isActive && !pg.disabled) {

                if (scrollToTop) {

                    //$("html,body").animate({ scrollTop: 0 }, "slow");
                }

                var offset = (appProps.resultItemsPerPg * pg.index) - 1;

                offset = offset < 0 ? 0 : offset;

                vm.currentPgNmbr = pg.index;

                vm.populateTags(appProps.resultItemsPerPg, offset);
            }
        };


        vm.deleteModal = function (tag) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'deleteTagModal.html',
                controller: 'deleteTagInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#tagsVw .modal-container'),
                resolve: {
                    param: function () {

                        return {
                            tag: tag,
                            save: vm.save
                        };
                    }
                }
            });
        };


        vm.openModal = function (tag) {

            $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'tagModal.html',
                controller: 'tagInstanceCtrlr',
                controllerAs: 'vm',
                size: 'lg',
                appendTo: $('#tagsVw .modal-container'),
                resolve: {
                    tag: function () {

                        var copy = jQuery.extend(true, {}, tag);

                        return copy;
                    }
                }
            });
        };

        vm.goInEditMode = function (e, tag) {

            if (tag) {

                tag.inEditMode = true;

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


        var checkRecordState = function (tag, compTag, toBeDeleted) {

            if (tag
                && compTag) {

                if (tag.id > 0) {

                    if (toBeDeleted) {

                        tag.recordState = 30;
                    }
                    else if (tag.name_en !== compTag.name_en
                            || tag.name_fr !== compTag.name_fr
                            || tag.slug_en !== compTag.slug_en
                            || tag.slug_fr !== compTag.slug_fr
                            || tag.active !== compTag.active) {

                        tag.recordState = 20;
                    }
                    else {

                        tag.recordState = 0;
                    }
                }
                else {

                    tag.recordState = 10;
                }
            }
        };


        var prevName = '';
        vm.populateTags = function (limit, offset, name) {

            var forceGetCount = false;

            name = !name ? vm.searchValue : name;

            if (name || prevName) {

                forceGetCount = (name !== prevName);
            }

            prevName = name;

            forceGetCount = (forceGetCount || vm.tagsCountMod !== 0);

            vm.showError = false;
            vm.errorstatus = '';
            vm.errormsg = '';
            vm.errorid = 0;

            limit = !limit ? appProps.resultItemsPerPg : limit;
            offset = !offset ? 0 : offset;


            var successFunc = function (resp) {

                vm.tags.length = 0;

                if (resp.data) {

                    if (resp.data.count > 0
                        || (resp.data.count === 0 && resp.data.result && resp.data.result.length === 0)) {

                        vm.tagsCountMod = 0;
                        vm.tagsCount = resp.data.count;

                        var ttlPgs = parseInt(vm.tagsCount / appProps.resultItemsPerPg);

                        ttlPgs += ((vm.tagsCount % appProps.resultItemsPerPg) > 0) ? 1 : 0;

                        vm.totalPgs = ttlPgs;
                    }

                    setupPages();

                    if (resp.data.result) {

                        for (var i = 0; i < resp.data.result.length; i++) {

                            var tag = resp.data.result[i];

                            tag.name = appProps.currentLang.startsWith('en') ? tag.name_en : tag.name_fr;

                            tag.__comp = jQuery.extend(true, {}, tag);

                            vm.tags.push(tag);

                            tag.inEditMode = false;
                            tag.saving = false;
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

            $http.post(appProps.urlGetTags, { limit: limit, offset: offset, getTotalTags: (vm.tagsCount <= 0 || forceGetCount), name: (name ? name : null) })
                 .then(successFunc, errorFunc)
                 .finally(finallyFunc);
        };


        vm.revert = function (tag) {

            if (tag
                && tag.__comp) {

                if (tag.id > 0) {

                    var org = tag.__comp;

                    tag.name_en = org.name_en;
                    tag.name_fr = org.name_fr;
                    tag.slug_en = org.slug_en;
                    tag.slug_fr = org.slug_fr;
                    tag.active = org.active;

                    tag.inEditMode = false;
                }
                else {

                    vm.tags.remove(tag);
                }
            }
        };


        vm.save = function (tags, finallyCallback, deleteFlags) {

            if (tags) {

                if (!Array.isArray(tags)) {

                    tags = [tags];
                }

                if (deleteFlags && !Array.isArray(deleteFlags)) {

                    deleteFlags = [deleteFlags];
                }

                var validTags = [];
                var toBeSavedTags = [];

                var hasDeleteFlags = (deleteFlags && deleteFlags.length === tags.length);

                for (var i = 0; i < tags.length; i++) {

                    var tag = tags[i];

                    if (tag
                        && tag.__comp) {

                        var toBeSaved = jQuery.extend(true, {}, tag);
                        delete toBeSaved.__comp;

                        checkRecordState(toBeSaved, tag.__comp, hasDeleteFlags ? deleteFlags[i] : false);
                        toBeSavedTags.push(toBeSaved);

                        tag.saving = true;
                        validTags.push(tag);
                    }
                }

                if (validTags.length > 0) {

                    var successFunc = function (resp) {

                        if (resp.data
                            && resp.data.result
                            && resp.data.result.length > 0) {

                            if (validTags.length === resp.data.result.length) {

                                $.each(resp.data.result, function (index, value) {

                                    var tempValue = validTags[index];

                                    if (value) {

                                        tempValue.id = value.id;
                                        tempValue.name_en = value.name_en;
                                        tempValue.name_fr = value.name_fr;
                                        tempValue.slug_en = value.slug_en;
                                        tempValue.slug_fr = value.slug_fr;
                                        tempValue.active = value.active;

                                        tag.name = appProps.currentLang.startsWith('en') ? tag.name_en : tag.name_fr;

                                        if (toBeSavedTags[index].recordState === 10) {

                                            vm.tagsCountMod += 1;

                                            if (!vm.totalPgs || vm.totalPgs <= 0) {

                                                vm.totalPgs = 1;
                                            }
                                        }

                                        delete tempValue.status;
                                        delete tempValue.recordState;

                                        tempValue.__comp = value;
                                    }
                                    else {

                                        vm.tags.remove(tempValue);

                                        vm.tagsCountMod -= 1;

                                        if ((vm.tagsCount + vm.tagsCountMod) <= 0) {

                                            vm.totalPgs = 0;
                                        }
                                    }

                                    $scope.$apply();
                                });
                            }
                        }
                    };

                    var errorFunc = function (error) {

                        $.each(validTags, function (index, value) {

                            value.inEditMode = true;
                        });

                        if (error.data
                            && checkRedirectForSignIn(error.data)) {

                            vm.errorstatus = error.status + ' - ' + error.statusText;
                            vm.errormsg = error.data.msg;
                            vm.errorid = error.data.id;
                            vm.showError = true;
                        }
                    };

                    var finallyFunc = function () {

                        for (var i = 0; i < validTags.length; i++) {

                            validTags[i].saving = false;
                            validTags[i].inEditMode = false;
                        }

                        if (finallyCallback) {

                            finallyCallback();
                        }
                    };


                    vm.showError = false;
                    vm.errorstatus = '';
                    vm.errormsg = '';
                    vm.errorid = 0;

                    $http.post(appProps.urlSaveTags, { tags: toBeSavedTags })
                         .then(successFunc, errorFunc)
                         .finally(finallyFunc);
                }
            }
        };


        var buttonClick = function (e) {

            var $btn = $(this);

            if ($btn.hasClass('refresh')) {

                var activePg = $.grep(vm.pgsCollection, function (pg) { return pg.isActive; });

                if (activePg && Array.isArray(activePg)) {

                    activePg = activePg.length > 0 ? activePg[0] : null;
                }

                if (activePg) {

                    var offset = (appProps.resultItemsPerPg * activePg.index) - 1;

                    $scope.$apply(function () {

                        vm.populateTags(appProps.resultItemsPerPg, offset);
                    });
                }
            }
            else if ($btn.hasClass('add')) {

                $scope.$apply(function () {

                    var newItem = {
                        id: 0,
                        name_en: null,
                        name_fr: null,
                        slug_en: null,
                        slug_fr: null,
                        active: true
                    };

                    newItem.__comp = jQuery.extend(true, {}, newItem);

                    newItem.inEditMode = true;
                    newItem.saving = false;

                    vm.tags.insert(0, newItem);
                });
            }
        };

        $('#searchBar .btn.btn-tb').on('click', buttonClick);

        $('#searchBar').on('searchGo', function (e, arg) {

            if (arg && arg.searchValue) {

                $scope.$apply(function () {

                    vm.searchValue = arg.searchValue;
                    vm.populateTags(appProps.resultItemsPerPg, 0, arg.searchValue);
                });
            }
        });
        $('#searchBar').on('searchClear', function (e, arg) {

            $scope.$apply(function () {

                vm.searchValue = '';
                vm.populateTags(appProps.resultItemsPerPg, 0);
            });
        });
    }


    angular.module('app-mainmenu')
        .controller('deleteTagInstanceCtrlr', ['$uibModalInstance', 'appProps', 'param', deleteTagInstanceCtrlrFunc]);

    function deleteTagInstanceCtrlrFunc($uibModalInstance, appProps, param) {

        var vm = this;
        vm.appProps = appProps;

        vm.tag = param.tag;

        vm.yes = function () {

            $uibModalInstance.close(vm.tag);
            $('#glblWait').removeClass('hidden');

            param.save(param.tag, function () {

                $('#glblWait').addClass('hidden');
            }, true);
        };

        vm.no = function () {

            $uibModalInstance.close();
        };
    }

})();