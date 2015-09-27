(function () {
    angular
        .module('famiLYNX')
        .service('accountService', ['$http','$location','$resource', 'routeUrls', function ($http, $location, $resource, routeUrls) {
            var self = this;

            var registerApi = $resource(routeUrls.registerUser);

            var errMessage = [];

            self.register = function (user) {
                return new registerApi(user).$save(function (data) {
                    $http.post('/Token', "grant_type=password&username=" + user.Email + "&password=" + user.Password,
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
                    user.errormessage = xhr.data.ModelState[''];
                    
                });
            };

            self.validateEmail = function (email) {

                return $resource('/API/ValidateEmail/validate').get({ email: email });

                
                //$http.post('https://api.mailgun.net/v3/address/validate', { adresses: "test@gmail.com", api_key: "pubkey-4589ea7c99231f892c2304bcb754c08c" },
                    
                //    {
                //        headers: {
                //            'Accept': '*/*',
                //            'Content-Type': 'application/json',
                //            'Access-Control-Allow-Origin': 'http://localhost:60532'
                //        }
                //    })
                //    .then(function (data) {
                //        console.log(data);
                //    });
            };
        }]);
})();