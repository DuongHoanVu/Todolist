using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using System.Data.Entity;
//using Microsoft.Data.Entity;
//using System.Data.Entity.QueryableExtensions;
using System.Security.Claims;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {      
            Console.WriteLine("1111111111");
            //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            Console.WriteLine("2222222222");
            await EnsureRolesAsync(roleManager);
            
            Console.WriteLine("33333333333");
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            Console.WriteLine("44444444444");
            await EnsureTestAdminAsync(userManager);
            Console.WriteLine("55555555555");
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);
            Console.WriteLine("Check role Exist");
            if (alreadyExists) return;
            Console.WriteLine("Role doesn't exit, create a new one");
            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            Console.WriteLine("Check userManager");
            var testAdmin = await userManager.Users
            .Where(x => x.UserName == "admin_3@todo.local")
            .SingleOrDefaultAsync();
            
            if (testAdmin != null) return;
            
            Console.WriteLine("Create testAdmin");
            testAdmin = new IdentityUser
            {
                UserName = "admin_3@todo.local",
                Email = "admin_3@todo.local",
                // Adding
                EmailConfirmed = true,
            };
            // Source: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-5.0
            Console.WriteLine("Store the value in userManager class");
            await userManager.CreateAsync(testAdmin, "NotSecure123!!");
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
            var managerClaim = new Claim("Role", "Manager");
            await userManager.AddClaimAsync(testAdmin, managerClaim);
        }

    }
}