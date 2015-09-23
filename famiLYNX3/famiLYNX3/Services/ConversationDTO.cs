using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class ConversationDTO {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsEvent { get; set; }
        public bool Recurs { get; set; }  // Maybe set up an option to have an event recur to remind the organizer.

        public FamilyDTO WhichFam { get; set; }
        public ApplicationUserDTO CreatedBy { get; set; }

        public IList<ConversationsVisibleToMembersDTO> VisibleTo { get; set; }
        public IList<ConversationsAttendedByMembersDTO> Attenders { get; set; }

        [InverseProperty("Conversation")]
        public IList<MessageDTO> MessageList { get; set; }
    }
}