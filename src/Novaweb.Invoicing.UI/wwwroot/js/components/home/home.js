(function() {
    angular.module('Invoicing')
        .controller('homeController', homeController);

    homeController.$inject = [];
    function homeController() {
        console.debug('home controller loaded');
    }
})();