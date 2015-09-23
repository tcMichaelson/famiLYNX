using famiLYNX3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models.ViewModels {
    public class ConversationViewModel {
        public ConversationDTO Conversation { get; set; }
        public string FamilyKey { get; set; }
        public string FamilyName { get; set; }
        public string UserName { get; set; }
    }
}