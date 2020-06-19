using GiftSystem.App.Models;
using GiftSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GiftSystem.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsersService usersService;

        public HomeController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var result = await this.usersService.CreateIndexViewModel(userId);

                if (!result.Success)
                {
                    return this.View("Error", new ErrorViewModel(result.Message));
                }

                return this.View(result.Data);
            }
            catch (NullReferenceException)
            {
                return this.View();
            }
        }
    }
}