using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class Category
    {
        public int ID { get; set; }

        /*---------------------------------------------------------------------*/
        
        [Required(ErrorMessage = "Please input category name")]
        [Display(Name ="Category Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z -']*$", ErrorMessage = "Category name must begin with a capital letter")] 
        public string Name{ get; set; }
        
        /*---------------------------------------------------------------------*/
        
        [Display(Name = "Category Image")]
        //[DataType(DataType.ImageUrl)]
        public string CategoryImage{ get; set; }
        
        /*---------------------------------------------------------------------*/

        //  MANY categories TO MAMY restaurants

        public List<Restaurant> Restaurants{ get; set; }
    }
}
