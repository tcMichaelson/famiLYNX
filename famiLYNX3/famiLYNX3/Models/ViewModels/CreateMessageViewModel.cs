using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models.ViewModels {
    public class CreateMessageViewModel {
        public string MessageText { get; set; }
        public string ConversationKey { get; set; }
        public string FamilyKey { get; set; }
        public string MemberUserName { get; set; }
    }
}