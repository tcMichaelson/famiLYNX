using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models {
    public class InviteOrPlea : IPubDbObject {

        [Key]
        public string Key { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public Family Family { get; set; }
        public Response UserResponse { get; set; }
        public Response Approved { get; set; }
        
        public ApplicationUser Pleader { get; set; }
        public ApplicationUser Inviter { get; set; }
        public ApplicationUser Approver { get; set; }
    }

    public enum Response {
        None,
        Yes,
        No
    }
}