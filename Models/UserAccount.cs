using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class UserAccount
    {
        public int ID { get; set; }

        [StringLength(25, ErrorMessage = "First Name cannot be more than 25 characters long.")]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "Last Name cannot be more than 25 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be 8-50 characters")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Is this person an Admin?")]
        public bool isAdmin { get; set; } = false;
    }
}
