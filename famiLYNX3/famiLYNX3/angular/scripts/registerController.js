(function () {
    angular
        .module('famiLYNX')
        .controller('registerController', ['accountService',
            function (accountService) {
                var self = this;

                self.userNamePref = 'useemail';

                self.register = function () {
                    accountService.register(self);
                }

            }]);
})();