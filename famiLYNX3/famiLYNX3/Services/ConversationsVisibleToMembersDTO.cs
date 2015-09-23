using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class ConversationsVisibleToMembersDTO {
        public int Id { get; set; }

        [ForeignKey("Member")]
        public string MemberId { get; set; }
        public ApplicationUserDTO Member { get; set; }

        [ForeignKey("Conversation")]
        public int ConversationId { get; set; }
        public ConversationDTO Conversation { get; set; }
    }
}