using famiLYNX3.Infrastructure;
using famiLYNX3.Models;
using famiLYNX3.Models.ViewModels;
using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace famiLYNX3.API {
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController {

        IRepository _repo;
        AllServices _service;

        public UsersController(IRepository repo, AllServices service) {
            _repo = repo;
            _service = service;
        }

        [Route("CurrentUser", Name = "CurrentUser")]
        public ApplicationUser Get(int id) {
            return _repo.Query<ApplicationUser>().Where(a => a.UserName == User.Identity.Name).Include(a => a.UserAddress).Single();
        }

        [Route("UpdateUser", Name = "UpdateUser")]
        public HttpResponseMessage Post(ApplicationUser applicationUser) {
            var user = _repo.Find<ApplicationUser>(applicationUser.Id);
            try {

                //Attending
                if (user.Attending == null) {
                    user.Attending = new List<ConversationsAttendedByMembers>();
                }
                if (applicationUser.Attending != null) {
                    foreach (var convo in applicationUser.Attending) {
                        if (!user.Attending.Contains(convo)) {
                            user.Attending.Add(convo);
                        }
                    }
                }

                //Contributions
                if (user.Contributions == null) {
                    user.Contributions = new List<Message>();
                }
                if (applicationUser.Contributions != null) {
                    foreach (var msg in applicationUser.Contributions) {
                        if (!user.Contributions.Contains(msg)) {
                            user.Contributions.Add(msg);
                        }
                    }
                }

                //Email
                user.Email = applicationUser.Email ?? user.Email;

                //Families
                if (user.Familys == null) {
                    user.Familys = new List<FamilyUser>();
                }
                if (applicationUser.Familys != null) {
                    foreach (var fam in applicationUser.Familys) {
                        if (!user.Familys.Contains(fam)) {
                            user.Familys.Add(fam);
                        }
                    }
                }

                //First name
                user.FirstName = applicationUser.FirstName ?? user.FirstName;

                //Invites
                if (user.Invites == null) {
                    user.Invites = new List<InviteOrPlea>();
                }
                if (applicationUser.Invites != null) {
                    foreach (var inv in applicationUser.Invites) {
                        if (!user.Invites.Contains(inv)) {
                            user.Invites.Add(inv);
                        }
                    }
                }

                //Last Name
                user.LastName = applicationUser.LastName ?? user.LastName;

                //OrgRoles
                if (user.OrgRoles == null) {
                    user.OrgRoles = new List<OrgRole>();
                }
                if (applicationUser.OrgRoles != null) {
                    foreach (var oRole in applicationUser.OrgRoles) {
                        if (!user.OrgRoles.Contains(oRole)) {
                            user.OrgRoles.Add(oRole);
                        }
                    }
                }

                //Pleas
                if (user.Pleas == null) {
                    user.Pleas = new List<InviteOrPlea>();
                }
                if (applicationUser.Pleas != null) {
                    foreach (var plea in applicationUser.Pleas) {
                        if (!user.Pleas.Contains(plea)) {
                            user.Pleas.Add(plea);
                        }
                    }
                }

                //ToApprove
                if (user.ToApprove == null) {
                    user.ToApprove = new List<InviteOrPlea>();
                }
                if (applicationUser.ToApprove != null) {
                    foreach (var app in applicationUser.ToApprove) {
                        if (!user.ToApprove.Contains(app)) {
                            user.ToApprove.Add(app);
                        }
                    }
                }

                //UserAddress
                if (applicationUser.UserAddress == null) {
                    applicationUser.UserAddress = new Address();
                    
                }
                if (user.UserAddress == null) {
                    var uAddress = new Address();
                    _service.SetAddressKey(uAddress);
                    user.UserAddress = uAddress;
                }

                user.UserAddress.City = applicationUser.UserAddress.City ?? user.UserAddress.City ?? "";
               
                user.UserAddress.Street = applicationUser.UserAddress.Street ?? user.UserAddress.Street ?? "";
                
                user.UserAddress.State = applicationUser.UserAddress.State ?? user.UserAddress.State ?? "";
            
                user.UserAddress.Zip = applicationUser.UserAddress.Zip ?? user.UserAddress.Zip ?? "";
                
                
                //Visible Conversations
                if(user.VisibleConversations == null) {
                    user.VisibleConversations = new List<ConversationsVisibleToMembers>();
                }
                if (applicationUser.VisibleConversations != null) {
                    foreach (var convo in applicationUser.VisibleConversations) {
                        if (!user.VisibleConversations.Contains(convo)) {
                            user.VisibleConversations.Add(convo);
                        }
                    }
                }

                _repo.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, applicationUser);

            } catch {
                return Request.CreateResponse(HttpStatusCode.BadRequest, applicationUser);
            }


        }

    }

    //Post Methods are handled by Accounts Manager

}
