(function () {

    'use strict';


    angular.module('app-mainmenu').service('miscToolsService', ['$rootScope', '$timeout', miscToolsServiceFunc]);


    function miscToolsServiceFunc($rootScope, $timeout) {

        return {

            slugify: function (strValue) {

                if (strValue && typeof strValue === 'string') {

                    strValue = strValue
                                .accentFold(true)
                                .replace(/\s+/g, '-')           // Replace spaces with -
                                .replaceAll("_", "-")
                                .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
                                .replace(/\-\-+/g, '-')         // Replace multiple - with single -
                                .replace(/^-+/, '')             // Trim - from start of text
                                .replace(/-+$/, '');            // Trim - from end of text
                }

                return strValue;
            }
        };
    }

})();