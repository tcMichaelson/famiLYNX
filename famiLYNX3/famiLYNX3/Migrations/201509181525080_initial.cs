namespace famiLYNX3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Street = c.String(),
                        City = c.String(),
                        State = c.String(maxLength: 2),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserAddress_Key = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.UserAddress_Key)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserAddress_Key);
            
            CreateTable(
                "dbo.ConversationsAttendedByMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.String(maxLength: 128),
                        ConversationKey = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversations", t => t.ConversationKey)
                .ForeignKey("dbo.AspNetUsers", t => t.MemberId)
                .Index(t => t.MemberId)
                .Index(t => t.ConversationKey);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Topic = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(),
                        IsEvent = c.Boolean(nullable: false),
                        Recurs = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        WhichFam_Key = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.Families", t => t.WhichFam_Key)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.WhichFam_Key);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Text = c.String(nullable: false),
                        TimeSubmitted = c.DateTime(nullable: false),
                        Conversation_Key = c.String(maxLength: 128),
                        Contributor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Conversations", t => t.Conversation_Key)
                .ForeignKey("dbo.AspNetUsers", t => t.Contributor_Id)
                .Index(t => t.Conversation_Key)
                .Index(t => t.Contributor_Id);
            
            CreateTable(
                "dbo.ConversationsVisibleToMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.String(maxLength: 128),
                        ConversationKey = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversations", t => t.ConversationKey)
                .ForeignKey("dbo.AspNetUsers", t => t.MemberId)
                .Index(t => t.MemberId)
                .Index(t => t.ConversationKey);
            
            CreateTable(
                "dbo.Families",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        FamilyUserName = c.String(nullable: false),
                        OrgName = c.String(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Type_Key = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.FamilyTypes", t => t.Type_Key)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Type_Key);
            
            CreateTable(
                "dbo.InviteOrPleas",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        EmailAddress = c.String(nullable: false),
                        UserResponse = c.Int(nullable: false),
                        Approved = c.Int(nullable: false),
                        Family_Key = c.String(nullable: false, maxLength: 128),
                        Inviter_Id = c.String(maxLength: 128),
                        Pleader_Id = c.String(maxLength: 128),
                        Approver_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Families", t => t.Family_Key, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Inviter_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Pleader_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Approver_Id)
                .Index(t => t.Family_Key)
                .Index(t => t.Inviter_Id)
                .Index(t => t.Pleader_Id)
                .Index(t => t.Approver_Id);
            
            CreateTable(
                "dbo.FamilyUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FamilyKey = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Families", t => t.FamilyKey)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.FamilyKey)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FamilyTypes",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        OrgType = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrgRoles",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(nullable: false),
                        OrgType_Key = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.FamilyTypes", t => t.OrgType_Key, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.OrgType_Key)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserAddress_Key", "dbo.Addresses");
            DropForeignKey("dbo.InviteOrPleas", "Approver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InviteOrPleas", "Pleader_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrgRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrgRoles", "OrgType_Key", "dbo.FamilyTypes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InviteOrPleas", "Inviter_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Contributor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConversationsAttendedByMembers", "MemberId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConversationsAttendedByMembers", "ConversationKey", "dbo.Conversations");
            DropForeignKey("dbo.Families", "Type_Key", "dbo.FamilyTypes");
            DropForeignKey("dbo.FamilyUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FamilyUsers", "FamilyKey", "dbo.Families");
            DropForeignKey("dbo.InviteOrPleas", "Family_Key", "dbo.Families");
            DropForeignKey("dbo.Families", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "WhichFam_Key", "dbo.Families");
            DropForeignKey("dbo.ConversationsVisibleToMembers", "MemberId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConversationsVisibleToMembers", "ConversationKey", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "Conversation_Key", "dbo.Conversations");
            DropForeignKey("dbo.Conversations", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.OrgRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OrgRoles", new[] { "OrgType_Key" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.FamilyUsers", new[] { "UserId" });
            DropIndex("dbo.FamilyUsers", new[] { "FamilyKey" });
            DropIndex("dbo.InviteOrPleas", new[] { "Approver_Id" });
            DropIndex("dbo.InviteOrPleas", new[] { "Pleader_Id" });
            DropIndex("dbo.InviteOrPleas", new[] { "Inviter_Id" });
            DropIndex("dbo.InviteOrPleas", new[] { "Family_Key" });
            DropIndex("dbo.Families", new[] { "Type_Key" });
            DropIndex("dbo.Families", new[] { "CreatedBy_Id" });
            DropIndex("dbo.ConversationsVisibleToMembers", new[] { "ConversationKey" });
            DropIndex("dbo.ConversationsVisibleToMembers", new[] { "MemberId" });
            DropIndex("dbo.Messages", new[] { "Contributor_Id" });
            DropIndex("dbo.Messages", new[] { "Conversation_Key" });
            DropIndex("dbo.Conversations", new[] { "WhichFam_Key" });
            DropIndex("dbo.Conversations", new[] { "CreatedBy_Id" });
            DropIndex("dbo.ConversationsAttendedByMembers", new[] { "ConversationKey" });
            DropIndex("dbo.ConversationsAttendedByMembers", new[] { "MemberId" });
            DropIndex("dbo.AspNetUsers", new[] { "UserAddress_Key" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.OrgRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.FamilyTypes");
            DropTable("dbo.FamilyUsers");
            DropTable("dbo.InviteOrPleas");
            DropTable("dbo.Families");
            DropTable("dbo.ConversationsVisibleToMembers");
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
            DropTable("dbo.ConversationsAttendedByMembers");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Addresses");
        }
    }
}
