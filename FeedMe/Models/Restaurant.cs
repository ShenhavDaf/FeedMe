using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class Restaurant
    {
        public int ID { get; set; }
        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert name")]
        [Display(Name = "Restaurant Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z -'|]*$", ErrorMessage = "Restaurant name must begin with a capital letter")]
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
        [RegularExpression(@"^\+?(972\-?)?0?(([23489]{1}\-?\d{7})|[5]{1}\d{1}\-?\d{7})$", ErrorMessage = "Phone number is from the template: 03-1234567 / 052-1234567 / + 97252-1234567")]
        public string PhoneNumber{ get; set; }

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

        /*------------------------------------------------------*/
        public User User { get; set; }
    }
}
