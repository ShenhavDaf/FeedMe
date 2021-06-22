using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class RestaurantHours
    {
        public int Start{ get; set; }
        public int End { get; set; }
    }
}
