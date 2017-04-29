(function () {

    'use strict';


    angular.module('app-mainmenu').service('listItemService', ['$rootScope', '$timeout', listItemServiceFunc]);


    function listItemServiceFunc($rootScope, $timeout) {

        return {

            setItemFormValidation: function (element, arrItems) {

                if (element) {

                    if (!element.formVal) {

                        element.formVal = {};
                    }

                    var formVal = element.formVal;

                    if (arrItems && arrItems.length > 0) {

                        for (var i = 0; i < arrItems.length; i++) {

                            var item = arrItems[i];

                            if (item.name) {

                                item.type = item.type ? item.type : '';

                                if (!formVal[item.name]) {

                                    formVal[item.name] = {};
                                }

                                formVal[item.name].name = item.name;

                                formVal[item.name].isTouched = false;

                                formVal[item.name].touched = function () {

                                    this.isTouched = true;
                                };

                                if (item.isReq) {

                                    formVal[item.name].isReq = true;
                                }
                                else if (formVal[item.name].isReq) {

                                    delete formVal[item.name].isReq;
                                }

                                formVal[item.name].isValid = function () {

                                    if (this.isReq) {

                                        return element[this.name];
                                    }
                                    else {

                                        return true;
                                    }
                                };
                            }
                        }


                        formVal.isValid = function () {

                            var result = true;

                            $.each(this, function (key, value) {

                                if (value.isValid) {

                                    if (!value.isValid()) {

                                        result = false;
                                        return true;
                                    }
                                }
                            });

                            return result;
                        }
                    }
                }
            }
            ,
            isFormValArrayValid: function (formVals) {

                if (formVals && formVals.length > 0) {

                    for (var i = 0; i < formVals.length; i++) {

                        var frmVal = formVals[i];

                        if (!frmVal.isValid) {

                            frmVal = frmVal.formVal;
                        }

                        if (frmVal && frmVal.isValid) {

                            if (!frmVal.isValid()) {

                                return false;
                                break;
                            }
                        }
                    }
                }

                return true;
            }
        };
    }

})();