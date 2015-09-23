using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace famiLYNX3.Models {
    public class Message : IPubDbObject{
        [Key]
        public string Key { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime TimeSubmitted { get; set; }

        public ApplicationUser Contributor { get; set; }
        public Conversation Conversation { get; set; }
    }
}