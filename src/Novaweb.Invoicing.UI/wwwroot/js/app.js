(function() {
    "use strict";

    angular.module('Invoicing', ['auth0.lock', 'angular-jwt', 'ngRoute'])
        .config(function($routeProvider, lockProvider) {
            lockProvider.init({
                clientID: '6iy2UhvL2ymqkVRDUKPr3ntIagUHJUED',
                domain: 'novaweb.auth0.com'
            });

            $routeProvider
                .when('/',
                {
                    controller: 'homeController',
                    templateUrl: 'components/home/home.html'
                })
                .when('/login',
                {
                    controller: 'loginController',
                    templateUrl: 'components/login/login.html'
                });
        })
        .run([
            'authService', function(authService) {
                authService.registerAuthenticationListener();
            }
        ]);

})();