
(function () {

    'use strict';

    //Creating the module
    angular.module('app-main', ['ngRoute'])
           .config(function ($routeProvider) {

               $routeProvider.when('/', {
                   controller: 'mainCtrlr',
                   controllerAs: 'vm',
                   templateUrl: '/Views/App/Main.html',
               });

               $routeProvider.when('/profile', {
                   controller: 'profileCtrlr',
                   controllerAs: 'vm',
                   templateUrl: '/Views/App/Account/Profile.html',
               });

               $routeProvider.otherwise({ redirecTo: '/' });
           });
})();