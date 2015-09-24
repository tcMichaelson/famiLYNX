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
                fams.Add(_repo.Query<Family>().Where(f => f.Key == fam.FamilyKey).Include(f => f.CreatedBy).Include(f => f.ConversationList.Select(c => c.MessageList.Select(m => m.Contributor))).Single());
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
            Family dbFam = _repo.Find<Family>(family.Key);
            if (dbFam == null) {
                try {
                    dbFam = _repo.Query<Family>().Where(f => f.FamilyUserName == family.FamilyUserName).Include(f => f.MemberList).FirstOrDefault();

                    Family returnFam;
                    if (dbFam == null) {

                        //Family does not exists -- add family
                        dbFam = new Family {
                            CreatedBy = currUser,
                            MemberList = new List<FamilyUser> { new FamilyUser { User = currUser } },
                            OrgName = family.FamilyUserName,
                            FamilyUserName = family.FamilyUserName,
                            ConversationList = new List<Conversation>()
                        };
                        _service.SetFamilyKey(dbFam);
                        _repo.Add<Family>(dbFam);

                        returnFam = dbFam; 

                    } else if (dbFam.MemberList.Any(f => f.UserId == currUser.Id)) {
                        // Family exists and you are already a member.
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You are already a member of this family.");

                    } else {
                        // Family exists but is owned by someone else.  Add current user and include all existing conversations and messages.
                        dbFam.MemberList.Add(new FamilyUser { FamilyKey = dbFam.Key, UserId = currUser.Id });
                        returnFam = dbFam;
                        List<Conversation> dbConvos = _repo.Query<Conversation>().Where(c => c.WhichFam.Key == dbFam.Key).Include(c => c.CreatedBy).ToList();
                        if (dbConvos != null) {
                            returnFam.ConversationList = dbConvos;
                            foreach (var convo in returnFam.ConversationList) {
                                List<Message> dbMsgs = _repo.Query<Message>().Where(m => m.Conversation.Key == convo.Key).OrderBy(m => m.TimeSubmitted).Include(m => m.Contributor).ToList();
                                if (dbMsgs != null) {
                                    convo.MessageList = dbMsgs;
                                }
                            }
                        }
                    }
                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, returnFam);
                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Database update failed");
                }

            } else {
                try {
                    
                    //OrgName
                    dbFam.OrgName = family.OrgName ?? dbFam.OrgName;

                    //UserName
                    dbFam.FamilyUserName = family.FamilyUserName ?? dbFam.FamilyUserName;

                    //Type
                    dbFam.Type = family.Type ?? dbFam.Type;

                    //Update ConversationList
                    if (dbFam.ConversationList == null) {
                        dbFam.ConversationList = new List<Conversation>();
                    }
                    foreach (var convo in family.ConversationList) {
                        if (!dbFam.ConversationList.Contains(convo)) {
                            dbFam.ConversationList.Add(convo);
                        }
                    }

                    //Update InviteOrPlea
                    if (dbFam.InviteOrPleas == null) {
                        dbFam.InviteOrPleas = new List<InviteOrPlea>();
                    }
                    foreach (var inv in family.InviteOrPleas) {
                        if (!dbFam.InviteOrPleas.Contains(inv)) {
                            dbFam.InviteOrPleas.Add(inv);
                        }
                    }

                    //Add the current user to the MemberList if not already there.
                    if (!(dbFam.MemberList.Any(m => m.UserId == currUser.Id && m.FamilyKey == dbFam.Key))) {
                        dbFam.MemberList.Add(new FamilyUser { FamilyKey = family.Key, UserId = currUser.Id });
                    }

                    //Update MemberList to include members in the model sent in.
                    if (dbFam.MemberList == null) {
                        dbFam.MemberList = new List<FamilyUser>();
                    }

                    foreach (var member in family.MemberList) {
                        if (!dbFam.MemberList.Contains(member)) {
                            dbFam.MemberList.Add(member);
                        }
                    }

                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, dbFam);
                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Database update failed.");
                }
            }
        }


        [Route("Delete", Name = "DeleteFamily")]
        public HttpResponseMessage Delete(string key) {
            try {
                Family dbFam = _repo.Find<Family>(key);

                List<FamilyUser> famUsers = _repo.Query<FamilyUser>().Where(f => f.FamilyKey == key).ToList();
                foreach (var famUser in famUsers) {
                    _repo.Delete<FamilyUser>(famUser.Id);
                }
                
                List<Conversation> convos = _repo.Query<Conversation>().Where(c => c.WhichFam.Key == key).ToList();
                List<Message> messages;
                foreach (var convo in convos) {
                    messages = _repo.Query<Message>().Where(m => m.Conversation.Key == convo.Key).ToList();
                    foreach (var message in messages) {
                        _repo.Delete<Message>(message.Key);
                    }
                    _repo.Delete<Conversation>(convo.Key);
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
