﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class MyCartItem
    {
        public int ID { get; set; }

        //one to one
        public int DishID { get; set; }

        public Dish Dish { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        //MAMY cartItems TO ONE cart 
        public int MyCartID { get; set; }//נועד לקשר של יחיד לרבים, שלא יהיו בעיות בדאטהבייס
        public MyCart MyCart { get; set; }

        public Boolean SaveQ { get; set; } //Check if the quantity of cartItem has been saved.

    }
}
