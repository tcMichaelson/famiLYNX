using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class FamilyDTO {
        public int Id { get; set; }
        public string FamilyUserName { get; set; }
        public string OrgName { get; set; }

        public ApplicationUserDTO CreatedBy { get; set; }
        public FamilyTypeDTO Type { get; set; }

        //Navigation Properties
        public IList<InviteOrPleaDTO> InviteOrPleas { get; set; }
        public IList<FamilyUserDTO> MemberList { get; set; }
        public IList<ConversationDTO> ConversationList { get; set; }
    }
}