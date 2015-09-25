(function () {
    angular
        .module('famiLYNX')
        .service('profileService', ['$resource', 'routeUrls', function ($resource, routeUrls) {
            var self = this;
            var profileApi = $resource(routeUrls.userApi, {}, {});
            var updateUserApi = $resource(routeUrls.updateUser, {}, {});
            var getFam = $resource(routeUrls.famApi, {}, {});
            var getConvo = $resource(routeUrls.convoApi, {}, {});
            var getMsg = $resource(routeUrls.msgApi, {}, {});
            var getInvite = $resource(routeUrls.invApi, {}, {});
            var ownedFamilies = $resource(routeUrls.famsOwnedApi, {}, {});
            var postFamilies = $resource(routeUrls.famsPost, {}, {});
            var logout = $resource(routeUrls.logOut, {}, {});

            self.getUser = function () {
                return profileApi.get({ id: 0 });
            }

            self.getOwned = function () {
                return ownedFamilies.query();
            }

            self.getFamilies = function () {
                return getFam.query(function (data) {
                    return data.forEach(function (fam) {
                        fam.ConversationList.forEach(function (convo) {
                            convo.MessageList.sort(function (first, second) {
                                return first.TimeSubmitted < second.TimeSubmitted;
                            });
                        });
                    });
                });
            };
            self.getConversations = function () {
                return getConvo.query();
            };
            self.getInviteOrPleas = function () {
                return getInvite.query();
            };

            self.createFamily = function (family) {
                var newFam = new postFamilies(family);
                newFam.$save();
            }

            self.updateUserInfo = function (user) {
                var newUser = new updateUserApi(user);
                newUser.$save();
            }

            self.logout = function () {
                logout.save();
            }

        }]);
})();