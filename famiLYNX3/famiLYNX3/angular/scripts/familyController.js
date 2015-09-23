(function () {
    angular
    .module('famiLYNX')
    .controller('familyController'), function () {

        var self = this;

        self.user = profileService.getUser();
        self.selectedFamily = profileService.selectedFamily();
        self.convos = profileService.getConversations();
        self.inviteOrPleas = profileService.getInviteOrPleas();


    }
})();