(function () {

    angular
        .module('famiLYNX', ['ngRoute', 'ngResource'])
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: '/angular/views/Login.html',
                    controller: 'loginController',
                    controllerAs: 'self'
                })
                .when('/register', {
                    templateUrl: '/angular/views/Register.html',
                    controller: 'registerController',
                    controllerAs: 'self'
                })
                .when('/profile', {
                    templateUrl: '/angular/views/Profile.html',
                    controller: 'profileController',
                    controllerAs: 'self'
                })
                .when('/family/:id', {
                    templateUrl: '/angular/view/Family.html',
                    controller: 'familyController',
                    controllerAs: 'self'
                })
                .otherwise({
                    templateUrl: '/angular/views/NoSuchPage.html'
                });
        }]);
})();