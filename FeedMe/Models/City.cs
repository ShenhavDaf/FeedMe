using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class City
    {
        public int ID{ get; set; }

        /*------------------------------------------------------*/

        [Required(ErrorMessage = "Please insert name")]
        [Display(Name = "City Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z '-]*$", ErrorMessage = "City name must begin with a capital letter")]
        public string Name{ get; set; }

        /*------------------------------------------------------*/

        //MANY cities TO MANY restaurants 
        public List<Restaurant> Restaurants{ get; set; }
    }
}
