﻿using GiftSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftSystem.App.Areas.Identity.Data
{
    public class GiftSystemContext : IdentityDbContext<GiftSystemUser>
    {
        public GiftSystemContext(DbContextOptions<GiftSystemContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}