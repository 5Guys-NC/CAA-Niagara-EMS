using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
/*********************************
 * Created By: Brian Culp
 * Edited By: Jon Yade
 * ******************************/

namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for UserAccount Table
    /// </summary>
    public class UserAccount : Auditable
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string ID { get; set; }

        [StringLength(25, ErrorMessage = "First Name cannot be more than 25 characters long.")]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "Last Name cannot be more than 25 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName Required")]
        [StringLength(50, ErrorMessage = "User Name cannot be more than 50 characters")]
        public string UserName { get; set; }

        private string password;
        [Required(ErrorMessage = "Password is Required")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password 
        {
            get
            {
                return password;
            }
            set
            {
                //encrypt password on setting
                password = EncryptPassword(value.ToString());
            }
        }

        [Required(ErrorMessage = "Is this person an Admin?")]
        public bool isAdmin { get; set; } = false;

        /// <summary>
        /// Summary property to display full name
        /// </summary>
        public string FullName
        {
            get 
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        /// <summary>
        /// Method to Encrypt Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>string</returns>
        internal string EncryptPassword(string password)
        {
            //for information on this method go to:
            //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/

            //using the SHA256 method of encryption
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //encrypt password (returns byte array)
                byte[] bytesHashed = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                //loop through byte  array and build string with it
                for (int i = 0; i < bytesHashed.Length; i++)
                {
                    builder.Append(bytesHashed[i].ToString("x2"));
                }

                //return final StringBuilder object as string
                return builder.ToString();
            }
        }
    }
}
