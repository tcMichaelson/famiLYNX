(function () {
    angular
        .module('famiLYNX')
        .service('accountService', ['$http','$location','$resource', 'routeUrls', function ($http, $location, $resource, routeUrls) {
            var self = this;

            var registerApi = $resource(routeUrls.registerUser);

            self.register = function (user) {
                new registerApi(user).$save(function (data) {
                    $http.post('/Token', "grant_type=password&username=" + self.username + "&password=" + self.password,
                    {
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded'
                        }
                    })
                    .success(function (data) {
                        token = data.access_token;
                        $http.defaults.headers.common.Authorization = 'Bearer ' + token;
                        $location.path('/profile');
                    })
                    .error(function (error) {
                        console.log("Something went wrong");
                    });
                }, function (xhr) {
                    return xhr.data.ModelState[''];
                });
            };
        }]);
})();