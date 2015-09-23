(function () {
    angular
        .module('famiLYNX')
        .service('accountService', function ($location, $resource, routeUrls) {
            var self = this;

            var registerApi = $resource(routeUrls.registerUser);

            self.register = function (user) {
                new registerApi(user).$save(function (data) {
                    location.hash = '#/profile';
                });
            };
        });
})();