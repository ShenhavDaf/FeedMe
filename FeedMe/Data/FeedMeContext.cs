using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ourProject.Models;
using FeedMe.Models;

namespace FeedMe.Data
{
    public class FeedMeContext : DbContext
    {
        public FeedMeContext (DbContextOptions<FeedMeContext> options)
            : base(options)
        {
        }

        public DbSet<ourProject.Models.Category> Category { get; set; }

        public DbSet<ourProject.Models.Restaurant> Restaurant { get; set; }

        public DbSet<ourProject.Models.Dish> Dish { get; set; }

        public DbSet<ourProject.Models.City> City { get; set; }

        public DbSet<ourProject.Models.User> User { get; set; }

        public DbSet<ourProject.Models.Cart> Cart { get; set; }

        public DbSet<ourProject.Models.CartItem> CartItem { get; set; }
    }
}
