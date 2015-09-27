(function () {
    angular
        .module('famiLYNX')
        .controller('registerController', ['accountService',
            function (accountService) {
                var self = this;

                self.validEmail === false;
                self.emailString = /"is_valid": false/;
                self.errormessage = [];

                self.register = function () {
                    self.errormessage = accountService.register(self);
                };

                self.validateEmail = function (currScope) {
                    currScope.emailResults = accountService.validateEmail(self.Email);
                };

            }]);
})();