using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class MessageDTO {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeSubmitted { get; set; }
        public ApplicationUserDTO Contributor { get; set; }
        public ConversationDTO Conversation { get; set; }
    }
}