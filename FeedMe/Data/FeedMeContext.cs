using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FeedMe.Models;

namespace FeedMe.Data
{
    public class FeedMeContext : DbContext
    {
        public FeedMeContext (DbContextOptions<FeedMeContext> options)
            : base(options)
        {
        }

        public DbSet<FeedMe.Models.Category> Category { get; set; }

        public DbSet<FeedMe.Models.Restaurant> Restaurant { get; set; }

        public DbSet<FeedMe.Models.Dish> Dish { get; set; }

        public DbSet<FeedMe.Models.City> City { get; set; }

        public DbSet<FeedMe.Models.User> User { get; set; }

        public DbSet<FeedMe.Models.MyCartItem> MyCartItem { get; set; }

        public DbSet<FeedMe.Models.MyCart> MyCart { get; set; }
    }
}
