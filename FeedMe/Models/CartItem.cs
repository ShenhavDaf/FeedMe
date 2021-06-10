using ourProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class CartItem
    {
        public int ID { get; set; }

        public int DishID { get; set; }

        public Dish Dish { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
