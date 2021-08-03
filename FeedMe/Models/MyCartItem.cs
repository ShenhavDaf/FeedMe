using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class MyCartItem
    {
        public int ID { get; set; }
        /*------------------------------------------------------*/

        //one to one
        public int DishID { get; set; }

        /*------------------------------------------------------*/

        public Dish Dish { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert amount or insert 0")]
        [RegularExpression(@"^[1-9]{1}(?:[0-9])?$", ErrorMessage = "Quantity must be 1 to 50")]
        public int Quantity { get; set; }

        /*------------------------------------------------------*/

        public int Price { get; set; }

        /*------------------------------------------------------*/

        //MAMY cartItems TO ONE cart 
        public int MyCartID { get; set; }

        /*------------------------------------------------------*/
        public MyCart MyCart { get; set; }

        /*------------------------------------------------------*/

        public Boolean SaveQ { get; set; } //Check if the quantity of cartItem has been saved.

    }
}
