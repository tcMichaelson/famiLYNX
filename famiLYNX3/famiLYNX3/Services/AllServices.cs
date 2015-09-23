using AutoMapper;
using famiLYNX3.Infrastructure;
using famiLYNX3.Models;
using famiLYNX3.Models.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class AllServices {

        private IRepository _repo;
        private static Random _rndNum = new Random();

        public AllServices(IRepository repo) {
            _repo = repo;
        }

        //Old Code
        #region
        /*
        public void CreateMessage(string msgText, string contributorUserName, int conversationId) {
            Message newMessage = new Message();
            newMessage.Contributor = GetMemberByUserName(contributorUserName);
            newMessage.Conversation = _repo.Find<Conversation>(conversationId);
            newMessage.Text = msgText;
            newMessage.TimeSubmitted = DateTime.Now;
            _repo.Add<Message>(newMessage);
            _repo.SaveChanges();
        }

        
        public void CreateConversation(CreateConversationViewModel model) {
            var currUser = GetMemberByUserName(model.UserName);
            Conversation newConvo = new Conversation {
                Topic = model.NewTopic,
                CreatedBy = currUser,
                WhichFam = _repo.Find<Family>(model.FamKey),
                IsEvent = model.IsEvent,
                Recurs = model.Recurs,
                ExpirationDate = model.ExpirationDate,
                CreatedDate = DateTime.Now,
                MessageList = new List<Message>(),
                VisibleTo = new List<ConversationsVisibleToMembers>() { new ConversationsVisibleToMembers { MemberId = currUser.Id } },
                Attenders = new List<ConversationsAttendedByMembers> { new ConversationsAttendedByMembers { MemberId = currUser.Id } }
            };
            if (model.FirstMessage != null && model.FirstMessage != "") {
                Message newMessage = new Message { Text = model.FirstMessage, Contributor = newConvo.CreatedBy, Conversation = newConvo, TimeSubmitted = DateTime.Now };
                newConvo.MessageList.Add(newMessage);
                _repo.Add<Message>(newMessage);
            }
            _repo.Add<Conversation>(newConvo);
            _repo.SaveChanges();
        }

        public bool CreateFamily(string orgName, string currUser, string famUserName = "") {
            var creator = _repo.Query<ApplicationUser>().Where(m => m.UserName == currUser).Include(m => m.Familys).FirstOrDefault();
            if (_repo.Query<Family>().Where(m => m.OrgName == orgName).FirstOrDefault() == null) {
                var newFam = new Family {
                    OrgName = orgName,
                    FamilyUserName = orgName,
                    CreatedBy = creator,
                    MemberList = new List<FamilyUser> { new FamilyUser { UserId = creator.Id } }
                };
                _repo.Add<Family>(newFam);
                _repo.SaveChanges();
                return true;
            }
            return false;
        }

        //List method -- need Member and Family
        public FamilyViewModel PopulateFamilyViewModel(string userID, string famName) {
            ApplicationUser member = GetMemberByUserName(userID);
            Family fam = GetFamilyByUserName(famName);
            var convoMemberVisibles = _repo.Query<ConversationsVisibleToMembers>().Where(m => m.MemberId == member.Id).Select(m => m.ConversationKey).ToList();

            var dtoConvoList = new List<ConversationDTO>();
            foreach (var item in convoMemberVisibles) {
                var convo = _repo.Query<Conversation>().Where(m => m.Key == item).Include(m => m.MessageList).FirstOrDefault();

                if (convo.WhichFam == fam) {
                    dtoConvoList.Add(Mapper.Map<ConversationDTO>(convo));
                }
            }
            return new FamilyViewModel { ConversationList = dtoConvoList, FamilyKey = fam.Key, FamilyName = fam.OrgName, UserName = userID };
        }

        public ProfileViewModel GetProfileViewModel(string userName) {
            var user = GetDTOMemberByUserName(userName);
            var familys = GetFamilysByMemberUserId(GetMemberIdByUserName(userName));
            return new ProfileViewModel { Familys = familys, User = user };
        }

        public EditProfileViewModel GetEditProfileViewModel(string userName) {
            var user = GetDTOMemberByUserName(userName);
            var address = GetUserAddress(userName);
            var vm = new EditProfileViewModel {
                City = address.City,
                State = (Models.StName)address.State,
                Street = address.Street,
                User = user,
                Zip = address.Zip
            };
            return vm;
        }

        private bool IsMemberInFamily(ApplicationUser member, Family family) {
            //if (member != null && family != null) {
            //    return member.Familys.Contains(family);
            //}
            return false;
        }

        //List method - 
        public ConversationViewModel GetConversationData(string conversationKey, string familyKey, string familyName, string userName) {
            ConversationViewModel vm = new ConversationViewModel {
                Conversation = Mapper.Map<ConversationDTO>((from c in _repo.Query<Conversation>() where c.Key == conversationKey select c).Include(c => c.MessageList.Select(d => d.Contributor)).FirstOrDefault()),
                FamilyKey = familyKey,
                FamilyName = familyName,
                UserName = userName
            };
            return (vm);
        }

        public string GetFamilyNameById(string key) {
            return (from m in _repo.Query<Family>() where m.Key == key select m.OrgName).FirstOrDefault();
        }

        public ApplicationUserDTO GetDTOMemberByUserName(string userName) {
            var memberInfo = _repo.Query<ApplicationUser>().Where(m => m.UserName == userName).FirstOrDefault();
            var memberToPass = Mapper.Map<ApplicationUserDTO>(memberInfo);
            memberToPass.UserAddress = GetUserAddress(userName);
            var listOfInvites = _repo.Query<InviteOrPlea>().Where(m => m.Approver.UserName == memberToPass.UserName).Include(m => m.Family).ToList();
            foreach (var item in listOfInvites) {
                memberToPass.ToApprove.Add(Mapper.Map<InviteOrPleaDTO>(item));
            }
            return memberToPass;
        }

        public ApplicationUser GetMemberByUserName(string userName) {
            return _repo.Query<ApplicationUser>().Where(m => m.UserName == userName).FirstOrDefault();
        }

        public string GetMemberIdByUserName(string userName) {
            return _repo.Query<ApplicationUser>().Where(m => m.UserName == userName).Select(m => m.Id).FirstOrDefault();
        }

        public Family GetFamilyById(int id) {
            return _repo.Find<Family>(id);
        }

        public Family GetFamilyByUserName(string famName) {
            return _repo.Query<Family>().Where(f => f.FamilyUserName.ToLower() == famName.ToLower()).Include(f => f.MemberList).FirstOrDefault();
        }

        public List<FamilyDTO> GetFamilysByMemberUserId(string userName) {
            var ListOfFamilyUsers = _repo.Query<FamilyUser>().Where(m => m.UserId == userName).ToList();
            List<FamilyDTO> famDTOList = new List<FamilyDTO>();
            foreach (var item in ListOfFamilyUsers) {
                famDTOList.Add(Mapper.Map<FamilyDTO>(_repo.Find<Family>(item.FamilyKey)));
            }
            return famDTOList;
        }

        public void UpdateUserProfile(EditProfileViewModel currUser) {
            ApplicationUser userToUpdate = _repo.Query<ApplicationUser>().Where(m => m.UserName == currUser.User.UserName).FirstOrDefault();
            userToUpdate.FirstName = currUser.User.FirstName;
            userToUpdate.LastName = currUser.User.LastName;
            userToUpdate.Email = currUser.User.Email;
            userToUpdate.UserAddress = new Address {
                Street = currUser.Street,
                City = currUser.City,
                State = (Models.StName)currUser.State,
                Zip = currUser.Zip
            };
            _repo.SaveChanges();

        }

        public string ManageInviteOrPleas(string famUserName, string email, string currUserName, Response approved = Response.None, Response userResponse = Response.None) {

            var msg = "";

            var invite = _repo.Query<InviteOrPlea>().Where(m => m.Family.FamilyUserName == famUserName && m.EmailAddress == email).FirstOrDefault();
            if (invite != null) {
                if (invite.Approved == Models.Response.Yes && (approved == Response.Yes || approved == Response.None)) {
                    return "There already exists an invite for this user.";
                } else if (invite.UserResponse == Models.Response.Yes && userResponse == Response.Yes) {
                    return "There already exists a request from you.";
                } else if (approved == Response.Yes) {
                    invite.Approved = Models.Response.Yes;
                } else if (userResponse == Response.Yes) {
                    invite.UserResponse = Models.Response.Yes;
                }

                if (invite.Approved == Models.Response.Yes && invite.UserResponse == Models.Response.Yes) {
                    msg = AddUserToFamily(famUserName, email, currUserName);
                    _repo.Delete<InviteOrPlea>(invite.Key);
                }

            } else {

                if (userResponse == Response.Yes) {
                    msg = CreatePlea(famUserName, email, currUserName);
                } else if (userResponse == Response.None) {
                    msg = CreateInvite(famUserName, email, currUserName);
                } else if (approved == Response.No || userResponse == Response.No) {
                    msg = DenyInviteOrPlea(famUserName, email, currUserName);
                }
            }

            _repo.SaveChanges();

            return msg;
        }

        public AddressDTO GetUserAddress(string userName) {
            var addressToPass = Mapper.Map<AddressDTO>((from m in _repo.Query<ApplicationUser>() where m.UserName == userName select m.UserAddress).FirstOrDefault());
            return addressToPass ?? new AddressDTO { City = "No City Info", State = StName.None, Street = "No Street Info", Zip = "00000" };
        }

        public void UpdateUserAddress(ApplicationUserDTO user, AddressDTO currAddress) {
            Address newAddress = new Address {
                City = currAddress.City,
                State = (Models.StName)currAddress.State,
                Street = currAddress.Street,
                Zip = currAddress.Zip
            };
        }

        private string AddUserToFamily(string famUserName, string email, string currName) {
            Family fam = GetFamilyByUserName(famUserName);
            ApplicationUser userToAdd = _repo.Query<ApplicationUser>().Where(m => m.Email == email).FirstOrDefault();
            if (fam.MemberList.Count() > 0) {
                fam.MemberList.Add(new FamilyUser { User = userToAdd });
            } else {
                fam.MemberList = new List<FamilyUser> { new FamilyUser { User = userToAdd } };
            }

            return "Member has been added to the family.";
        }

        private string CreateInvite(string famUserName, string email, string currUserName) {
            IQueryable<Family> fam = _repo.Query<Family>().Where(m => m.FamilyUserName == famUserName);
            ApplicationUser approver = fam.Select(g => g.CreatedBy).FirstOrDefault();
            ApplicationUser inviter = _repo.Query<ApplicationUser>().Where(m => m.UserName == currUserName).FirstOrDefault();
            ApplicationUser invitee = _repo.Query<ApplicationUser>().Where(m => m.Email == email).FirstOrDefault();
            IList<FamilyUser> memberList = fam.Select(m => m.MemberList).FirstOrDefault();

            if (memberList.Any(m => m.UserId == invitee.Id)) {
                return "This user is already a member of this family.";
            } else if (_repo.Query<InviteOrPlea>().ToList().Any(m => m.EmailAddress == email && m.Family.FamilyUserName == famUserName)) {
                return "There is already a pending invite or plea for this user.";
            } else {
                var newFam = fam.FirstOrDefault();
                var newPlea = new InviteOrPlea {
                    //EmailAddress = email,
                    Approved = inviter == approver ? Models.Response.Yes : Models.Response.None,
                    UserResponse = Models.Response.None,
                    Family = newFam,
                    Approver = approver,
                    Inviter = inviter,
                    Pleader = invitee
                };
                _repo.Add<InviteOrPlea>(newPlea);
                return "An invitation has been sent to this user";
            }
        }

        private string CreatePlea(string famUserName, string email, string currUserName) {
            IQueryable<Family> fam = _repo.Query<Family>().Where(m => m.FamilyUserName == famUserName);
            ApplicationUser approver = fam.Select(g => g.CreatedBy).FirstOrDefault();
            ApplicationUser currUser = GetMemberByUserName(currUserName);
            IList<FamilyUser> memberList = fam.Select(m => m.MemberList).FirstOrDefault();

            if (memberList.Any(m => m.UserId == currUser.Id)) {
                return "You are already a member of this family.";
            } else if (_repo.Query<InviteOrPlea>().ToList().Any(m => m.EmailAddress == email && m.Family.FamilyUserName == famUserName)) {
                return "There already exists a request from you.";
            } else {
                var newFam = fam.FirstOrDefault();
                var newPlea = new InviteOrPlea {
                    EmailAddress = email,
                    Approved = Models.Response.None,
                    UserResponse = Models.Response.Yes,
                    Family = newFam,
                    Approver = approver,
                    Pleader = currUser,
                };
                _repo.Add<InviteOrPlea>(newPlea);
                return "A request has been submitted for you";
            }
        }

        private string DenyInviteOrPlea(string famUserName, string email, string currUserName) {
            var idToDelete = _repo.Query<InviteOrPlea>().Where(m => m.Family.FamilyUserName == famUserName && m.EmailAddress == email).Select(m => m.Key).FirstOrDefault();
            _repo.Delete<InviteOrPlea>(idToDelete);
            return "Invite or Plea has been denied.";
        }

        */

        #endregion
        //Old Code

        public static void RegisterServices(IKernel kernel) {
            NinjectInfrastructureConfig.RegisterInfrastructure(kernel);

        }

        public ApplicationUser GetCurrentUser(string uName) {
            return _repo.Query<ApplicationUser>().Where(a => a.UserName == uName).Single();
        }
                
        public void SetFamilyKey(Family family) {
            family.Key = "F" + GetKey();
        }

        public void SetConvoKey(Conversation convo) {
            convo.Key = "C" + GetKey();
        }

        public void SetMessageKey(Message msg) {
            msg.Key = "M" + GetKey();
        }

        public void SetInviteOrPleaKey(InviteOrPlea inv) {
            inv.Key = "I" + GetKey();
        }

        public void SetAddressKey(Address address) {
            address.Key = "A" + GetKey();
        }

        public void SetOrgRoleKey(OrgRole orgRole) {
            orgRole.Key = "O" + GetKey();
        }

        //A - Z are 65 to 90
        //a - z are 97 to 122
        //0 - 9 are 48 to 57
        private string GetKey() {
            string rndKey = "";
            for (var i = 0; i < 15; i++) {
                switch (_rndNum.Next(1, 4)) {
                    case 1:
                        rndKey += (Char)_rndNum.Next(65, 91);
                        break;
                    case 2:
                        rndKey += (Char)_rndNum.Next(97, 123);
                        break;
                    case 3:
                        rndKey += (Char)_rndNum.Next(48, 58);
                        break;
                }
            }
            return rndKey;
        }
    }
}