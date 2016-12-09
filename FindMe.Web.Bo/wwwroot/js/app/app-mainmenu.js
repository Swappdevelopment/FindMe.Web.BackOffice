
(function () {

    'use strict';

    //Creating the module
    angular.module('app-mainmenu', ['ngRoute'])
           .config(function ($routeProvider) {

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

               $routeProvider.when('/addresses', {
                   controller: 'addressesCtrlr',
                   controllerAs: 'vm',
                   templateUrl: '/Views/App/Admin/Addresses.html',
               });

               $routeProvider.otherwise({ redirecTo: '/' });
           })
        .service('headerConfigService', function () {

            var optns;

            var reset = function () {
                optns.title = '';
                optns.showSearchCtrl = false;
                optns.showToolBar = false;
                optns.showRefreshBtn = true;
                optns.showAddBtn = true;
                optns.showSaveBtn = true;
            };

            optns = {
                reset: reset,
                title: '',
                showSearchCtrl: false,
                showToolBar: false,
                showRefreshBtn: true,
                showAddBtn: true,
                showSaveBtn: true
            };

            return optns;
        });
})();