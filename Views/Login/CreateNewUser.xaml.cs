using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
/***************************************
 * Created By: Brian Culp
 * Edited By:
 * *************************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{
    /// <summary>
    /// Frame for Create New User
    /// </summary>
    public sealed partial class CreateNewUser : Page
    {
        Users newUser;

        IUsersRepository userRepository;

        public CreateNewUser()
        {
            this.InitializeComponent();
            userRepository = new UsersRepository();
        }

        /// <summary>
        /// Create Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            newUser = new Users();
            this.DataContext = newUser;

            try
            {
                //if first name exists
                if (txtFName.Text != "")
                {
                    //set first name
                    newUser.FirstName = txtFName.Text;
                }

                //if last name exists
                if (txtLName.Text != "")
                {
                    //set last name
                    newUser.LastName = txtLName.Text;
                }

                //if admin
                if (chkAdmin.IsChecked == true)
                {
                    //set admin to true
                    newUser.isAdmin = true;
                }
                else
                {
                    //set admin to false
                    newUser.isAdmin = false;
                }

                //if password matches confirm password
                if (txtPassword.Password == txtConfirmPassword.Password)
                {
                    //set password
                    newUser.Password = txtPassword.Password;
                }
                else
                {
                    //return error message
                    Jeeves.ShowMessage("Error", "Passwords do not match");
                    return;
                }

                //set username
                newUser.UserName = txtUserName.Text;

                //add user
                userRepository.AddUser(newUser);
                
                //clear fields to allow new creation
                ClearFields();

            }
            catch (Exception ex)
            {
                //error if adding user was unsuccessful
                Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
            }
        }

        /// <summary>
        /// Method to clear the text boxes for new creation
        /// </summary>
        private void ClearFields()
        {
            txtUserName.Text = "";
            txtFName.Text = "";
            txtLName.Text = "";
            txtPassword.Password = "";
            txtConfirmPassword.Password = "";
            chkAdmin.IsChecked = false;

        }
    }
}
     
