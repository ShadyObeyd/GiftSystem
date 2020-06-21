using System.Threading.Tasks;
using GiftSystem.App.Models;
using GiftSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftSystem.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService usersService;

        public UsersController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> All()
        {
            var result = await this.usersService.CreateAllUsersViewModel();

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            return this.View(result.Data);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await this.usersService.LogoutUser();

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}