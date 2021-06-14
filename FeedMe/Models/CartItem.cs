﻿using ourProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ourProject.Models
{
    public class CartItem
    {
        public int ID { get; set; }

        public int DishID { get; set; }

        public Dish Dish { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int CartID { get; set; }//נועד לקשר של יחיד לרבים, שלא יהיו בעיות בדאטהבייס

        //MAMY cartItems TO ONE cart 

        public Cart cart { get; set; }

        [Required(ErrorMessage = "Please tell about the order")]
        public string Description { get; set; }
    }
}
