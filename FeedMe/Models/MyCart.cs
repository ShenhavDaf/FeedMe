using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class MyCart
    {
        //Serial number (generic)
        public int ID { get; set; }

        //ONE cart TO MAMY cart items
        public List<MyCartItem> MyCartItems { get; set; }

        public int TotalAmount { get; set; }

        // MIGRATION
        public int UserID { get; set; }

        public User User { get; set; }

        public Boolean IsClose { get; set; }
        // MIGRATION
    }
}
