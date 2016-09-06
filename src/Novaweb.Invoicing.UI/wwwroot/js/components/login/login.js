(function () {
    angular.module('Invoicing')
        .controller('loginController', loginController);

    loginController.$inject = [];
    function loginController() {
        console.debug('login controller loaded');
    }
})();