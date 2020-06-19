using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace GiftSystem.App.Middlewares
{
    public class SeedAdmin
    {
        private readonly RequestDelegate next;

        public SeedAdmin(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider, SignInManager<GiftSystemUser> signInManager)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            bool adminRoleExists = await roleManager.RoleExistsAsync("Admin");

            if (!adminRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            bool userRoleExists = await roleManager.RoleExistsAsync("User");

            if (!userRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!signInManager.UserManager.Users.Any())
            {
                GiftSystemUser user = new GiftSystemUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    PhoneNumber = "0898412588"
                };

                await signInManager.UserManager.CreateAsync(user, "123");
                await signInManager.UserManager.AddToRoleAsync(user, "Admin");
            }

            await this.next(httpContext);
        }
    }
}