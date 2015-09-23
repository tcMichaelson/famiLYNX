(function () {

    angular
        .module('famiLYNX')
        .controller('loginController', ['$http','$location', function ($http, $location) {
            
            var self = this;

            self.login = function () {
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
                    .error(function () {
                        console.error('Error logging in.');
                    });
            }

        }]);

})();