﻿using GiftSystem.App.Areas.Identity.Data;
using GiftSystem.Data.Repositories.Contracts;
using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiftSystem.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly GiftSystemContext db;
        private readonly SignInManager<GiftSystemUser> signInManager;

        public UsersRepository(GiftSystemContext db, SignInManager<GiftSystemUser> signInManager)
        {
            this.db = db;
            this.signInManager = signInManager;
        }

        public async Task<IEnumerable<GiftSystemUser>> GetAllUsers()
        {
            var users = await this.db.Users.ToArrayAsync();

            return users;
        }

        public async Task<IEnumerable<GiftSystemUser>> GetAllUsersWithTransactions()
        {
            var users = await this.db.Users.Include(u => u.SentTransactions)
                                           .Include(u => u.ReceivedTransactions)
                                           .ToArrayAsync();
            return users;
        }

        public async Task<GiftSystemUser> GetUserById(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<GiftSystemUser> GetUserWithTransactionsAndUsersById(string id)
        {
            var user = await this.db.Users.Include(u => u.SentTransactions)
                                          .ThenInclude(st => st.Receiver)
                                          .Include(u => u.ReceivedTransactions)
                                          .ThenInclude(rt => rt.Sender)
                                          .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task UpdateUser(GiftSystemUser user)
        {
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        public async Task<IdentityResult> CreateUser(GiftSystemUser user, string password)
        {
            var result = await this.signInManager.UserManager.CreateAsync(user, password);

            return result;
        }

        public async Task SignInUser(GiftSystemUser user)
        {
            await this.signInManager.SignInAsync(user, isPersistent: true);
        }

        public async Task LoginUser(string email, string passowrd)
        {
            await this.signInManager.PasswordSignInAsync(email, passowrd, isPersistent: true, lockoutOnFailure: false);
        }

        public async Task LogoutUser()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task AsignUserToRole(GiftSystemUser user)
        {
            await this.signInManager.UserManager.AddToRoleAsync(user, "User");
        }
    }
}