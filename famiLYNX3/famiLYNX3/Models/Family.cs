using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models {
    public class Family : IPubDbObject {
        [Key]
        public string Key { get; set; }

        [Required]
        public string FamilyUserName { get; set; }

        [Required]
        public string OrgName { get; set; }

        public ApplicationUser CreatedBy { get; set; }
        public FamilyType Type { get; set; }

        //Navigation Properties
        public IList<InviteOrPlea> InviteOrPleas { get; set; }
        public IList<FamilyUser> MemberList { get; set; }
        public IList<Conversation> ConversationList { get; set; }
    }
}