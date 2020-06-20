using System.Threading.Tasks;
using GiftSystem.App.Models;
using GiftSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftSystem.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UsersService usersService;

        public UsersController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var result = await this.usersService.CreateAllUsersViewModel();

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            return this.View(result.Data);
        }
    }
}