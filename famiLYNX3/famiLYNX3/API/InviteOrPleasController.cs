using famiLYNX3.Infrastructure;
using famiLYNX3.Models;
using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace famiLYNX3.API {
    [Authorize]
    [RoutePrefix("api/inviteorpleas")]
    public class InviteOrPleasController : ApiController {

        private IRepository _repo;
        private AllServices _service;

        public InviteOrPleasController(IRepository repo, AllServices service) {
            _repo = repo;
            _service = service;
        }

        [Route("All", Name = "InviteOrPleas")]
        public IList<InviteOrPlea> Get() {
            return _repo.Query<InviteOrPlea>().Where(m => m.Approver.UserName == User.Identity.Name || m.Inviter.UserName == User.Identity.Name || m.Pleader.UserName == User.Identity.Name).ToList();
        }

        [HttpPost]
        [Route("CreateUpdate", Name = "invPost")]        
        public HttpResponseMessage Post(InviteOrPlea invite) {
            //None = 0, Yes = 1, No = 2
            InviteOrPlea inv = _repo.Find<InviteOrPlea>(invite.Key);
            if (inv == null) {
                //try {
                //    var currUser = _service.GetCurrentUser(User.Identity.Name);
                //    var family = _repo.Query<Family>().Where(f => f.FamilyUserName == invite.Family.FamilyUserName).Single();
                //    var members = _repo.Query<FamilyUser>().Where(m => m.Family == family).ToList();

                //    //check to see if the user is already a memeber of this family.
                //    if (members.Any(m => m.User == currUser)) {
                //        return Request.CreateResponse(HttpStatusCode.Conflict, invite);
                //    } else { }
                //    if(family.MemberList.Contains(currUser.Id))
                //    _service.SetInviteOrPleaKey(invite);

                //    invite.EmailAddress = currUser.Email;
                //    invite.
                //    if (ModelState.IsValid) {
                //        _repo.Add<InviteOrPlea>(invite);
                //        _repo.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.Created, invite);
                //    } else {
                //        return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                //    }
                //} catch {
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, invite);
                //}

            } else {
                try {
                    inv.Approved = invite.Approved;
                    inv.UserResponse = invite.UserResponse;
                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, invite);
                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, invite);
                }
            }
        }

        [HttpPost]
        public HttpResponseMessage Delete(string key) {
            _repo.Delete<InviteOrPlea>(key);
            _repo.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
