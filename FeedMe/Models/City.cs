using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ourProject.Models
{
    public class City
    {
        public int ID{ get; set; }


        [Required(ErrorMessage = "Please insert name")]
        [Display(Name = "City Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string Name{ get; set; }


        //MANY cities TO MANY restaurants 
        public List<Restaurant> Restaurants{ get; set; }
    }
}
