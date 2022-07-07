using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie247.Areas.Identity.Data;
using Movie247.Models;

namespace Movie247.Data
{
    public class Movie247Context : IdentityDbContext<Movie247User>
    {
        public Movie247Context(DbContextOptions<Movie247Context> options)
            : base(options)
        {
        }
        public DbSet<Favourite> FavouriteMovies { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Favourite>()
            //    .HasOne(f => f.User)
            //    .WithMany(u => u.FavoriteMovies)
            //    .HasForeignKey(f => f.UserId);
            builder.Entity<Favourite>()
                .HasOne(po => po.User)
                .WithMany(a => a.FavoriteMovies)
                .HasForeignKey(po => po.UserId);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }


    }
}
