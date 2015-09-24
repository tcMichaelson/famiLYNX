(function () {
    angular
        .module('famiLYNX')
        .controller('registerController', ['accountService',
            function (accountService) {
                var self = this;

                self.register = function () {
                    accountService.register(self);
                };

            }]);
})();