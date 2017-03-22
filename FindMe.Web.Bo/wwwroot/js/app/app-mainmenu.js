﻿
(function () {

    'use strict';

    //Creating the module
    angular.module('app-mainmenu', ['ngRoute', 'ui.bootstrap'])
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

               $routeProvider.when('/addresses', {
                   controller: 'addressesCtrlr',
                   controllerAs: 'vm',
                   templateUrl: '/Views/App/Admin/Addresses.html',
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
        });
})();