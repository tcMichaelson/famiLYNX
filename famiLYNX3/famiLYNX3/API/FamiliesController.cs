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
    [RoutePrefix("api/families")]
    public class FamiliesController : ApiController {
        private IRepository _repo;
        private AllServices _service;

        public FamiliesController(IRepository repo, AllServices service) {
            _repo = repo;
            _service = service;
        }

        [Route("All", Name = "Families")]
        public IList<Family> Get() {
            ApplicationUser currUser = _repo.Query<ApplicationUser>().Where(a => a.UserName == User.Identity.Name).Single();
            var famUsers = _repo.Query<FamilyUser>().Where(m => m.UserId == currUser.Id).ToList();
            var fams = new List<Family>();
            foreach (var fam in famUsers) {
                fams.Add(_repo.Query<Family>().Where(f => f.Key == fam.FamilyKey).Include(f => f.CreatedBy).Include(f => f.ConversationList.Select(g => g.MessageList)).Single());
            }
            return fams;
        }

        [Route("Selected", Name = "SelectedFamily")]
        public Family GetSelected(string key) {
            return _repo.Find<Family>(key);
        }

        [Route("Owned", Name = "OwnedFamilies")]
        public IList<Family> GetCreated() {
            ApplicationUser currUser = _repo.Query<ApplicationUser>().Where(a => a.UserName == User.Identity.Name).Single();
            var fams = _repo.Query<Family>().Where(f => f.CreatedBy.Id == currUser.Id).ToList();
            return fams;
        }

        [HttpPost]
        [Route("CreateUpdate", Name = "CreateUpdateFamily")]
        public HttpResponseMessage Post(Family family) {
            if (family.FamilyUserName == "") {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "You did not enter a Family Name.");
            }
            var currUser = _repo.Query<ApplicationUser>().Where(a => a.UserName == User.Identity.Name).Single();
            Family fam = _repo.Find<Family>(family.Key);
            if (fam == null) {
                try {
                    fam = _repo.Query<Family>().Where(f => f.FamilyUserName == family.FamilyUserName).Include(f => f.MemberList).FirstOrDefault();
                    if (fam == null) {

                        _service.SetFamilyKey(family);
                        family.CreatedBy = currUser;
                        _repo.Add<Family>(family);
                        _repo.Add<FamilyUser>(new FamilyUser { FamilyKey = family.Key, UserId = currUser.Id });
                        _repo.SaveChanges();
                        var fullFamily = _repo.Query<Family>().Where(f => f.Key == family.Key).Include(f => f.CreatedBy).Include(f => f.ConversationList.Select(g => g.MessageList)).Single();
                        return Request.CreateResponse(HttpStatusCode.Created, fullFamily);
                    }
                    if (fam.MemberList.Any(f => f.UserId == currUser.Id)) {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You are already a member of this family.");
                    }
                    fam.MemberList.Add(new FamilyUser { FamilyKey = fam.Key, UserId = currUser.Id });
                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, fam);
                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Database update failed");
                }

            } else {
                try {
                    
                    //OrgName
                    fam.OrgName = family.OrgName ?? fam.OrgName;

                    //UserName
                    fam.FamilyUserName = family.FamilyUserName ?? fam.FamilyUserName;

                    //Type
                    fam.Type = family.Type ?? fam.Type;

                    //Update ConversationList
                    if (fam.ConversationList == null) {
                        fam.ConversationList = new List<Conversation>();
                    }
                    foreach (var convo in family.ConversationList) {
                        if (!fam.ConversationList.Contains(convo)) {
                            fam.ConversationList.Add(convo);
                        }
                    }

                    //Update InviteOrPlea
                    if (fam.InviteOrPleas == null) {
                        fam.InviteOrPleas = new List<InviteOrPlea>();
                    }
                    foreach (var inv in family.InviteOrPleas) {
                        if (!fam.InviteOrPleas.Contains(inv)) {
                            fam.InviteOrPleas.Add(inv);
                        }
                    }

                    //Add the current user to the MemberList if not already there.
                    if (!(fam.MemberList.Any(m => m.UserId == currUser.Id && m.FamilyKey == fam.Key))) {
                        fam.MemberList.Add(new FamilyUser { FamilyKey = family.Key, UserId = currUser.Id });
                    }

                    //Update MemberList to include members in the model sent in.
                    if (fam.MemberList == null) {
                        fam.MemberList = new List<FamilyUser>();
                    }

                    foreach (var member in family.MemberList) {
                        if (!fam.MemberList.Contains(member)) {
                            fam.MemberList.Add(member);
                        }
                    }

                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, fam);
                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Database update failed.");
                }
            }
        }


        [Route("Delete", Name = "DeleteFamily")]
        public HttpResponseMessage Delete(string key) {
            try {
                FamilyUser famUser;
                while ((famUser = _repo.Query<FamilyUser>().Where(f => f.FamilyKey == key).FirstOrDefault()) != null) {
                    _repo.Delete<FamilyUser>(famUser.Id);
                    _repo.SaveChanges();
                }

                Message message;
                Conversation convo;
                while ((convo = _repo.Query<Conversation>().Where(c => c.WhichFam.Key == key).FirstOrDefault()) != null) {
                    while ((message = _repo.Query<Message>().Where(m => m.Conversation.Key == convo.Key).FirstOrDefault()) != null) {
                        _repo.Delete<Message>(message.Key);
                    }
                    _repo.Delete<Conversation>(convo.Key);
                    _repo.SaveChanges();
                }
                _repo.Delete<Family>(key);
                _repo.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            } catch {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
