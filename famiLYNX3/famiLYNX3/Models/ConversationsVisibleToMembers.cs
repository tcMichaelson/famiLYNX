using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models {
    public class ConversationsVisibleToMembers {
        public int Id { get; set; }

        [ForeignKey("Member")]
        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        [ForeignKey("Conversation")]
        public string ConversationKey { get; set; }
        public Conversation Conversation { get; set; }
    }
}