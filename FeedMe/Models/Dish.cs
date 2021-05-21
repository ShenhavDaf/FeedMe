using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ourProject.Models
{
    public class Dish
    {
        public int ID{ get; set; }
        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert name")]
        [Display(Name = "Dose Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string Name { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert image")]
        [Display(Name = "Dish Image")]
        public string DishImage { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please tell about the dish")]
        public string Description { get; set; }

        /*------------------------------------------------------*/


        //טבעוני, חלבי, בשרי
        [Required(ErrorMessage = "Please insert food type")]
        public int FoodType{ get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert price")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        /*------------------------------------------------------*/

        public int RestaurantID{ get; set; }//נועד לקשר של יחיד לרבים, שלא יהיו בעיות בדאטהבייס

        //MAMY dishes TO ONE restaurant 

        public Restaurant Restaurant{ get; set; }
    }
}
