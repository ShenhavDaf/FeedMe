using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class ViewJoin
    {
        [ForeignKey("RestaurantID")]
        public int Id { get; set; }

        [NotMapped]
        public List<Dish> Dishes { get; set; }

        [NotMapped]
        public List<Restaurant> Restaurants { get; set; }
    }
}
