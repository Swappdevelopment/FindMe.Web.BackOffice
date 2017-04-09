
(function () {

    'use strict';

    //Creating the module
    angular.module('app-mainmenu', ['ngRoute', 'ui.bootstrap', 'ui.bootstrap.fontawesome', 'ngTouch', 'ngAnimate'])

        .config(function ($routeProvider, $locationProvider) {

            $routeProvider.when('/', {
                controller: 'homeCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Home.html',
            });
            $routeProvider.when('/home', {
                controller: 'homeCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Home.html',
            });

            $routeProvider.when('/profile', {
                controller: 'profileCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Account/Profile.html',
            });

            $routeProvider.when('/users', {
                controller: 'usersCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/Users.html',
            });

            $routeProvider.when('/clients', {
                controller: 'clientsCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/Clients.html',
            });

            $routeProvider.when('/categories', {
                controller: 'categoriesCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/Categories.html',
            });

            $routeProvider.when('/cities', {
                controller: 'citiesCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/cities.html',
            });

            $routeProvider.when('/tags', {
                controller: 'tagsCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/tags.html',
            });

            $routeProvider.when('/addresses', {
                controller: 'addressesCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/Addresses.html',
            });

            $routeProvider.when('/bulkimport', {
                controller: 'bulkImportCtrlr',
                controllerAs: 'vm',
                templateUrl: '/Views/App/Admin/BulkImport.html',
            });


            $routeProvider.otherwise({ redirecTo: '/' });

            //$locationProvider.html5Mode(true);
        })

        .service('headerConfigService', function () {

            var optns = {};

            var reset = function () {
                optns.title = '';
                optns.showSearchCtrl = false;
                optns.showToolBar = false;
                optns.showRefreshBtn = true;
                optns.showAddBtn = true;
                optns.showSaveBtn = true;
                optns.refreshBtnTltp = '';
                optns.addBtnTltp = '';
                optns.saveBtnTltp = '';

                optns.tbBtnClickCallback = null;
            };

            optns = {
                reset: reset,
                title: '',
                showSearchCtrl: false,
                showToolBar: false,
                showRefreshBtn: true,
                showAddBtn: true,
                showSaveBtn: true,
                refreshBtnTltp: '',
                addBtnTltp: '',
                saveBtnTltp: '',
                tbBtnClickCallback: null
            };

            return optns;
        })

        .directive('elastic', ['$timeout', function ($timeout) {

            return {
                restrict: 'A',
                link: function ($scope, element) {

                    var target = element[0];

                    //$scope.initialHeight = $scope.initialHeight || target.style.height;

                    $scope.initialHeight = $(target).height();

                    var resize = function () {

                        if (target.scrollHeight && target.scrollHeight > 0 && $(target).val()) {

                            target.style.height = '' + target.scrollHeight + 'px';
                        }
                        else {

                            target.style.height = '' + $scope.initialHeight + 'px';
                        }
                    };

                    $(target).on('input change load ready', resize);

                    $timeout(resize, 0);
                }
            };
        }
        ]);

        //.run(function ($animate) {

        //    $animate.enabled(true);
        //});
})();