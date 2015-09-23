using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class FamilyUserDTO {
        [ForeignKey("Family")]
        public int FamilyId { get; set; }
        public virtual FamilyDTO Family { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUserDTO User { get; set; }
    }
}