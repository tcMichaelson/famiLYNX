using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class ApplicationUserDTO {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressDTO UserAddress { get; set; }

        public virtual IList<ConversationDTO> VisibleConversations { get; set; }
        public virtual IList<ConversationDTO> Attending { get; set; }
        public virtual IList<InviteOrPleaDTO> Pleas { get; set; }
        public virtual IList<InviteOrPleaDTO> Invites { get; set; }
        public virtual IList<InviteOrPleaDTO> ToApprove { get; set; }
        public virtual IList<FamilyUserDTO> Familys { get; set; }
        public virtual IList<OrgRoleDTO> OrgRoles { get; set; }
    }
}