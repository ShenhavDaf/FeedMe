using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Models
{
    public class CreditCard
    {
        public int ID { get; set; }

        /*----------------------------------------------------------------------*/
        [Required]
        [RegularExpression("@^[1-9][0-9]{3}-[1-3]{4}-[0-9]{4}-[0-9]{4}$")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        /*----------------------------------------------------------------------*/

        [Required]
        [RegularExpression("@^[0-9][1-9]/[1-9][0-9]$")]
        [Display(Name = "Expiration Date")]
        public DateTime Expiration { get; set; }

        /*----------------------------------------------------------------------*/

        [Required]
        [RegularExpression("@^[0-9]{3}")]
        public int CVV { get; set; }
    
        /*----------------------------------------------------------------------*/
        
        public int UserID { get; set; }
        //one to one
        public User User { get; set; }

        [Required]
        [RegularExpression("@[0-9]{9}")]
        public string IDnumber{ get; set; }
    }
}
