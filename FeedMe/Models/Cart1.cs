using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class Cart1
    {
        //אמור להיות לתצוגה בלבד, יכול להיות שבכלל לא צריך מודל

        //Serial number (generic)
        public int ID { get; set; }

        //ONE cart TO MAMY cart items
        public List<CartItem1> CartItems { get; set; }

        public int TotalAmount { get; set; }

    }
}
