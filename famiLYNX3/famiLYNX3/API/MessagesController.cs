using famiLYNX3.Infrastructure;
using famiLYNX3.Models;
using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace famiLYNX3.API
{
    [Authorize]
    [Route("api/messages", Name = "messages")]
    public class MessagesController : ApiController
    {

        private IRepository _repo;
        private AllServices _service;

        public MessagesController(IRepository repo, AllServices service) {
            _repo = repo;
            _service = service;
        }

        public IList<Message> Get(string convoKey) {
            return _repo.Query<Message>().Where(m => m.Conversation.Key == convoKey).ToList();
        }

        public HttpResponseMessage Post(Message message) {
            var msg = _repo.Find<Message>(message.Key);
            if (msg == null) {
                try {
                    ApplicationUser currUser = _repo.Query<ApplicationUser>().Where(a => a.UserName == User.Identity.Name).Single();
                    Conversation currConvo = _repo.Find<Conversation>(message.Conversation.Key);
                    _service.SetMessageKey(message);
                    message.Contributor = currUser;
                    message.TimeSubmitted = DateTime.Now;
                    message.Conversation = currConvo;
                    if(currConvo.MessageList == null) {
                        currConvo.MessageList = new List<Message>();
                    }
                    ModelState.Clear();
                    if (ModelState.IsValid) {
                        _repo.Add<Message>(message);
                        currConvo.MessageList.Add(message);
                        _repo.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.Created, message);
                    } else {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            } else {
                try {

                    //Message Text
                    msg.Text = message.Text ?? msg.Text;
                    _repo.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, msg);

                } catch {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
        }

        public HttpResponseMessage Delete(string key) {
            _repo.Delete<Message>(key);
            _repo.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
