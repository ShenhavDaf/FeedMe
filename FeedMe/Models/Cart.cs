using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ourProject.Models
{
    public class Cart
    {
        //אמור להיות לתצוגה בלבד, יכול להיות שבכלל לא צריך מודל

        //Serial number (generic)
        public int ID{ get; set; }

        //ONE cart TO MAMY dishes
        public List<Dish> Dishes{ get; set; }

        public int TotalAmount{ get; set; }

        [DataType(DataType.Currency)]
        public int Price{ get; set; }

        public TimeSpan ArrivalTime { get; set; }

    }
}
