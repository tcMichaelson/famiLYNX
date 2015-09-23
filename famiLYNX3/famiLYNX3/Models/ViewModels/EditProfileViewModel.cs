using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models.ViewModels {
    public class EditProfileViewModel {
        public ApplicationUserDTO User { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public StName State { get; set; }
        public string Zip { get; set; }
    }


}