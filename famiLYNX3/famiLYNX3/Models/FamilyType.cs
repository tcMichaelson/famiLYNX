using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//This is primarily used for family roles, but I'm not sure if
//it's really needed for functionality's sake.
namespace famiLYNX3.Models {
    public class FamilyType : IPubDbObject {
        [Key]
        public string Key { get; set; }
        public string OrgType { get; set; }
    }
}