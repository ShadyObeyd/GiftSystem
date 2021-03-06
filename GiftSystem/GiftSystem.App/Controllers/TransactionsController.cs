﻿using GiftSystem.App.Models;
using GiftSystem.Models.InputModels.Transactions;
using GiftSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GiftSystem.App.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly TransactionsService transactionsService;
        private readonly UsersService usersService;

        public TransactionsController(TransactionsService transactionsService, UsersService usersService)
        {
            this.transactionsService = transactionsService;
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = await this.transactionsService.CreateTransactionInputModel(userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionInputModel inputModel)
        {
            var receiverResult = await this.usersService.GetUserById(inputModel.ReceiverId);

            if (receiverResult.Data.PhoneNumber != inputModel.RecieverPhoneNumber)
            {
                string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var model = await this.transactionsService.CreateTransactionInputModel(userId);

                ModelState.AddModelError("RecieverPhoneNumber", "Incorrect receiver phone number!");
                
                return this.View(model);
            }

            var senderResult = await this.usersService.GetUserById(inputModel.SenderId);

            if (senderResult.Data.Credits < inputModel.Credits)
            {
                string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var model = await this.transactionsService.CreateTransactionInputModel(userId);

                ModelState.AddModelError("Credits", "You don't have enough credits for this transaction!");

                return this.View(model);
            }

            if (!ModelState.IsValid)
            {
                string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var model = await this.transactionsService.CreateTransactionInputModel(userId);

                return this.View(model);
            }

            await this.transactionsService.CreateTransaction(inputModel.SenderId, inputModel.ReceiverId, inputModel.Wish, inputModel.Credits);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await this.transactionsService.CreateTransactionDetailsViewModel(id, userId);

            if (!result.Success)
            {
                return this.View("Error", new ErrorViewModel(result.Message));
            }

            return this.View(result.Data);
        }
    }
}