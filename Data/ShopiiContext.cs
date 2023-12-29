using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ShopiiContext : IdentityDbContext<
        User, 
        IdentityRole,
        string, 
        IdentityUserClaim<string>, 
        IdentityUserRole<string>, 
        IdentityUserLogin<string>, 
        IdentityRoleClaim<string>, 
        UserToken>
    {
        public ShopiiContext(DbContextOptions<ShopiiContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
