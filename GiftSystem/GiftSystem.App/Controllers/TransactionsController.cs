using GiftSystem.App.Models;
using GiftSystem.Models.InputModels.Transactions;
using GiftSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GiftSystem.App.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly TransactionsService transactionsService;
        private readonly UsersService usersService;

        public TransactionsController(TransactionsService transactionsService, UsersService usersService)
        {
            this.transactionsService = transactionsService;
            this.usersService = usersService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = this.transactionsService.CreateTransactionInputModel(userId);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionInputModel inputModel)
        {
            var result = await this.usersService.GetUserById(inputModel.ReceiverId);

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            if (result.Data.PhoneNumber != inputModel.RecieverPhoneNumber)
            {
                string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var model = this.transactionsService.CreateTransactionInputModel(userId);

                ModelState.AddModelError("RecieverPhoneNumber", "Incorrect receiver phone number!");
                
                return this.View(model);
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.transactionsService.CreateTransaction(inputModel.SenderId, inputModel.ReceiverId, inputModel.Wish, inputModel.Credits);
            return this.RedirectToAction("Index", "Home");
        }
    }
}