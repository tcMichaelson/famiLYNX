using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class InviteOrPleaDTO {
        public string EmailAddress { get; set; }
        public Response UserResponse { get; set; }
        public Response Approved { get; set; }

        public ApplicationUserDTO Pleader { get; set; }
        public ApplicationUserDTO Approver { get; set; }
        public ApplicationUserDTO Inviter { get; set; }
        public FamilyDTO Family { get; set; }
    }
    
    public enum Response {
        None,
        Yes,
        No
    }
}