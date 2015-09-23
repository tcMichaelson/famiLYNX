(function () {
    angular
        .module('famiLYNX')
        .factory('familyCreator', ['$resource', 'routeUrls', function ($resource, routeUrls) {
            var self = this;

            var getFam = $resource(routeUrls.famsPost, {}, {});
            var getPlea = $resource(routeUrls.invApi, {}, {});

            return {
                newFam: function (family) {
                    return new getFam(family).$save();
                }
            }
        }]);
})();

(function() {
    angular
        .module('famiLYNX')
        .factory('familyDeleter', ['$resource', 'routeUrls', function ($resource, routeUrls) {
            var self = this;

            var delFam = $resource(routeUrls.famsDelete, {}, {});

            return {

                deleteFamily: function (family) {
                    return new delFam().$remove({ key: family.Key });
                }
            }

        }]);
})();

(function () {
    angular
        .module('famiLYNX')
        .factory('convoCreator', ['$resource', 'routeUrls', function ($resource, routeUrls) {
            var self = this;

            var convoApi = $resource(routeUrls.convoApi, {}, {});

            return {

                addConvo: function (conversation) {
                    return new convoApi(conversation).$save();
                }
            }

        }]);
})();

(function () {
    angular
        .module('famiLYNX')
        .factory('msgCreator', ['$resource', 'routeUrls', function ($resource, routeUrls) {
            var self = this;

            var msgApi = $resource(routeUrls.msgApi, {}, {});

            return {

                addMsg: function (text, convo) {
                    return new msgApi({ Text: text, Conversation: convo}).$save();
                }
            }

        }]);
})();