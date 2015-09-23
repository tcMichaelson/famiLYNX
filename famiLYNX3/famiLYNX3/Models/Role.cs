using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models {

    public class OrgRole : IPubDbObject {
        [Key]
        public string Key { get; set; }
        [Required]
        public FamilyType OrgType { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}