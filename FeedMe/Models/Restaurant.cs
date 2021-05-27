using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ourProject.Models
{
    public class Restaurant
    {
        public int ID { get; set; }
        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert name")]
        [Display(Name = "Restaurant Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z -'|]*$")]
        public string Name { get; set; }

        /*------------------------------------------------------*/

        [Display(Name = "Restaurant Logo")]
        public string RestaurantImage { get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please say a few words about the restaurant")]
        [Display(Name = "Tell us about your restaurant")]
        public string Description{ get; set; }
        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert restaurant address")]
        public string Address{ get; set; }

        [Required(ErrorMessage = "Please insert restaurant phone")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber{ get; set; }
        /*------------------------------------------------------*/

        //[Required(ErrorMessage = "Please insert restaurant hours")]
        //[Display(Name = "Opening Hours")]

        //public RestaurantHours[] OpeningHours = new RestaurantHours[6];


        /*------------------------------------------------------*/

        //MANY restaurants TO MANY cities
        [Display(Name = "Where do you make deliveries?")]
        public List<City> DeliveryCities { get; set; }


        /*------------------------------------------------------*/

        public double Rate{ get; set; }

        /*------------------------------------------------------*/

        //ONE restaurant TO MAMY dishes
        public List<Dish> Dishes{ get; set; }

        /*------------------------------------------------------*/

        //  MANY restaurants TO MAMY Categories

        public List<Category> Categories{ get; set; }
    }
}
