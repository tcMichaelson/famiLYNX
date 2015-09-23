using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace famiLYNX3.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address UserAddress { get; set; }

        public IList<ConversationsVisibleToMembers> VisibleConversations { get; set; }
        public IList<ConversationsAttendedByMembers> Attending { get; set; }

        [InverseProperty("Pleader")]
        public IList<InviteOrPlea> Pleas { get; set; }
        [InverseProperty("Inviter")]
        public IList<InviteOrPlea> Invites { get; set; }
        [InverseProperty("Approver")]
        public IList<InviteOrPlea> ToApprove { get; set; }

        [InverseProperty("Contributor")]
        public IList<Message> Contributions { get; set; }

        public IList<OrgRole> OrgRoles { get; set; }
        public IList<FamilyUser> Familys { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public IDbSet<Address> Addresses { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}