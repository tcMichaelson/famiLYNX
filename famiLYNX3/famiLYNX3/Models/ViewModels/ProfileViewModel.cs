using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models.ViewModels {
    public class ProfileViewModel {
        public ApplicationUserDTO User { get; set; }
        public List<FamilyDTO> Familys { get; set; }        
    }
}