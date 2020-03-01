using System.ComponentModel.DataAnnotations;
/***************************
 * Created By: Brian Culp
 * Edited By:
 * *************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the USERS table in the database
    /// </summary>
    public class Users
    {
        public int ID { get; set; }

        [StringLength(25,ErrorMessage = "First Name cannot be more than 25 characters long.")]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "Last Name cannot be more than 25 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        [RegularExpression(@"/^(?=.*[A-Z])(?=.*\d)[A-Za-z\d][A-Za-z\d]{8,20}$/", ErrorMessage = "Username must contain a Capital and a Number")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(50,MinimumLength = 8,ErrorMessage = "Password must be 8-50 characters")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Is this person an Admin?")]
        public bool isAdmin { get; set; } = false;

    }
}
