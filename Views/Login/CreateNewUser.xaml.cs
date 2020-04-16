using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using System.Security.Cryptography;
using System.Text;
using Windows.UI.Xaml.Media.Animation;
/***************************************
* Created By: Brian Culp
* Edited By:
* *************************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management.Views.Login
{
    /// <summary>
    /// Frame for Create New User
    /// </summary>
    public sealed partial class CreateNewUser : Page
    {
        UserAccount newUser;
        IUserAccountRepository userRepository;
        //get user logged in currently
        App userLoggedIn = (App)Application.Current;
        UserAccount creator = new UserAccount();

        public CreateNewUser()
        {
            this.InitializeComponent();
            userRepository = new UserAccountRepository();

            creator = userRepository.GetUser(userLoggedIn.userAccountName);

            ((Window.Current.Content as Frame).Content as MainPage).ChangeMainPageTitleName("CREATE NEW USER");
        }

        /// <summary>
        /// Create Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            newUser = new UserAccount();
            this.DataContext = newUser;

            string popupMsg = "You are about to create a user. Once created, your password cannot be changed. " +
                                "Admin's can make other edits. Would you like to continue?";

            var result = await Jeeves.ConfirmDialog("Creating a New User", popupMsg);

            if (result == ContentDialogResult.Secondary)
            {
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


                    string userInput = txtPassword.Password;
                    //if password matches confirm password
                    if (userInput == txtConfirmPassword.Password)
                    {
                        //check to make sure password contains a capital, contains a digit, and contains a special character
                        if (Regex.IsMatch(userInput, "[A-Z]") && Regex.IsMatch(userInput, "\\d") && Regex.IsMatch(userInput, "[ _!@#$%^&*()\\./,';:]"))
                        {
                            //check to see if password provided is between 6 and 30 characters
                            if (userInput.Length < 30 && userInput.Length > 6)
                            {
                                //set password (encrypted in model)
                                newUser.Password = txtPassword.Password;
                            }
                            else
                            {
                                //return error message
                                Jeeves.ShowMessage("Error", "Password must be between 6 and 30 characters");
                                return;
                            }
                        }
                        else
                        {
                            //return error message
                            Jeeves.ShowMessage("Error", "Password must contain a capital letter, a number, and a special character");
                            return;
                        }
                    }
                    else
                    {
                        //return error message
                        Jeeves.ShowMessage("Error", "Password does not match");
                        return;
                    }

                    //set username
                    newUser.UserName = txtUserName.Text;

                    //make new Guid for ID
                    newUser.ID = Guid.NewGuid().ToString();

                    //audit fields
                    newUser.CreatedBy = creator.UserName;
                    newUser.CreatedDate = DateTime.Now;
                    newUser.LastModifiedBy = creator.UserName;
                    newUser.LastModifiedDate = DateTime.Now;

                    //add user
                    userRepository.AddUser(newUser);

                    //clear fields to allow new creation
                    ClearFields();

                    Frame.Navigate(typeof(UsersSummary), null, new SuppressNavigationTransitionInfo());
                }
                catch (Exception ex)
                {
                    //error if adding user was unsuccessful
                    Jeeves.ShowMessage("Error", ex.GetBaseException().Message.ToString());
                    return;
                }
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UsersSummary), null, new SuppressNavigationTransitionInfo());
        }
    }
}
     
