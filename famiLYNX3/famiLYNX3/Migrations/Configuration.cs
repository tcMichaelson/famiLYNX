namespace famiLYNX3.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<famiLYNX3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(famiLYNX3.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = manager.FindByName("tmichael");
            if (user == null) {
                user = new ApplicationUser {
                    FirstName = "Tom",
                    UserName = "tmichael",
                    Email = "tc.michaelson@gmail.com"
                };
                manager.Create(user, "Secret123!");
                
            }

            var user2 = manager.FindByName("bsperry");
            if (user2 == null) {
                user2 = new ApplicationUser {
                    FirstName = "Ben",
                    UserName = "bsperry",
                    Email = "bsperry@gmail.com"
                };
                manager.Create(user2, "Secret123!");


            }

        }
    }
}
