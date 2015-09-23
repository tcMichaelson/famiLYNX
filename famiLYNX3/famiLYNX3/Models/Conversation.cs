using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models {
    public class Conversation : IPubDbObject {
        [Key]
        public string Key { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsEvent { get; set; }
        public bool Recurs { get; set; }  // Maybe set up an option to have an event recur to remind the organizer.

        public Family WhichFam { get; set; }
        [Required]
        public ApplicationUser CreatedBy { get; set; }

        public IList<ConversationsVisibleToMembers> VisibleTo { get; set; }
        public IList<ConversationsAttendedByMembers> Attenders { get; set; }

        [InverseProperty("Conversation")]
        public IList<Message> MessageList { get; set; }
    }
}