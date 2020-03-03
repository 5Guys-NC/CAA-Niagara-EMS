using CAA_Event_Management.Data;
using CAA_Event_Management.Models;
using CAA_Event_Management.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
/******************************
*  Model Created By: Nathan Smith
*  Edited by: Brian Culp
*******************************/

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace CAA_Event_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsersSummary : Page
    {
        #region Startup - variables. repositories, methods

        IUsersRepository usersRepository;

        public UsersSummary()
        {
            this.InitializeComponent();
            usersRepository = new UsersRepository();
            
            //Fill list of Users
            FillUserList();
        }

        /// <summary>
        /// Method to create a list of all Users in database
        /// </summary>
        private void FillUserList()
        {
            try
            {
                //get users
                List<User> users = usersRepository.GetUsers();

                //set source for the ListView to the list of users
                lvUsers.ItemsSource = users;
            }
            catch (Exception e)
            {
                //could not get list of users
                Jeeves.ShowMessage("Error", e.Message.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Details Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            UserParams up = new UserParams();
            
            //if there is a selected user
            if (lvUsers.SelectedItem != null)
            {
                //put selected user data in UserParams object
                up.selectedUser = (User)lvUsers.SelectedItem;
                //navigate to corresponding frame
                Frame.Navigate(typeof(UserDetails), up);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //Will this be needed, CAA does not require
        }

        /// <summary>
        /// Delete Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //get user
            var u = lvUsers.SelectedItem;
            //delete user
            usersRepository.DeleteUser((User)u);
            Frame.GoBack();
        }

        /// <summary>
        /// Create Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to CreateNewUser
            Frame.Navigate(typeof(CreateNewUser));
        }
    }
    
}