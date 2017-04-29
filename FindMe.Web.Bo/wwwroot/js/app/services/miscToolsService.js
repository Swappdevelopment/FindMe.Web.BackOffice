(function () {

    'use strict';


    angular.module('app-mainmenu').service('miscToolsService', ['$rootScope', '$timeout', miscToolsServiceFunc]);


    function miscToolsServiceFunc($rootScope, $timeout) {

        return {

            slugify: function (strValue) {

                if (strValue && typeof strValue === 'string') {

                    strValue = strValue
                                .accentFold(true)
                                .replaceAll('"', "-")
                                .replaceAll("\r", "-")
                                .replaceAll("\n", "-")
                                .replaceAll("\t", "-")
                                .replaceAll("\\", "-")
                                .replaceAll("/", "-")
                                .replaceAll("   ", "-")
                                .replaceAll("  ", "-")
                                .replaceAll(" ", "-")
                                .replaceAll("(", "-")
                                .replaceAll(")", "-")
                                .replaceAll("[", "-")
                                .replaceAll("]", "-")
                                .replaceAll("{", "-")
                                .replaceAll("}", "-")
                                .replaceAll("_", "-")
                                .replaceAll("=", "-")
                                .replaceAll("+", "-")
                                .replaceAll("&", "-")
                                .replaceAll(",", "-")
                                .replaceAll(".", "-")
                                .replaceAll("!", "")
                                .replaceAll("?", "")
                                .replaceAll("'", "")
                                .replaceAll("------", "-")
                                .replaceAll("-----", "-")
                                .replaceAll("----", "-")
                                .replaceAll("---", "-")
                }

                return strValue;
            }
        };
    }

})();