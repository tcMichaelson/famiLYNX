(function () {
    angular
    .module('famiLYNX')
    .controller('profileController', ['familyGetter', 'profileService', 'familyCreator', 'familyDeleter', 'convoCreator', 'msgCreator',
        function (familyGetter, profileService, familyCreator, familyDeleter, convoCreator, msgCreator) {

            var self = this;

            self.user = profileService.getUser();
            self.convos = profileService.getConversations();
            self.inviteOrPleas = profileService.getInviteOrPleas();
            self.ownedFamilies = profileService.getOwned();
            self.families = profileService.getFamilies();
            self.page = 'profile';
            self.Editing = 'n';

            self.createFamily = function () {
                familyCreator.newFam({ OrgName: self.families.newFamilyName, FamilyUserName: self.families.newFamilyName })
                    .then(function (data) {
                        self.ownedFamilies.push(data);
                        self.families.push(data);
                        self.families.newFamilyName = null;
                    });
            };

            self.isFamUName = function (uName) {
                var newList = [];
                newList = self.families.forEach(function (item) {
                    return item.FamilyUserName === uName;
                });
                return newList;
            };

            self.updateUserInfo = function () {
                profileService.updateUserInfo(self.user);
            };

            self.deleteFamily = function (family) {
                familyDeleter.deleteFamily(family).then(function (data) {
                    self.families = self.families.filter(function (fam) {
                        return fam !== family;
                    });
                });
            };

            self.addConvo = function () {
                convoCreator.addConvo({ Topic: self.newTopic, WhichFam: self.selectedFamily }).then(function (data) {
                    if (self.selectedFamily.ConversationList === null) {
                        self.selectedFamily.ConversationList = [];
                    }
                    self.selectedFamily.ConversationList.push(data);
                    self.newTopic = null;
                    console.log(data);
                });
            }

            self.addMessage = function (message, convo) {
                msgCreator.addMsg(message, convo).then(function (data) {
                    console.log(data);
                    if (self.selectedFamily.currentConvo.MessageList === null) {
                        self.selectedFamily.currentConvo.MessageList = [];
                    }
                    self.selectedFamily.currentConvo.MessageList.unshift(data);
                    self[convo.Key].newMessage = null;
                });
            }

            self.logout = function () {
                profileService.logout();
            }

            
        }]);
})();