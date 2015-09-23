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
    [Route("api/conversations", Name = "conversations")]
    public class ConversationsController : ApiController {

        private IRepository _repo;
        private AllServices _service;

        public ConversationsController(IRepository repo, AllServices service) {
            _repo = repo;
            _service = service;

        }

        public IList<Conversation> Get(Family family) {
            return _repo.Query<Conversation>().Where(c => c.WhichFam == family).ToList();
        }

        public HttpResponseMessage Post(Conversation conversation) {
            var convo = _repo.Find<Conversation>(conversation.Key);
            if (convo == null) {
                try {
                    ApplicationUser currUser = _repo.Query<ApplicationUser>().Where(a => a.UserName == User.Identity.Name).Single();
                    Family family = _repo.Find<Family>(conversation.WhichFam.Key);
                    convo = new Conversation {
                        CreatedBy = currUser,
                        CreatedDate = DateTime.Now,
                        IsEvent = false,
                        Recurs = false,
                        Topic = conversation.Topic,
                        WhichFam = family
                    };
                    _service.SetConvoKey(convo);
                    if(family.ConversationList == null) {
                        family.ConversationList = new List<Conversation>();
                    }
                    family.ConversationList.Add(convo);
                    _repo.Add<Conversation>(convo);
                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, convo);

                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            } else {
                try {
                    //Attenders
                    if (convo.Attenders == null) {
                        convo.Attenders = new List<ConversationsAttendedByMembers>();
                    }
                    foreach (var user in conversation.Attenders) {
                        if (!convo.Attenders.Contains(user)) {
                            convo.Attenders.Add(user);
                        }
                    }

                    //ExpirationDate
                    convo.ExpirationDate = conversation.ExpirationDate ?? convo.ExpirationDate;

                    //IsEvent
                    convo.IsEvent = conversation.IsEvent;

                    //MessageList
                    if (convo.MessageList == null) {
                        convo.MessageList = new List<Message>();
                    }
                    foreach (var msg in conversation.MessageList) {
                        if (!convo.MessageList.Contains(msg)) {
                            convo.MessageList.Add(msg);
                        }
                    }

                    //Recurs
                    convo.Recurs = conversation.Recurs;

                    //VisibleTo
                    if (convo.VisibleTo == null) {
                        convo.VisibleTo = new List<ConversationsVisibleToMembers>();
                    }
                    foreach (var user in conversation.VisibleTo) {
                        if (!convo.VisibleTo.Contains(user)) {
                            convo.VisibleTo.Add(user);
                        }
                    }

                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, convo);

                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
        }

        public HttpResponseMessage Delete(string key) {
            try {
                _repo.Delete<Conversation>(key);
                _repo.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            } catch {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

    }
}
