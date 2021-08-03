using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public enum FoodType
    {
        Meat,
        Diary,
        Vegetarian,
        Vegan
    }
    public class Dish
    {
        public int ID { get; set; }
        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert name")]
        [Display(Name = "Dish Name")]
        a-zA-Z0-9 -'|.!&
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9 -'+.!&]*$", ErrorMessage = "Dish name must begin with a capital letter")]
        public string Name { get; set; }

        /*------------------------------------------------------*/

        [Display(Name = "Dish Image")]
        public string DishImage { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please tell about the dish")]
        public string Description { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert food type")]
        public FoodType FoodType { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert price")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[1-9]{1}(?:[0-9])?$", ErrorMessage = "Price must be greater than 0")]
        public int Price { get; set; }


        /*------------------------------------------------------*/
        //MAMY dishes TO ONE restaurant 

        [Display(Name = "In the menu of the restaurant:")]
        public int RestaurantID { get; set; }

        public Restaurant Restaurant { get; set; }

    }
}
