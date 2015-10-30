(function () {
    angular
        .module('famiLYNX')
        .service('profileService', ['$resource', 'routeUrls', function ($resource, routeUrls) {
            var self = this;

            var profileApi = $resource(routeUrls.userApi, {}, {});  //UsersController/Currentuser (Get(int id))
            var updateUserApi = $resource(routeUrls.updateUser, {}, {}); //Userscontroller/UpdateUser (Post)
            var getFam = $resource(routeUrls.famApi, {}, {}); //FamiliesController/Families (Get)
            var getConvo = $resource(routeUrls.convoApi, {}, {}); //ConversationsController
            var getMsg = $resource(routeUrls.msgApi, {}, {}); //MessagesController
            var getInvite = $resource(routeUrls.invApi, {}, {}); //InviteOrPleasController/InviteOrPleas (Get)
            var ownedFamilies = $resource(routeUrls.famsOwnedApi, {}, {}); //FamiliesController/OwnedFamilies
            var postFamilies = $resource(routeUrls.famsPost, {}, {}); //FamiliesController/CreateUpdateFamily
            var logout = $resource(routeUrls.logOut, {}, {}); //AccountsController/Logout

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
                            convo.MessageList = convo.MessageList.sort(function (first, second) {
                                if (second.TimeSubmitted < first.TimeSubmitted) {
                                    return -1;
                                } else {
                                    return 1;
                                }
                            });
                            console.log(convo.MessageList);
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