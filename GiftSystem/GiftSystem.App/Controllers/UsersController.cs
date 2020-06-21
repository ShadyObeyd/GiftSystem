using System.Threading.Tasks;
using GiftSystem.App.Models;
using GiftSystem.Models.InputModels.Users;
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
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel inputModel)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var result = await this.usersService.RegisterUser(inputModel.Email, inputModel.Password, inputModel.ConfirmPassword, inputModel.PhoneNumber);

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var result = await this.usersService.LoginUser(inputModel.Email, inputModel.Password);

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            return this.RedirectToAction("Index", "Home");
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