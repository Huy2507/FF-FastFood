using FF_Fastfood.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;

public class RoleConfig
{
    public static void RegisterRoles()
    {
        using (var context = new ApplicationDbContext())
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Tạo vai trò Admin nếu chưa tồn tại
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);
            }

            // Tạo vai trò SuperAdmin nếu chưa tồn tại
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                var role = new IdentityRole("SuperAdmin");
                roleManager.Create(role);
            }
        }
    }
}
