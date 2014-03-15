using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace $safeprojectname$.Models
{
    public class IdentityDbInitializer : CreateDatabaseIfNotExists<IdentityDbContext>
    {
        protected override void Seed(IdentityDbContext context)
        {       
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var adminRole = new IdentityRole { Name = "Administrators" };
            var userRole = new IdentityRole { Name = "RegisteredUsers" };

            roleManager.Create(adminRole);
            roleManager.Create(userRole);
            
            var userStore = new UserStore<IdentityUser>(context);
            var userManager = new UserManager<IdentityUser>(userStore);
            
            var administrator = new IdentityUser { UserName = "Administrator" };

            userManager.Create(administrator, "Administrator451");

            userManager.AddToRole(administrator.Id, "RegisteredUsers");
            userManager.AddToRole(administrator.Id, "Administrators");

        }
    }
}