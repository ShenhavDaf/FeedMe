using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ourProject.Models
{
    public enum UserType
    {
        Guest,
        Client,
        rManager,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please insert Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please insert password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Please insert name")]
        public String Name { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Please insert phone")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }

        [Display(Name = "Birthday Date")]
        public DateTime BirthdayDate { get; set; }

        //one to one
        public CreditCard CreditCard { get; set; }

        public UserType Type { get; set; } = UserType.Guest;
    }
}
