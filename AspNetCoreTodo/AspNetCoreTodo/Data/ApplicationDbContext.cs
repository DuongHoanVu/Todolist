using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Data
{
    public class ApplicationUser : IdentityUser
    {
        
    }

    public class ApplicationDbContext : IdentityDbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /* 
            Interacting with database to read & save items
            needs 2 things: context update & database migration
        */
        
    // Context Update
        // DbSet represent a table in database.
        // Tell Entity Framework Core to store TodoItem entity in Item table.
        // Context & Database are out of sync, thus creating Migration
        public DbSet<TodoItem> Items { get; set; }

    // Database Migration
        // Create Migration
            // Run: 'dotnet ef migrations add AddItems'                 PM> add-migration MyFirstMigration
            // To undo this action, use 'ef migrations remove'

            // A list of Migration
            // Run: 'dotnet ef migrations list'
        // Apply Migration to Dabase
            // Create Items table in the database
            // Run: 'dotnet ef database update'                         PM> Update-Database
            // To role back the database, run 'dotnet ef database update CreateIdentitySchema' (name of previous of migration)
            // To erase the database, run 'dotnet ef database drop', and 'dotnet ef database update'

    }
}
