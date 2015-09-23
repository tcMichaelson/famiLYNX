using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using famiLYNX3.Infrastructure;
using famiLYNX3.Models;
using System.Collections.Generic;
using famiLYNX3.API;

namespace FamiLYNX3.tests {
    [TestClass]
    public class APITests {
        [TestMethod]
        public void FamnilyControllerTests() {
            //Arrange

            var families = new List<Family> {
                new Family {
                     ConversationList = new List<Conversation> {
                         new Conversation(),
                         new Conversation()
                     },
                      CreatedBy = new ApplicationUser(),
                       FamilyUserName = "Michaelson",
                        InviteOrPleas = new List<InviteOrPlea>(),
                         Key = "F01234567890qwe",
                          MemberList = new List<FamilyUser>(),
                           OrgName = "Michaelson",
                            Type = new FamilyType()
                },
                new Family {
                     ConversationList = new List<Conversation>(),
                      CreatedBy = new ApplicationUser(),
                       FamilyUserName = "Stevenson",
                        InviteOrPleas = new List<InviteOrPlea>(),
                         Key = "F01234567890qwd",
                          MemberList = new List<FamilyUser>(),
                           OrgName = "Stevenson",
                            Type = new FamilyType()
                }
            };

            var mockRepo = new Mock<FamiliesController>();


            //Act


            //Assert
        }
    }
}
