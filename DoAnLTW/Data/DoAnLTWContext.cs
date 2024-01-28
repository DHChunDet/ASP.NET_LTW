using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoAnLTW.Models;

namespace DoAnLTW.Data
{
    public class DoAnLTWContext : DbContext
    {
        public DoAnLTWContext (DbContextOptions<DoAnLTWContext> options)
            : base(options)
        {
        }

        public DbSet<DoAnLTW.Models.Category> Category { get; set; } = default!;
        public DbSet<DoAnLTW.Models.Product> Product { get; set; } = default!;
        public DbSet<DoAnLTW.Models.User> User { get; set; } = default!;
        public DbSet<DoAnLTW.Models.ShopingCart> ShopingCart { get; set; } = default!;
    }
}
