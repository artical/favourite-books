using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is missing")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is missing")]
        [DataType(DataType.Password)]
        [UIHint("Password")]
        public string Password { get; set; }
    }
}
