﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Core.Abstracts;
using WorkerDemoApp.Entity;
using WorkerDemoApp.Mapping;

namespace WorkerDemoApp.DAL
{
    public class BaseContext
      : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IEFContext
    {
        public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
        public BaseContext(DbContextOptions options) : base(options)
        {

        }
        public override DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.ModifiedDate = now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = now;
                    if (entry.Entity.Status == 0) entry.Entity.Status = Core.Enums.Status.Active;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.Status = Core.Enums.Status.Deleted;
                    entry.Entity.ModifiedDate = now;
                }
            }

            try { return await base.SaveChangesAsync(cancellationToken); }
            catch (DbUpdateException ex)
            {
                // Stack trace’i koru
                throw;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseMap<IEntity>).Assembly);

        }
    }
}
